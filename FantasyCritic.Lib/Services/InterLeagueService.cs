﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FantasyCritic.Lib.Domain;
using FantasyCritic.Lib.Interfaces;
using FantasyCritic.Lib.OpenCritic;
using MoreLinq;
using NodaTime;

namespace FantasyCritic.Lib.Services
{
    public class InterLeagueService
    {
        private readonly IFantasyCriticRepo _fantasyCriticRepo;
        private readonly IMasterGameRepo _masterGameRepo;

        public InterLeagueService(IFantasyCriticRepo fantasyCriticRepo, IMasterGameRepo masterGameRepo)
        {
            _fantasyCriticRepo = fantasyCriticRepo;
            _masterGameRepo = masterGameRepo;
        }

        public Task<SystemWideSettings> GetSystemWideSettings()
        {
            return _fantasyCriticRepo.GetSystemWideSettings();
        }

        public Task<SystemWideValues> GetSystemWideValues()
        {
            return _fantasyCriticRepo.GetSystemWideValues();
        }

        public Task<SiteCounts> GetSiteCounts()
        {
            return _fantasyCriticRepo.GetSiteCounts();
        }

        public Task CreateMasterGame(MasterGame masterGame)
        {
            return _masterGameRepo.CreateMasterGame(masterGame);
        }

        public Task<IReadOnlyList<SupportedYear>> GetSupportedYears()
        {
            return _fantasyCriticRepo.GetSupportedYears();
        }

        public async Task<SupportedYear> GetCurrentYear()
        {
            var supportedYears = await _fantasyCriticRepo.GetSupportedYears();
            return supportedYears.Where(x => x.OpenForPlay).MinBy(x => x.Year).Single();
        }

        public Task<IReadOnlyList<MasterGame>> GetMasterGames()
        {
            return _masterGameRepo.GetMasterGames();
        }

        public Task<IReadOnlyList<MasterGameYear>> GetMasterGameYears(int year, bool useCache)
        {
            return _masterGameRepo.GetMasterGameYears(year, useCache);
        }

        public Task<Maybe<MasterGame>> GetMasterGame(Guid masterGameID)
        {
            return _masterGameRepo.GetMasterGame(masterGameID);
        }

        public Task<Maybe<MasterGameYear>> GetMasterGameYear(Guid masterGameID, int year)
        {
            return _masterGameRepo.GetMasterGameYear(masterGameID, year);
        }

        public Task<IReadOnlyList<Guid>> GetAllSelectedMasterGameIDsForYear(int year)
        {
            return _masterGameRepo.GetAllSelectedMasterGameIDsForYear(year);
        }

        public Task UpdateCriticStats(MasterGame masterGame, OpenCriticGame openCriticGame)
        {
            return _masterGameRepo.UpdateCriticStats(masterGame, openCriticGame);
        }

        public Task UpdateCriticStats(MasterSubGame masterSubGame, OpenCriticGame openCriticGame)
        {
            return _masterGameRepo.UpdateCriticStats(masterSubGame, openCriticGame);
        }

        public Task<EligibilityLevel> GetEligibilityLevel(int eligibilityLevel)
        {
            return _masterGameRepo.GetEligibilityLevel(eligibilityLevel);
        }

        public Task<IReadOnlyList<EligibilityLevel>> GetEligibilityLevels()
        {
            return _masterGameRepo.GetEligibilityLevels();
        }

        public Task SetBidProcessingMode(bool modeOn)
        {
            return _fantasyCriticRepo.SetBidProcessingMode(modeOn);
        }

        public Task CreateMasterGameRequest(MasterGameRequest domainRequest)
        {
            return _masterGameRepo.CreateMasterGameRequest(domainRequest);
        }

        public Task CreateMasterGameChangeRequest(MasterGameChangeRequest domainRequest)
        {
            return _masterGameRepo.CreateMasterGameChangeRequest(domainRequest);
        }

        public Task<IReadOnlyList<MasterGameRequest>> GetAllMasterGameRequests()
        {
            return _masterGameRepo.GetAllMasterGameRequests();
        }

        public Task<IReadOnlyList<MasterGameChangeRequest>> GetAllMasterGameChangeRequests()
        {
            return _masterGameRepo.GetAllMasterGameChangeRequests();
        }

        public Task<IReadOnlyList<MasterGameRequest>> GetMasterGameRequestsForUser(FantasyCriticUser user)
        {
            return _masterGameRepo.GetMasterGameRequestsForUser(user);
        }

        public Task<IReadOnlyList<MasterGameChangeRequest>> GetMasterGameChangeRequestsForUser(FantasyCriticUser user)
        {
            return _masterGameRepo.GetMasterGameChangeRequestsForUser(user);
        }

        public Task<Maybe<MasterGameRequest>> GetMasterGameRequest(Guid requestID)
        {
            return _masterGameRepo.GetMasterGameRequest(requestID);
        }

        public Task<Maybe<MasterGameChangeRequest>> GetMasterGameChangeRequest(Guid requestID)
        {
            return _masterGameRepo.GetMasterGameChangeRequest(requestID);
        }

        public Task DeleteMasterGameRequest(MasterGameRequest request)
        {
            return _masterGameRepo.DeleteMasterGameRequest(request);
        }

        public Task DeleteMasterGameChangeRequest(MasterGameChangeRequest request)
        {
            return _masterGameRepo.DeleteMasterGameChangeRequest(request);
        }

        public Task CompleteMasterGameRequest(MasterGameRequest masterGameRequest, Instant responseTime, string responseNote, Maybe<MasterGame> masterGame)
        {
            return _masterGameRepo.CompleteMasterGameRequest(masterGameRequest, responseTime, responseNote, masterGame);
        }

        public  Task CompleteMasterGameChangeRequest(MasterGameChangeRequest masterGameRequest, Instant responseTime, string responseNote)
        {
            return _masterGameRepo.CompleteMasterGameChangeRequest(masterGameRequest, responseTime, responseNote);
        }

        public Task DismissMasterGameRequest(MasterGameRequest masterGameRequest)
        {
            return _masterGameRepo.DismissMasterGameRequest(masterGameRequest);
        }

        public Task DismissMasterGameChangeRequest(MasterGameChangeRequest request)
        {
            return _masterGameRepo.DismissMasterGameChangeRequest(request);
        }

        public Task LinkToOpenCritic(MasterGame masterGame, int openCriticID)
        {
            return _masterGameRepo.LinkToOpenCritic(masterGame, openCriticID);
        }

        public Task UpdateSystemWideValues(SystemWideValues systemWideValues)
        {
            return _fantasyCriticRepo.UpdateSystemWideValues(systemWideValues);
        }
    }
}
