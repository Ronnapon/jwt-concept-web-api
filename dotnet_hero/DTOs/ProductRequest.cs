using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_hero.DTOs
{
    public class ProductRequest
    {
        public int? ProductId { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name, maximun length 100")]
        public string Name { get; set; }

        [Range(0, 10000)]
        public int Stock { get; set; }

        [Range(0, 1_000_000)]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public List<IFormFile> FormFiles { get; set; }
    }
}
