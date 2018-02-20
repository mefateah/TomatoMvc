using System;
using MvcTomato.Models;

namespace MvcTomato.ViewModels
{
    public class HistoryDay
    {
        // TODO: move to application properties (settings)
        private static readonly TimeSpan Rate = TimeSpan.FromHours(8);
        private static readonly TimeSpan Dinner = TimeSpan.FromMinutes(30);
        
        public HistoryDay(WorkingDay d)
        {
            Id = d.Id;
            Date = d.Date;
            var dinner = d.DinnerFinish - d.DinnerStart;
            if (dinner < Dinner)
            {
                dinner = TimeSpan.Zero;
            }
            else
            {
                dinner = dinner - Dinner;
            }
            WorkedTime = d.Exit - d.Enter - dinner;
            Statistics = Rate - WorkedTime;
            WorkedTimeWithDinner = d.Exit - d.Enter;
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Statistics { get; set; }
        public TimeSpan? WorkedTime { get; set; }
        public TimeSpan? WorkedTimeWithDinner { get; set; }
    }
}