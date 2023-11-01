using EDS.Services.ServiceModels;
using EDS.API.ViewModels.Shared;

namespace EDS.API.ViewModels
{
    public class ClientVM : BaseAutoViewModel<EdsClientSM, ClientVM>
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
    }
}
