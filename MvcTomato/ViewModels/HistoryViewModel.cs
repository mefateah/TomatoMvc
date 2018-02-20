using System;
using System.Collections.Generic;

namespace MvcTomato.ViewModels
{
    public class HistoryViewModel
    {
        public DateTime Date { get; set; }
        public IEnumerable<HistoryDay> Days { get; set; }
        public TimeSpan? TotalWorkedTime { get; set; }
        public TimeSpan? TotalWorkedTimeWithDinner { get; set; }
        public IEnumerable<int> AvailableYears { get; set; }
    }
}