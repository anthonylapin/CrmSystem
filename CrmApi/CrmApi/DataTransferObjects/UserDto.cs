using System.ComponentModel.DataAnnotations;

namespace CrmApi.DataTransferObjects
{
    public class UserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}