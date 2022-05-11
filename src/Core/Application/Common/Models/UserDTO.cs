namespace Application.Common.Models
{
    public class UserDTO
    {
        public UserDTO(string id, string userName, string email, string phone)
        {
            UserName = userName;
            Id = id;
            Email = email;
            Phone = phone;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
