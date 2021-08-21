using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrmApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ImageSource { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public IList<Category> Categories { get; set; }
    }
}