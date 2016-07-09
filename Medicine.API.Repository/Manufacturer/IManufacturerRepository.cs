using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine.API.Repository.Manufacturer
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManufacturerRepository
    {
        /// <summary>
        /// Adds the specified manufaturers.
        /// </summary>
        /// <param name="manufaturers">The manufaturers.</param>
        /// <returns></returns>
        bool Add(List<Contracts.Manufacturer> manufaturers);
    }
}
