using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine.API.Contracts
{
    public class Generic : BaseEntity
    {
        public string Overview { get; set; }

        public string SideEffects { get; set; }

        public virtual ICollection<GenericInBrand> Brands { get; set; }
    }
}
