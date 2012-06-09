using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PoliceSMS.Lib.Report
{
    public class StationReportResult
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string OfficerName { get; set; }
        public int? UpRank { get; set; }
        public int Rank { get; set; }
        public double StationRate { get; set; }
        public double Score { get; set; }

        public int TotalCount { get; set; }

        public int G1Count { get; set; }
        public double G1Rate { get; set; }

        public int G2Count { get; set; }
        public double G2Rate { get; set; }

        public int G3Count { get; set; }
        public double G3Rate { get; set; }

        public int G4Count { get; set; }
        public double G4Rate { get; set; }

        public int G5Count { get; set; }
        public double G5Rate { get; set; }

        public double Rate { get; set; }

        public int G6Count { get; set; }
        public double G6Rate { get; set; }

        public int G7Count { get; set; }
        public double G7Rate { get; set; }

        public int TotalScore { get; set; }

    }
}
