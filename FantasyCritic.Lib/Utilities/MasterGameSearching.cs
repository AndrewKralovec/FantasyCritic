﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyCritic.Lib.Domain;
using FuzzyString;

namespace FantasyCritic.Lib.Utilities
{
    public static class MasterGameSearching
    {
        private static readonly int MaxDistanceGames = 5;

        public static IReadOnlyList<MasterGame> SearchMasterGames(string gameName, IEnumerable<MasterGame> masterGames)
        {
            gameName = gameName.ToLower();
            var distances = masterGames
                .Select(x => new Tuple<MasterGame, double>(x, GetDistance(gameName, x.GameName)));

            var lowDistance = distances.OrderBy(x => x.Item2).Select(x => x.Item1).Take(MaxDistanceGames);

            var matchingMasterGames = masterGames
                .Where(x => x.GameName.ToLower().Contains(gameName))
                .Concat(lowDistance).Distinct();

            return matchingMasterGames.ToList();
        }

        public static IReadOnlyList<MasterGameYear> SearchMasterGameYears(string gameName, IEnumerable<MasterGameYear> masterGames)
        {
            gameName = gameName.ToLower();
            var distances = masterGames
                .Select(x => new Tuple<MasterGameYear, double>(x, GetDistance(gameName, x.MasterGame.GameName)));

            var lowDistance = distances.OrderBy(x => x.Item2).Select(x => x.Item1).Take(MaxDistanceGames);

            var matchingMasterGames = masterGames
                .Where(x => x.MasterGame.GameName.ToLower().Contains(gameName))
                .Concat(lowDistance).Distinct();

            return matchingMasterGames.ToList();
        }

        private static double GetDistance(string source, string target)
        {
            var longestCommon = source.LongestCommonSubsequence(target);
            return longestCommon.Length;
        }

        //https://stackoverflow.com/a/9453762
        private static int CalculateLevenshteinDistance(this string a, string b)
        {
            if (string.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
            {
                return 0;
            }
            if (string.IsNullOrEmpty(a))
            {
                return b.Length;
            }
            if (String.IsNullOrEmpty(b))
            {
                return a.Length;
            }
            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
            for (int j = 1; j <= lengthB; j++)
            {
                int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                distances[i, j] = Math.Min
                (
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost
                );
            }
            return distances[lengthA, lengthB];
        }
    }
}
