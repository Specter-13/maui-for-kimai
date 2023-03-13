using CommunityToolkit.Mvvm.ComponentModel;
using MauiForKimai.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Authentication;

public partial class AuthHandler : DelegatingHandler
{
    public const string AUTHENTICATED_CLIENT = nameof(AUTHENTICATED_CLIENT);

    private readonly ApiStateProvider _apiStateProvider; 
    public AuthHandler(ApiStateProvider apiStateProvider)
    {
        _apiStateProvider = apiStateProvider;

    }

    //ovveride send async to add authorization
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
 
        request.Headers.Add("X-AUTH-USER", _apiStateProvider.UserName);
        request.Headers.Add("X-AUTH-TOKEN", _apiStateProvider.ApiPassword);
       
        return await base.SendAsync(request, cancellationToken);
    }
}
