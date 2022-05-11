namespace WebAPI.Areas.Auth.Contracts.RequestModel
{
    public class ConfirmEmailRequestModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public string UserType { get; set; }
    }
}
