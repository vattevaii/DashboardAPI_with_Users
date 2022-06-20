using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DashboardAPI.Models
{
    public class Product
    {

        public Product(ProductAddModel product, User user)
        {
            Name = product.Name;
            Description = product.Description;
            MarkedPrice = product.MarkedPrice;
            Category = product.Category;
            belongsTo = user;
        }
        public Product()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MarkedPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public Categories Category { get; set; }
        public int Stock { get; set; }
        public User belongsTo { get; set; }
        public IEnumerable<Orders> Orders { get; set; }
    }
    
    public enum Categories
    {
        Electronics,
        Fashion,
        Books,
        Sports,
        Home,
        Automobiles,
        Others
    }
    public class ProductAddModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MarkedPrice { get; set; }
        public Categories Category { get; set; }
    }
}
