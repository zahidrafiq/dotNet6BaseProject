using System;
using System.Collections.Generic;
using Core.Repository.DataContext;
using EDS.DB.Model.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace EDS.DB.Model.Data;

public partial class SerializationContext : DataContext
{
    public SerializationContext()
    {
    }

    public SerializationContext(DbContextOptions<SerializationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EdsClient> EdsClients { get; set; }

    public virtual DbSet<EdsOrganization> EdsOrganizations { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EdsClient>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK_Client");

            entity.HasOne(d => d.Organization).WithMany(p => p.EdsClients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_Org_OrgsOrganizationID");
        });

        modelBuilder.Entity<EdsOrganization>(entity =>
        {
            entity.HasKey(e => e.OrganizationId).HasName("PK_Org");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
