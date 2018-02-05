using System;
using MvcTomato.Models;

namespace MvcTomato.ViewModels
{
    public class HistoryDay
    {
        // TODO: move 7.5 to application properties (settings)
        public static TimeSpan rate = TimeSpan.FromHours(7.5);
        
        public HistoryDay(WorkingDay d)
        {
            Id = d.Id;
            Date = d.Date;
            Statistics = rate - (d.Exit - d.Enter - (d.DinnerFinish - d.DinnerStart));
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Statistics { get; set; }
    }
}