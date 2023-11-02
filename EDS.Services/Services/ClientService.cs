using Core.Repository.Infrastructure;
using EDS.Common.Utilities;
using EDS.DB.Model.EF.Models;
using EDS.Services.ServiceModels;
using EDS.Services.ServiceModels.Shared;
using EDS.Services.Services.Shared;
using HMIS.IM.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EDS.Services.Services
{
    public class ClientService: BaseService<ClientSM, EdsClient>
    {
        ClientService _service;
        public ClientService(TenantConfig tenantConfig) : base(tenantConfig)
        {
        }
        public int CreateClient(ClientSM sm, out int code, out string message)
        {
            try
            {
                int? clientId = 0;
                //var statusLkps = new LookupService(_currentTenant).GetLookupByType(LookupConstants.GRN_STATUS_LKP_TYPE);
                
                //Add Grn Audit trail
                sm.ObjectState = ObjectState.Added;
            
                Insert(sm);
                int numOfRecord = uow.SaveChanges();

                clientId = Get()?.ClientId;
                //_logger.LogInformation($"CustomLog:CreateClient:Client Created, # of Record Inserted: {numOfRecord}, ClientId: {clientId}");

                if (clientId != null && clientId > 0)
                {
                    code = (int)HttpStatusCode.OK;
                    message = "Client Created successfully";
                }
                else
                {
                    message = "Failed to create Client";
                    code = 500;
                }
                return clientId.GetValueOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<FilterModel> GetAllClients()
        {
            try
            {
                var a =  uow.RepositoryAsync<EdsClient>().Queryable().ToList();
                var b =  uow.RepositoryAsync<EdsClient>().Queryable()
                    .Include(x=>x.Organization).ToList();
                var c =  uow.RepositoryAsync<EdsOrganization>().Queryable().ToList();
                var d =  uow.RepositoryAsync<EdsOrganization>().Queryable().
                    Include(x=>x.EdsClients).ToList();
                
                return uow.RepositoryAsync<EdsClient>().Queryable()
                  .Where(x => x.IsActive)
                    .Select(x => new FilterModel()
                    {
                        Id = x.ClientId,
                        Name = x.ClientName,
                        Code = x.ClientCode
                    })
                    .OrderBy(x => x.Name)
                    .ToList();
            }
            catch (Exception exp)
            {

                throw;
            }
        }
    }
}
