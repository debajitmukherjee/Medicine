using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine.API.Contracts
{
    public class GenericInBrand
    {
        [Key, Column(Order = 0)]
        public int BrandId { get; set; }

        [Key, Column(Order = 1)]
        public int GenericId { get; set; }

        public string Unit { get; set; }

        public bool IsSingleGeneric { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        [ForeignKey("GenericId")]
        public virtual Generic Generic { get; set; }

    }
}
