/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System.Collections.Generic;

namespace Blazr.Database.Core
{
    public static class WeatherSummaries
    {
        public static readonly string[] Summaries = new[] {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

        public static List<string> SummariesList
        {
            get
            {
                var list = new List<string>();
                list.AddRange(WeatherSummaries.Summaries);
                return list;
            }
        }
    }
}
