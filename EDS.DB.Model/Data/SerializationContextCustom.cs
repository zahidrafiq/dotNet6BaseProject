using EDS.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Repository.DataContext;

namespace EDS.DB.Model.Data
{
    public partial class SerializationContext : DataContext
    {
        private readonly TenantConfig currentTenant;
        private static string DBConnectionString { get; set; }

        public SerializationContext(TenantConfig _currentTenant)
        {
            currentTenant = _currentTenant;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (currentTenant != null)
                {
                    optionsBuilder.UseSqlServer(currentTenant.EdsDbConnectionString);
                    DBConnectionString = currentTenant.EdsDbConnectionString;
                }
                else
                {
                    // if DBContext is not added through DI using AddDBContext, then get Connection string from the variable instead of hard-coded connection string
                    optionsBuilder.UseSqlServer(DBConnectionString);
                }
                //optionsBuilder.AddInterceptors(new DBIntercepter());
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}
