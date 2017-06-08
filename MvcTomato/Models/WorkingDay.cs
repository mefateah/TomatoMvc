using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTomato.Models
{
    public class WorkingDay
    {
        // TODO: investigate date and time types in SQL DB in order to choose right type in .NET
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Enter { get; set; }
        public TimeSpan Exit { get; set; }
        public TimeSpan DinnerStart { get; set; }
        public TimeSpan DinnerFinish { get; set; }
    }
}