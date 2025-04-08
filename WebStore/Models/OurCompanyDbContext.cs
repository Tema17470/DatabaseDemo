using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Models;

public partial class OurCompanyDbContext : DbContext
{
    public OurCompanyDbContext()
    {
    }

    public OurCompanyDbContext(DbContextOptions<OurCompanyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderedProduct> OrderedProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductLine> ProductLines { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=OurCompanyDB;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrdDate).HasColumnName("ord_date");
            entity.Property(e => e.OrdStatus)
                .HasMaxLength(50)
                .HasColumnName("ord_status");
            entity.Property(e => e.OrdTotal).HasColumnName("ord_total");
            entity.Property(e => e.UId).HasColumnName("u_id");

            entity.HasOne(d => d.UIdNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("orders_u_id_fkey");
        });

        modelBuilder.Entity<OrderedProduct>(entity =>
        {
            entity.HasKey(e => e.ListId).HasName("ordered_products_pkey");

            entity.ToTable("ordered_products");

            entity.Property(e => e.ListId).HasColumnName("list_id");
            entity.Property(e => e.OrdId).HasColumnName("ord_id");
            entity.Property(e => e.ProdId).HasColumnName("prod_id");

            entity.HasOne(d => d.Ord).WithMany(p => p.OrderedProducts)
                .HasForeignKey(d => d.OrdId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ordered_products_ord_id_fkey");

            entity.HasOne(d => d.Prod).WithMany(p => p.OrderedProducts)
                .HasForeignKey(d => d.ProdId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ordered_products_prod_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProdId).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.ProdId).HasColumnName("prod_id");
            entity.Property(e => e.LineId).HasColumnName("line_id");
            entity.Property(e => e.ProdCategory)
                .HasMaxLength(50)
                .HasColumnName("prod_category");
            entity.Property(e => e.ProdDesc)
                .HasMaxLength(200)
                .HasColumnName("prod_desc");
            entity.Property(e => e.ProdPrice).HasColumnName("prod_price");

            entity.HasOne(d => d.Line).WithMany(p => p.Products)
                .HasForeignKey(d => d.LineId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("products_line_id_fkey");
        });

        modelBuilder.Entity<ProductLine>(entity =>
        {
            entity.HasKey(e => e.LineId).HasName("product_line_pkey");

            entity.ToTable("product_line");

            entity.HasIndex(e => e.LineName, "product_line_line_name_key").IsUnique();

            entity.Property(e => e.LineId).HasColumnName("line_id");
            entity.Property(e => e.LineName)
                .HasMaxLength(50)
                .HasColumnName("line_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.UEmail, "users_u_email_key").IsUnique();

            entity.Property(e => e.UId).HasColumnName("u_id");
            entity.Property(e => e.UAddress)
                .HasMaxLength(100)
                .HasColumnName("u_address");
            entity.Property(e => e.UEmail)
                .HasMaxLength(50)
                .HasColumnName("u_email");
            entity.Property(e => e.UFirstname)
                .HasMaxLength(50)
                .HasColumnName("u_firstname");
            entity.Property(e => e.ULastname)
                .HasMaxLength(50)
                .HasColumnName("u_lastname");
            entity.Property(e => e.UPostalcode).HasColumnName("u_postalcode");
            entity.Property(e => e.URegion)
                .HasMaxLength(50)
                .HasColumnName("u_region");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
