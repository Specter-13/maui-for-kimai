using FluentValidation;
using MauiForKimai.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Validators;
public class TimesheetFavouriteEntityValidator : AbstractValidator<TimesheetFavouriteEntity>
{
    public TimesheetFavouriteEntityValidator()
    {
        RuleFor(x => x.ActivityId).NotEmpty().WithMessage("Activity is required");
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project is required");
        RuleFor(x => x.Tags).MinimumLength(2).WithMessage("Tag must be at least 2 characters long");
    }
}