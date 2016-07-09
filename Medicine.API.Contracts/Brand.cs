using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine.API.Contracts
{
    public class Brand : BaseEntity
    {
        [MaxLength(50)]
        public string Type { get; set; }

        [MaxLength(50)]
        public string Dose { get; set; }

        [MaxLength(50)]
        public string PackageUnit { get; set; }

        public double Price { get; set; }

        public double PricePerUnit { get; set; }

        public bool HasSingleGeneric { get; set; }

        [Index]
        public int ManufacturerId { get; set; }

        [ForeignKey("ManufacturerId")]
        public virtual Manufacturer Manufacturer { get; set; }

        public virtual ICollection<GenericInBrand> Generics { get; set; }
    }
}
