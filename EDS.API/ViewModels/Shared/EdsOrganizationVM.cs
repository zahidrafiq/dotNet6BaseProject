using Core.Repository.Infrastructure;
using EDS.DB.Model.EF.Models;
using EDS.Services.ServiceModels;

namespace EDS.API.ViewModels.Shared
{
    public class EdsOrganizationVM: BaseAutoViewModel<EdsOrganizationSM, EdsOrganizationVM>
    {
        public int OrganizationId { get; set; }
        public string OrganizationCode { get; set; } = null!;

        public string? ParentOrganizationCode { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public ICollection<EdsClient> EdsClients { get; set; } = new List<EdsClient>();
        public ObjectState ObjectState { get; set; }
    }
}
