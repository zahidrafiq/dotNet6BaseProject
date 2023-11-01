﻿using Core.Repository.Infrastructure;
using EDS.DB.Model.EF.Models;
using EDS.Services.ServiceModels.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataMapper;

namespace EDS.Services.ServiceModels
{
    public class EdsOrganizationSM:BaseServiceModel<EdsOrganization,EdsOrganizationSM>,IObjectState
    {
        public int OrganizationId { get; set; }
        public string OrganizationCode { get; set; } = null!;

        public string? ParentOrganizationCode { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }
        [Complex]
        public ICollection<EdsClient> EdsClients { get; set; } = new List<EdsClient>();
        public ObjectState ObjectState { get; set; }
    }
}
