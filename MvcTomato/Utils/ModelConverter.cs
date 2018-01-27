using System;
using MvcTomato.Models;
using MvcTomato.ViewModels;
using System.Collections.Generic;

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

        public static IEnumerable<WorkingDayViewModel> ToViewModels(IEnumerable<WorkingDay> days)
        {
            var result = new List<WorkingDayViewModel>();
            foreach (var day in days)
            {
                result.Add(ToViewModel(day));
            }
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