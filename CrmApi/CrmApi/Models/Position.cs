using System.ComponentModel.DataAnnotations;

namespace CrmApi.Models
{
    public class Position
    {
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required]
        public double Cost { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
    }
}