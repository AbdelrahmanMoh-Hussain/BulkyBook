using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public decimal ListPrice { get; set; }

        [Required]
        [Display(Name = "Price for 1 - 50")]
        [Range(1, 1000)]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]
        public decimal Price50 { get; set; }
        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public decimal Price100 { get; set; }

        //[Range(1, 100, ErrorMessage = "Quantity must be between 1 - 100.")]
        //[Required(ErrorMessage = "The Quantity field is required")]
        //public int Quantity { get; set; }

        //[Display(Name = "Category Id")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
		[ValidateNever]

		public string? ImageUrl { get; set; }
    }
}
