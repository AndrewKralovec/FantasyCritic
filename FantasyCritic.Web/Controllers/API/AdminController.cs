using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FantasyCritic.Lib.Domain;
using FantasyCritic.Lib.Enums;
using FantasyCritic.Lib.OpenCritic;
using FantasyCritic.Lib.Services;
using FantasyCritic.Web.Models;
using FantasyCritic.Web.Models.Requests;
using FantasyCritic.Web.Models.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace FantasyCritic.Web.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;
        private readonly FantasyCriticService _fantasyCriticService;
        private readonly InterLeagueService _interLeagueService;
        private readonly IOpenCriticService _openCriticService;
        private readonly IClock _clock;

        public AdminController(AdminService adminService, FantasyCriticService fantasyCriticService, IOpenCriticService openCriticService, IClock clock, InterLeagueService interLeagueService)
        {
            _adminService = adminService;
            _fantasyCriticService = fantasyCriticService;
            _openCriticService = openCriticService;
            _clock = clock;
            _interLeagueService = interLeagueService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMasterGame([FromBody] CreateMasterGameRequest viewModel)
        {
            EligibilityLevel eligibilityLevel = await _interLeagueService.GetEligibilityLevel(viewModel.EligibilityLevel);
            Instant instant = _clock.GetCurrentInstant();
            var currentYear = await _interLeagueService.GetCurrentYear();
            MasterGame masterGame = viewModel.ToDomain(eligibilityLevel, instant, currentYear);
            await _interLeagueService.CreateMasterGame(masterGame);
            await Task.Delay(1000);
            await _adminService.RefreshCaches();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CompleteMasterGameRequest([FromBody] CompleteMasterGameRequestRequest request)
        {
            Maybe<MasterGameRequest> maybeRequest = await _interLeagueService.GetMasterGameRequest(request.RequestID);
            if (maybeRequest.HasNoValue)
            {
                return BadRequest("That request does not exist.");
            }

            Maybe<MasterGame> masterGame = Maybe<MasterGame>.None;
            if (request.MasterGameID.HasValue)
            {
                masterGame = await _interLeagueService.GetMasterGame(request.MasterGameID.Value);
                if (masterGame.HasNoValue)
                {
                    return BadRequest("Bad master game");
                }
            }

            Instant instant = _clock.GetCurrentInstant();
            await _interLeagueService.CompleteMasterGameRequest(maybeRequest.Value, instant, request.ResponseNote, masterGame);

            return Ok();
        }

        public async Task<ActionResult<List<MasterGameRequestViewModel>>> ActiveMasterGameRequests()
        {
            IReadOnlyList<MasterGameRequest> requests = await _interLeagueService.GetAllMasterGameRequests();

            var viewModels = requests.Select(x => new MasterGameRequestViewModel(x, _clock)).ToList();
            return viewModels;
        }

        [HttpPost]
        public async Task<IActionResult> RefreshCriticInfo()
        {
            await _adminService.RefreshCriticInfo();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetOpenCriticGames([FromBody] OpenCriticSearchRequest request)
        {
            int id = request.MinimumOpenCriticID;
            var masterGames = await _interLeagueService.GetMasterGames();
            while (true)
            {
                var openCriticGame = await _openCriticService.GetOpenCriticGame(id);
                if (openCriticGame.HasNoValue)
                {
                    return Ok();
                }

                if (masterGames.Any(x => x.OpenCriticID.HasValue && x.OpenCriticID.Value == id))
                {
                    id++;
                    continue;
                }

                EligibilityLevel eligibilityLevel = await _interLeagueService.GetEligibilityLevel(0);
                int minimumReleaseYear = 2018;
                if (openCriticGame.Value.ReleaseDate.HasValue)
                {
                    minimumReleaseYear = openCriticGame.Value.ReleaseDate.Value.Year;
                }

                MasterGame masterGame = new MasterGame(Guid.NewGuid(), openCriticGame.Value.Name, openCriticGame.Value.ReleaseDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    openCriticGame.Value.ReleaseDate, id, openCriticGame.Value.Score, minimumReleaseYear, eligibilityLevel, false, false, "", null, false, _clock.GetCurrentInstant());
                await _interLeagueService.CreateMasterGame(masterGame);

                id++;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFantasyPoints()
        {
            var systemWideValues = await _interLeagueService.GetSystemWideValues();
            var supportedYears = await _interLeagueService.GetSupportedYears();
            foreach (var supportedYear in supportedYears)
            {
                if (supportedYear.Finished || !supportedYear.OpenForPlay)
                {
                    continue;
                }

                await _fantasyCriticService.UpdateFantasyPoints(systemWideValues, supportedYear.Year);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPickups()
        {
            var systemWideSettings = await _interLeagueService.GetSystemWideSettings();
            if (!systemWideSettings.BidProcessingMode)
            {
                return BadRequest("Turn on bid processing mode first.");
            }

            SystemWideValues systemWideValues = await _interLeagueService.GetSystemWideValues();
            var supportedYears = await _interLeagueService.GetSupportedYears();
            foreach (var supportedYear in supportedYears)
            {
                if (supportedYear.Finished || !supportedYear.OpenForPlay)
                {
                    continue;
                }

                await _fantasyCriticService.ProcessPickups(systemWideValues, supportedYear.Year);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLeague([FromBody] DeleteLeagueRequest request)
        {
            Maybe<League> league = await _fantasyCriticService.GetLeagueByID(request.LeagueID);
            if (league.HasNoValue)
            {
                return BadRequest();
            }

            if (!league.Value.TestLeague)
            {
                return BadRequest();
            }

            await _fantasyCriticService.DeleteLeague(league.Value);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> TurnOnBidProcessingMode()
        {
            await _interLeagueService.SetBidProcessingMode(true);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> TurnOffBidProcessingMode()
        {
            await _interLeagueService.SetBidProcessingMode(false);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RefreshCaches()
        {
            await _adminService.RefreshCaches();
            return Ok();
        }
    }
}
