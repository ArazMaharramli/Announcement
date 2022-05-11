using System;
namespace WebAPI.Areas.Auth.Contracts.RequestModel
{
    public class RegisterRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
