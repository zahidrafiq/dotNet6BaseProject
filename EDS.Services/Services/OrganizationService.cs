using EDS.Common.Utilities;
using EDS.DB.Model.EF.Models;
using EDS.Services.ServiceModels;
using EDS.Services.Services.Shared;
using HMIS.IM.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDS.Services.Services
{
    public class OrganizationService : BaseService<OrganizationSM, EdsOrganization>
    {
        private readonly TenantConfig _tenantConfig;

        public OrganizationService(TenantConfig tenantConfig) : base(tenantConfig)
        {
            _tenantConfig = tenantConfig;
        }
        public IEnumerable<OrganizationSM> GetAllOrganizations()
        {
            try
            {
                var orgListDbModel = uow.RepositoryAsync<EdsOrganization>().Queryable()
                    .Include(x=>x.EdsClients)
                  .Where(x => x.IsActive)
                  .ToList();
                var orgListSM = new OrganizationSM().FromDataModelList(orgListDbModel);
                return orgListSM;
            }
            catch (Exception exp)
            {

                throw;
            }
        }
    }
}
