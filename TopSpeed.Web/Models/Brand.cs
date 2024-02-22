using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TopSpeed.Web.Models
{
    public class Brand
    {
        [Key]   // Its an primary Key    Its Used the DataAnnotation library
        public int ID { get; set; }
        [Required]
        public string Name { get; set;}
        [Display(Name = "Established Year")]
        public int Established { get; set; }

        public string BrandLogo { get; set;  }

    }
}
