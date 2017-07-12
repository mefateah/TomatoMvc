using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcTomato.Models
{
    public class WorkingDay
    {
        // TODO: investigate date and time types in SQL DB in order to choose right type in .NET
        [Key]
        public int Id { get; set; }
        [Index(IsUnique = true)]
        public DateTime Date { get; set; }
        public TimeSpan? Enter { get; set; }
        public TimeSpan? Exit { get; set; }
        public TimeSpan? DinnerStart { get; set; }
        public TimeSpan? DinnerFinish { get; set; }
        public bool Finished { get; set; }
    }
}