using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EDS.DB.Model.EF.Models;

[Table("EDS_Organization")]
public partial class EdsOrganization
{
    [Key]
    [Column("Organization_ID")]
    public int OrganizationId { get; set; }

    [Column("Organization_Code")]
    public string OrganizationCode { get; set; } = null!;

    [Column("ParentOrganization_Code")]
    [StringLength(255)]
    public string? ParentOrganizationCode { get; set; }

    [Column("Created_By")]
    public string CreatedBy { get; set; } = null!;

    [Column("Created_At")]
    public DateTime CreatedAt { get; set; }

    [Column("Updated_By")]
    public string? UpdatedBy { get; set; }

    [Column("Updated_At")]
    public DateTime? UpdatedAt { get; set; }

    [Column("Is_Active")]
    public bool IsActive { get; set; }

    [InverseProperty("Organization")]
    public virtual ICollection<EdsClient> EdsClients { get; set; } = new List<EdsClient>();
}
