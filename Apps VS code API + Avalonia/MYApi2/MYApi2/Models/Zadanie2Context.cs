using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MYApi2.Models;

public partial class Zadanie2Context : DbContext
{
    public Zadanie2Context()
    {
    }

    public Zadanie2Context(DbContextOptions<Zadanie2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public DbSet<Position> Positions => Set<Position>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Zadanie2;Username=postgres;Password=Asdfgh415");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("employee_id")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.PositionCode).HasColumnName("position_code");
            entity.Property(e => e.Salary)
                .HasPrecision(10, 2)
                .HasColumnName("salary");

            entity.HasOne(d => d.PositionCodeNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionCode)
                .HasConstraintName("employees_position_code_fkey");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionCode).HasName("positions_pkey");

            entity.ToTable("positions");

            entity.Property(e => e.PositionCode)
                .ValueGeneratedNever()
                .HasColumnName("position_code");
            entity.Property(e => e.PositionName)
                .HasMaxLength(100)
                .HasColumnName("position_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
