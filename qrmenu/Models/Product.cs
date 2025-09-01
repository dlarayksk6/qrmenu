using System.ComponentModel.DataAnnotations.Schema;

namespace qrmenu.Models
{
    public class Product : BaseEntity
    {


        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
    }

}
