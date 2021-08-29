using Microsoft.AspNetCore.Http;

namespace CrmApi.Models
{
    public class CategoryModifyDto
    {
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }

        public void Deconstruct(out string? categoryName, out IFormFile? image)
        {
            categoryName = Name;
            image = Image;
        }
    }
}