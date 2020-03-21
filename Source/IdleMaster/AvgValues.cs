using System.Collections.Generic;

namespace IdleMaster
{
    public class EnhancedSteamHelper
    {
        //ReSharper disable once InconsistentNaming
        public List<Avg> AvgValues { get; set; }
    }

    public class Avg
    {
        public string AppId { get; set; }

        //ReSharper disable once InconsistentNaming
        public double AvgPrice { get; set; }
    }
}