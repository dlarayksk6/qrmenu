namespace qrmenu.Models
{
    public class Category:BaseEntity
    {
       
        public int DisplayOrder { get; set; }  

        public string? ImageUrl { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

}
