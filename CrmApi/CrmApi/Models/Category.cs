﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrmApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ImageSource { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}