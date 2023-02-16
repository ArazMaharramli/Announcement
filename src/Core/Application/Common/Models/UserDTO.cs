namespace Application.Common.Models;

public class UserDTO
{
    public UserDTO(string id, string name, string userName, string email, string phone, string profilePictureUrl)
    {
        UserName = userName;
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
    }

    public string Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ProfilePictureUrl { get; set; }
}
