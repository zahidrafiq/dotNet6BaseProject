using Core.DataMapper;
using Core.Repository.Infrastructure;
using EDS.DB.Model.EF.Models;
using EDS.Services.ServiceModels.Shared;

namespace EDS.Services.ServiceModels
{
    public class EdsClientSM : BaseServiceModel<EdsClient, EdsClientSM>, IObjectState
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; } = null!;

        public string ClientCode { get; set; } = null!;
        public int OrgsOrganizationId { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        [Complex]
        public EdsOrganization EdsOrganization { get; set; }
        public ObjectState ObjectState { get; set; }
    }
}
