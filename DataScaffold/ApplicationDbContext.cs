//using System;
//using System.Collections.Generic;
//using BlazorApp1.Models;
//using Microsoft.EntityFrameworkCore;

//namespace BlazorApp1.Data;

//public partial class EcoMealDbContext : DbContext
//{
//    public EcoMealDbContext()
//    {
//    }

//    public EcoMealDbContext(DbContextOptions<EcoMealDbContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Business> Businesses { get; set; }

//    public virtual DbSet<BusinessType> BusinessTypes { get; set; }

//    public virtual DbSet<Order> Orders { get; set; }

//    public virtual DbSet<OrderPackage> OrderPackages { get; set; }

//    public virtual DbSet<Package> Packages { get; set; }

//    public virtual DbSet<PackageType> PackageTypes { get; set; }

//    public virtual DbSet<Role> Roles { get; set; }

//    public virtual DbSet<Status> Statuses { get; set; }

//    public virtual DbSet<User> Users { get; set; }


//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Business>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Business__3213E83F062384E2");

//            entity.ToTable("Business");

//            entity.Property(e => e.Id)
//                .ValueGeneratedNever()
//                .HasColumnName("id");
//            entity.Property(e => e.Address)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("address");
//            entity.Property(e => e.BusinessTypeId).HasColumnName("business_type_id");
//            entity.Property(e => e.Description)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("description");
//            entity.Property(e => e.ImageUrl)
//                .HasMaxLength(500)
//                .IsUnicode(false)
//                .HasColumnName("image_url");
//            entity.Property(e => e.Name)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("name");

//            entity.HasOne(d => d.BusinessType).WithMany(p => p.Businesses)
//                .HasForeignKey(d => d.BusinessTypeId)
//                .HasConstraintName("FK__Business__busine__4D94879B");
//        });

//        modelBuilder.Entity<BusinessType>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Business__3213E83FBBD111F7");

//            entity.ToTable("BusinessType");

//            entity.Property(e => e.Id)
//                .ValueGeneratedNever()
//                .HasColumnName("id");
//            entity.Property(e => e.Name)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("name");
//        });

//        modelBuilder.Entity<Order>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Orders__3213E83F32226420");

//            entity.Property(e => e.Id)
//                .ValueGeneratedNever()
//                .HasColumnName("id");
//            entity.Property(e => e.BusinessId).HasColumnName("business_id");
//            entity.Property(e => e.OrderNumber).HasColumnName("order_number");
//            entity.Property(e => e.StatusId).HasColumnName("status_id");
//            entity.Property(e => e.UserId).HasColumnName("user_id");

//            entity.HasOne(d => d.Business).WithMany(p => p.Orders)
//                .HasForeignKey(d => d.BusinessId)
//                .HasConstraintName("FK__Orders__business__5629CD9C");

//            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
//                .HasForeignKey(d => d.StatusId)
//                .HasConstraintName("FK__Orders__status_i__571DF1D5");

//            entity.HasOne(d => d.User).WithMany(p => p.Orders)
//                .HasForeignKey(d => d.UserId)
//                .HasConstraintName("FK__Orders__user_id__5812160E");
//        });

//        modelBuilder.Entity<OrderPackage>(entity =>
//        {
//            entity.HasKey(e => e.OrderId).HasName("PK__OrderPac__46596229686687F4");

//            entity.ToTable("OrderPackage");

//            entity.Property(e => e.OrderId)
//                .ValueGeneratedNever()
//                .HasColumnName("order_id");
//            entity.Property(e => e.PackageId).HasColumnName("package_id");
//            entity.Property(e => e.Quantity).HasColumnName("quantity");

//            entity.HasOne(d => d.Order).WithOne(p => p.OrderPackage)
//                .HasForeignKey<OrderPackage>(d => d.OrderId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__OrderPack__order__5AEE82B9");

//            entity.HasOne(d => d.Package).WithMany(p => p.OrderPackages)
//                .HasForeignKey(d => d.PackageId)
//                .HasConstraintName("FK__OrderPack__packa__5BE2A6F2");
//        });

//        modelBuilder.Entity<Package>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Package__3214EC27DE1169C0");

//            entity.ToTable("Package");

//            entity.Property(e => e.Id)
//                .ValueGeneratedNever()
//                .HasColumnName("ID");
//            entity.Property(e => e.BusinessId).HasColumnName("business_id");
//            entity.Property(e => e.Description)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("description");
//            entity.Property(e => e.ImageUrl)
//                .HasMaxLength(500)
//                .IsUnicode(false)
//                .HasColumnName("image_url");
//            entity.Property(e => e.Name)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("name");
//            entity.Property(e => e.PackageTypeId).HasColumnName("package_type_id");
//            entity.Property(e => e.PickupEnd)
//                .HasColumnType("datetime")
//                .HasColumnName("pickup_end");
//            entity.Property(e => e.PickupStart)
//                .HasColumnType("datetime")
//                .HasColumnName("pickup_start");
//            entity.Property(e => e.Price).HasColumnName("price");
//            entity.Property(e => e.Quantity).HasColumnName("quantity");

//            entity.HasOne(d => d.Business).WithMany(p => p.Packages)
//                .HasForeignKey(d => d.BusinessId)
//                .HasConstraintName("FK__Package__busines__5070F446");

//            entity.HasOne(d => d.PackageType).WithMany(p => p.Packages)
//                .HasForeignKey(d => d.PackageTypeId)
//                .HasConstraintName("FK__Package__package__5165187F");
//        });

//        modelBuilder.Entity<PackageType>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__PackageT__3214EC27722EBDB0");

//            entity.ToTable("PackageType");

//            entity.Property(e => e.Id)
//                .ValueGeneratedNever()
//                .HasColumnName("ID");
//            entity.Property(e => e.Name)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("name");
//        });

//        modelBuilder.Entity<Role>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC272D4DF184");

//            entity.ToTable("Role");

//            entity.Property(e => e.Id)
//                .ValueGeneratedNever()
//                .HasColumnName("ID");
//            entity.Property(e => e.Name)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//        });

//        modelBuilder.Entity<Status>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Status__3213E83FD1EB6A86");

//            entity.ToTable("Status");

//            entity.Property(e => e.Id)
//                .ValueGeneratedNever()
//                .HasColumnName("id");
//            entity.Property(e => e.Name)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("name");
//        });

//        modelBuilder.Entity<User>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC279A200B42");

//            entity.Property(e => e.Id)
//                .ValueGeneratedNever()
//                .HasColumnName("ID");
//            entity.Property(e => e.Email)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.Name)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.Password)
//                .HasMaxLength(500)
//                .IsUnicode(false);
//            entity.Property(e => e.RoleId).HasColumnName("role_id");

//            entity.HasOne(d => d.Role).WithMany(p => p.Users)
//                .HasForeignKey(d => d.RoleId)
//                .HasConstraintName("FK__Users__role_id__3B75D760");
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
