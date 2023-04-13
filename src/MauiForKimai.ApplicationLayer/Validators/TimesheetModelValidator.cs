using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Validators;

public class TimesheetModelStartValidator : AbstractValidator<TimesheetModel>
{
    public TimesheetModelStartValidator()
    {
        RuleFor(x => x.ActivityId).NotEmpty().WithMessage("Activity is required");
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project is required");
        RuleFor(x => x.Tags).MinimumLength(2).WithMessage("Tag must at least 2 characters long");
    }
}

public class TimesheetModelCreateValidator : AbstractValidator<TimesheetModel>
{
     public TimesheetModelCreateValidator()
    {
        RuleFor(x => x.Begin).LessThanOrEqualTo(x => x.End).WithMessage("The start date-time must be earlier than the end date-time.");
        RuleFor(x => x.End).GreaterThanOrEqualTo(x => x.Begin).WithMessage("The end date-time must be later than the start date-time.");
        RuleFor(x => x.ActivityId).NotEmpty().WithMessage("Activity is required");
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project is required");
        RuleFor(x => x.Tags).MinimumLength(2).WithMessage("Tag must at least 2 characters long");

        
    }
}
