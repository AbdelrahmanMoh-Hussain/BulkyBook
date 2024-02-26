using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
	public class Category
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(30)]
		[Display(Name = "Category Name")]
		public string Name { get; set; } = default!;

		[Display(Name = "Display Order")]
		[Range(1, 100, ErrorMessage = "Display Order must be between 1 - 100.")]
		[Required(ErrorMessage = "The Display Order field is required")]
		public int DisplayOrder { get; set; } //Which category display first

		[ValidateNever]
		public List<Product> Products { get; set; }
	}
}
