using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardAPI.Models
{
    public enum Roles
    {
        User, Admin
    }
    public class UserDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Roles role { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Orders> Orders { get; set; }
        public UserDTO toDTO()
        {
            return new UserDTO()
            {
                Username = this.Username,
                Password = "This is a secret you know.."
            };
        }
    }
    public class UserAction
    {
        public string UserName { get; set; } = string.Empty;
        public string _message { get; set; } = string.Empty;

        public UserAction(User u, string Message)
        {
            UserName = u.Username;
            _message = Message;
        }
    }
}

