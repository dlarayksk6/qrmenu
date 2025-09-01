using qrmenu.Models;

namespace qrmenu.Models
{
    public class Restaurant: BaseEntity
    {
 

        public string? LogoUrl { get; set; }
        public string? FacebookUrl { get; set; } = "";
        public string? InstagramUrl { get; set; } = "";
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}