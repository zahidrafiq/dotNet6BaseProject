using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EDS.DB.Model.EF.Models;

[Table("EDS_Client")]
public partial class EdsClient
{
    [Key]
    [Column("Client_ID")]
    public int ClientId { get; set; }

    [Column("Client_Name")]
    [StringLength(255)]
    public string ClientName { get; set; } = null!;

    [Column("Client_Code")]
    [StringLength(50)]
    public string ClientCode { get; set; } = null!;

    [Column("Organization_ID")]
    public int OrganizationId { get; set; }

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

    [ForeignKey("OrganizationId")]
    [InverseProperty("EdsClients")]
    public virtual EdsOrganization Organization { get; set; } = null!;
}
