using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CrmApi.Models
{
    public class User : IdentityUser
    {
        public IList<Category> Categories { get; set; }
        public IList<Position> Positions { get; set; }
    }
}