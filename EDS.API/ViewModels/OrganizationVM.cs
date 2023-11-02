using Core.DataMapper;
using Core.Repository.Infrastructure;
using EDS.API.ViewModels.Shared;
using EDS.Services.ServiceModels;

namespace EDS.API.ViewModels
{
    public class OrganizationVM : BaseAutoViewModel<OrganizationSM, OrganizationVM>
    {
        public int OrganizationId { get; set; }
        public string OrganizationCode { get; set; } = null!;
        public string? ParentOrganizationCode { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<ClientVM>? EdsClients { get; set; }
    }
}
