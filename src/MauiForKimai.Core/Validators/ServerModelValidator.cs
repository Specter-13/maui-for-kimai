using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Validators;
public class ServerModelValidator :  AbstractValidator<ServerModel>
{
    public ServerModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Url).NotEmpty().Must(BeAValidUrl).WithMessage("Invalid Url format");
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.ApiPasswordKey).NotEmpty().WithMessage("Api password is required");
    }

    private bool BeAValidUrl(string url)
    {
        // Check if the input string is a valid URL using Uri.IsWellFormedUriString method
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
