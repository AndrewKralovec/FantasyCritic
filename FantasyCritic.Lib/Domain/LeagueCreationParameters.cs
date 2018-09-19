using System;
using System.Collections.Generic;
using System.Text;
using FantasyCritic.Lib.Domain.ScoringSystems;
using FantasyCritic.Lib.Enums;

namespace FantasyCritic.Lib.Domain
{
    public class LeagueCreationParameters
    {
        public LeagueCreationParameters(FantasyCriticUser manager, string leagueName, int draftGames, int waiverGames, int counterPicks, decimal estimatedGameScore, int initialYear,
            EligibilitySystem eligibilitySystem, DraftSystem draftSystem, WaiverSystem waiverSystem, ScoringSystem scoringSystem)
        {
            Manager = manager;
            LeagueName = leagueName;
            DraftGames = draftGames;
            WaiverGames = waiverGames;
            CounterPicks = counterPicks;
            EstimatedGameScore = estimatedGameScore;
            InitialYear = initialYear;
            EligibilitySystem = eligibilitySystem;
            DraftSystem = draftSystem;
            WaiverSystem = waiverSystem;
            ScoringSystem = scoringSystem;
        }

        public FantasyCriticUser Manager { get; }
        public string LeagueName { get; }
        public int DraftGames { get; }
        public int WaiverGames { get; }
        public int CounterPicks { get; }
        public decimal EstimatedGameScore { get; }
        public int InitialYear { get; }
        public EligibilitySystem EligibilitySystem { get; }
        public DraftSystem DraftSystem { get; }
        public WaiverSystem WaiverSystem { get; }
        public ScoringSystem ScoringSystem { get; }
    }
}
