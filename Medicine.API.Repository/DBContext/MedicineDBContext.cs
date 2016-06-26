using Medicine.API.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine.API.Repository.DBContext
{
    public class MedicineDbContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MedicineDbContext()
            : base("DBConnection")
        {
            
        }

        #region DB Set

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Generic> Generics { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<GenericInBrand> GenericInBrand { get; set; }

        #endregion


        /// <summary>
        /// On Model Creating event
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Method intentionally left empty.
        }
    }
}
