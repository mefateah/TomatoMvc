using System;
using System.Collections.Generic;

namespace MvcTomato.ViewModels
{
    public class StatisticsViewModel
    {
        public WorkingDayViewModel Day { get; set; }
        public IEnumerable<WorkingDayViewModel> UncompletedDays { get; set; }
        public TimeSpan? TodayStatistic { get; set; }
        public TimeSpan? MonthSum { get; set; }
        public TimeSpan? MonthStatistic { get; set; }
    }
}