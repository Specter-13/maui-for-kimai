using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Validators;
public class TimesheetEditFormValidator : AbstractValidator<TimesheetEditForm>
{
     public TimesheetEditFormValidator()
    {
        RuleFor(x => x.Begin).NotEmpty().WithMessage("Begin time is required");
        RuleFor(x => x.End).NotEmpty().WithMessage("End time is required");
        RuleFor(x => x.Activity).NotEmpty().WithMessage("Activity is required");
        RuleFor(x => x.Project).NotEmpty().WithMessage("Project is required");
        RuleFor(x => x.Tags).MinimumLength(2).WithMessage("Tag must at least 2 characters long");
    }
}
