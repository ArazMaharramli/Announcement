using System;
namespace WebAPI.Areas.Auth.Contracts.ResponseModels
{
    public class AuthResponseModel
    {
        public AuthResponseModel(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
