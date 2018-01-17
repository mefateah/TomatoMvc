using System;
using MvcTomato.Models;
using MvcTomato.ViewModels;

namespace MvcTomato.Utils
{
    public class ModelConverter
    {
        public static WorkingDayViewModel ToViewModel(WorkingDay day)
        {
            var result = new WorkingDayViewModel
            {
                Id = day.Id,
                Date = day.Date,
                Enter = day.Enter,
                Exit = day.Exit,
                DinnerStart = day.DinnerStart,
                DinnerFinish = day.DinnerFinish,
                Finished = day.Finished
            };
            return result;
        }

        internal static WorkingDay ToModel(WorkingDayViewModel day)
        {
            var result = new WorkingDay
            {
                Id = day.Id,
                Date = day.Date,
                Enter = day.Enter,
                Exit = day.Exit,
                DinnerStart = day.DinnerStart,
                DinnerFinish = day.DinnerFinish,
                Finished = day.Finished
            };
            return result;
        }
    }
}