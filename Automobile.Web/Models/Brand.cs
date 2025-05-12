using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Automobile.Web.Models
{
    public class Brand
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        
        [DisplayName("Established Year")]
        [Required]
        public int EstablishedYear { get; set; }

        [DisplayName("Brand Logo")]
        public string BrandLogo { get; set; }

    }
}
