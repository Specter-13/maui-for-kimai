using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Authentication;
public class AuthHandler : DelegatingHandler
{
    public const string AUTHENTICATED_CLIENT = nameof(AUTHENTICATED_CLIENT);

    private string _userName = string.Empty;
    private string _apiPassword = string.Empty;
    private string _baseUrl = string.Empty;
    public void SetBaseUrl(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public string GetBaseUrl() => _baseUrl;
    public void SetAccessTokens(string token, string apiPassword)
    {
        _userName = token;
        _apiPassword = apiPassword;
    }

    public string GetUserNameToken() => _userName;
    public string GetApiPasswordToken()  => _apiPassword;

    //ovveride send async to add authorization
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
 
        request.Headers.Add("X-AUTH-USER", _userName);
        request.Headers.Add("X-AUTH-TOKEN", _apiPassword);
       
        return await base.SendAsync(request, cancellationToken);
    }
}
