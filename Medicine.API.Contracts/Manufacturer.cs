using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine.API.Contracts
{
    public class Manufacturer : BaseEntity
    {
        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string PhoneNo { get; set; }

        [MaxLength(50)]
        public string Fax { get; set; }
    }
}
