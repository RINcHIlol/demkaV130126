using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace demkaV130126.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agent> Agents { get; set; }

    public virtual DbSet<AgentPriorityHistory> AgentPriorityHistories { get; set; }

    public virtual DbSet<AgentType> AgentTypes { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialCountHistory> MaterialCountHistories { get; set; }

    public virtual DbSet<MaterialSupplier> MaterialSuppliers { get; set; }

    public virtual DbSet<Materialtype> Materialtypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCostHistory> ProductCostHistories { get; set; }

    public virtual DbSet<ProductMaterial> ProductMaterials { get; set; }

    public virtual DbSet<ProductSale> ProductSales { get; set; }

    public virtual DbSet<Producttype> Producttypes { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=5432");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("agent_pkey");

            entity.ToTable("agent");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(300)
                .HasColumnName("address");
            entity.Property(e => e.AgentTypeId).HasColumnName("agent_type_id");
            entity.Property(e => e.DirectorName)
                .HasMaxLength(100)
                .HasColumnName("director_name");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Inn)
                .HasMaxLength(12)
                .HasColumnName("inn");
            entity.Property(e => e.Kpp)
                .HasMaxLength(9)
                .HasColumnName("kpp");
            entity.Property(e => e.Logo)
                .HasMaxLength(100)
                .HasColumnName("logo");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");

            entity.HasOne(d => d.AgentType).WithMany(p => p.Agents)
                .HasForeignKey(d => d.AgentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("agent_agent_type_id_fkey");
        });

        modelBuilder.Entity<AgentPriorityHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("agent_priority_history_pkey");

            entity.ToTable("agent_priority_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgentId).HasColumnName("agent_id");
            entity.Property(e => e.ChangeDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("change_date");
            entity.Property(e => e.PriorityValue).HasColumnName("priority_value");

            entity.HasOne(d => d.Agent).WithMany(p => p.AgentPriorityHistories)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("agent_priority_history_agent_id_fkey");
        });

        modelBuilder.Entity<AgentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("agenttype_pkey");

            entity.ToTable("agent_type");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('agenttype_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .HasColumnName("image");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("material_pkey");

            entity.ToTable("material");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost)
                .HasPrecision(10, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Countinpack).HasColumnName("countinpack");
            entity.Property(e => e.Countinstock)
                .HasPrecision(10, 2)
                .HasColumnName("countinstock");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .HasColumnName("image");
            entity.Property(e => e.Materialtypeid).HasColumnName("materialtypeid");
            entity.Property(e => e.Mincount)
                .HasPrecision(10, 2)
                .HasColumnName("mincount");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .HasColumnName("unit");

            entity.HasOne(d => d.Materialtype).WithMany(p => p.Materials)
                .HasForeignKey(d => d.Materialtypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("material_materialtypeid_fkey");
        });

        modelBuilder.Entity<MaterialCountHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("material_count_history_pkey");

            entity.ToTable("material_count_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChangeDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("change_date");
            entity.Property(e => e.CountValue)
                .HasPrecision(10, 2)
                .HasColumnName("count_value");
            entity.Property(e => e.MaterialId).HasColumnName("material_id");

            entity.HasOne(d => d.Material).WithMany(p => p.MaterialCountHistories)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("material_count_history_material_id_fkey");
        });

        modelBuilder.Entity<MaterialSupplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("materialsupplier_pkey");

            entity.ToTable("material_supplier");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('materialsupplier_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");

            entity.HasOne(d => d.Material).WithMany(p => p.MaterialSuppliers)
                .HasForeignKey(d => d.Materialid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("materialsupplier_materialid_fkey");

            entity.HasOne(d => d.Supplier).WithMany(p => p.MaterialSuppliers)
                .HasForeignKey(d => d.Supplierid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("materialsupplier_supplierid_fkey");
        });

        modelBuilder.Entity<Materialtype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("materialtype_pkey");

            entity.ToTable("materialtype");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Defectedpercent)
                .HasPrecision(10, 2)
                .HasColumnName("defectedpercent");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Articlenumber)
                .HasMaxLength(10)
                .HasColumnName("articlenumber");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .HasColumnName("image");
            entity.Property(e => e.Mincostforagent)
                .HasPrecision(10, 2)
                .HasColumnName("mincostforagent");
            entity.Property(e => e.Productionpersoncount).HasColumnName("productionpersoncount");
            entity.Property(e => e.Productionworkshopnumber).HasColumnName("productionworkshopnumber");
            entity.Property(e => e.Producttypeid).HasColumnName("producttypeid");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Producttype).WithMany(p => p.Products)
                .HasForeignKey(d => d.Producttypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_producttypeid_fkey");
        });

        modelBuilder.Entity<ProductCostHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_cost_history_pkey");

            entity.ToTable("product_cost_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChangeDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("change_date");
            entity.Property(e => e.CostValue)
                .HasPrecision(10, 2)
                .HasColumnName("cost_value");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductCostHistories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_cost_history_product_id_fkey");
        });

        modelBuilder.Entity<ProductMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_material_pkey");

            entity.ToTable("product_material");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count)
                .HasPrecision(10, 2)
                .HasColumnName("count");
            entity.Property(e => e.MaterialId).HasColumnName("material_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Material).WithMany(p => p.ProductMaterials)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_material_material_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductMaterials)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_material_product_id_fkey");
        });

        modelBuilder.Entity<ProductSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_sale_pkey");

            entity.ToTable("product_sale");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgentId).HasColumnName("agent_id");
            entity.Property(e => e.ProductCount).HasColumnName("product_count");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.SaleDate).HasColumnName("sale_date");

            entity.HasOne(d => d.Agent).WithMany(p => p.ProductSales)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_sale_agent_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSales)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_sale_product_id_fkey");
        });

        modelBuilder.Entity<Producttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("producttype_pkey");

            entity.ToTable("producttype");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Defectedpercent)
                .HasPrecision(10, 2)
                .HasColumnName("defectedpercent");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shop_pkey");

            entity.ToTable("shop");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(300)
                .HasColumnName("address");
            entity.Property(e => e.AgentId).HasColumnName("agent_id");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");

            entity.HasOne(d => d.Agent).WithMany(p => p.Shops)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shop_agent_id_fkey");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supplier_pkey");

            entity.ToTable("supplier");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Inn)
                .HasMaxLength(12)
                .HasColumnName("inn");
            entity.Property(e => e.Qualityrating).HasColumnName("qualityrating");
            entity.Property(e => e.Startdate).HasColumnName("startdate");
            entity.Property(e => e.Suppliertype)
                .HasMaxLength(20)
                .HasColumnName("suppliertype");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
