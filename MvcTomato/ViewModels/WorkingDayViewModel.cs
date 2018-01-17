using System;

namespace MvcTomato.ViewModels
{
    public class WorkingDayViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Enter { get; set; }
        public TimeSpan? Exit { get; set; }
        public TimeSpan? DinnerStart { get; set; }
        public TimeSpan? DinnerFinish { get; set; }
        public bool Finished { get; set; }
    }
}