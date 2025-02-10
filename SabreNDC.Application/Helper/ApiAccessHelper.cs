using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using SabreNDC.Application.Dtos.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TripLover.AirCommonModels;
using ZstdSharp.Unsafe;

namespace SabreNDC.Application.Helper;

public abstract class ApiAccessHelper
{
    public static IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
    private static readonly MDBSetting _mdbSetting;
    private static readonly AuthLogService _authLogService;
    static ApiAccessHelper()
    {
        _mdbSetting = new MDBSetting()
        {
            Connection = config.GetSection("mongo:Connection").Value,
            DatabaseName = config.GetSection("mongo:DataBaseName").Value
        };
        _authLogService = new AuthLogService(_mdbSetting);
    }
    public static async Task<string> GetAccessToken(ACMApiCredential apiCredential, string UniqueTransID, bool newToken = false)
    {
        var isLive = apiCredential?.IsLive ?? false;
        if (isLive)
        {
            var lastToken = await ResolveLastTokenAsync(apiCredential.UserName, isLive);
            if (lastToken != null && newToken == false)
            {
                return lastToken;
            }
        }

        var authRequest = new AuthRequest() { ClientID = $"V1:{apiCredential.UserName}:{apiCredential.PCC}:AA", ClientSecret = $"{apiCredential.Password.Trim()}" };
        FileHelper.ToWriteJson($"Auth-{UniqueTransID}-Req", "Auth", JsonConvert.SerializeObject(authRequest));
        var authResponseJson = await SabreAuthRequest(authRequest, apiCredential.ServiceUrl, "v2/auth/token");
        var authResObj = JsonConvert.DeserializeObject<AuthResponse>(authResponseJson);
        if (authResObj != null && string.IsNullOrEmpty(authResObj.error))
        {
            FileHelper.ToWriteJson($"Auth-{UniqueTransID}-Rsp", "Auth", authResponseJson);
            if (isLive)
            {
                await _authLogService.CreateAsync(new AuthLog()
                {
                    userName = apiCredential.UserName,
                    token = authResObj.access_token,
                    validUntil = DateTime.Now.AddSeconds(authResObj.expires_in),
                    CreatedAt = DateTime.Now,
                    IsLive = isLive

                });
            }

            return authResObj.access_token;
        }
        else
        {
            FileHelper.ToWriteJson($"Auth-{UniqueTransID}-Rsp", "Auth", authResponseJson);
            return null;
        }
    }
    private static async Task<string> ResolveLastTokenAsync(string userName, bool isLive = false)
    {
        var lastToken = await _authLogService.GetLastWithUserNameAsync(userName, isLive);
        if (lastToken?.validUntil > DateTime.Now.AddHours(-6))
        {
            return lastToken.token;
        }
        return null;
    }
    public static async Task<string> SabreAuthRequest(AuthRequest model, string baseUrl, string methodUrl)
    {
        try
        {
            var client = new RestClient(baseUrl + methodUrl);
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("grant_type", "client_credentials");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            string encodedClientID = Convert.ToBase64String(Encoding.ASCII.GetBytes(model.ClientID));
            string encodedClientSecret = Convert.ToBase64String(Encoding.ASCII.GetBytes(model.ClientSecret));
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(encodedClientID + ":" + encodedClientSecret)));
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static async Task<string> SabrePostRequest<T>(T model, string token, string baseUrl, string methodUrl, bool removeNullWhenSerializing = false, string transId = "")
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string stringContent = string.Empty;

                #region Handle Null Serialization
                if (removeNullWhenSerializing == true)
                {
                    try
                    {
                        stringContent = JsonConvert.SerializeObject(
                            model,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

                        FileHelper.ToWriteJson(
                            $"{transId}-{nameof(T)}-Req",
                            "NullHandleLogs",
                            stringContent);
                    }
                    catch (Exception ex)
                    {
                        FileHelper.ToWriteJson(
                            $"{transId}-{nameof(T)}-Err",
                            "NullHandleLogs",
                            JsonConvert.SerializeObject(ex));

                        stringContent = JsonConvert.SerializeObject(model);
                    }
                }
                else
                {
                    stringContent = JsonConvert.SerializeObject(model);
                }
                #endregion Handle Null Serialization

                StringContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(baseUrl + methodUrl, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return apiResponse;
                }
            }
        }
        catch (Exception ex)
        {

            throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
        }
    }
}
