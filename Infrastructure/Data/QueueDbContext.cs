using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Queue.Domain.Entities;

namespace Infrastructure.Data;

public class QueueDbContext : DbContext
{
    public QueueDbContext(DbContextOptions<QueueDbContext> options) : base(options) { }
    
    public DbSet<Ticket>? Tickets { get; set; }
    public DbSet<Service>? Services { get; set; }
    public DbSet<Users>? Users { get; set; }
    public DbSet<UserRoles>? UserRoles { get; set; }
    public DbSet<Roles>? Roles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        var ticket = builder.Entity<Ticket>();
        ticket.ToTable("tickets");
        ticket.HasKey(t => t.Id);
        ticket.Property(t => t.Id).HasDefaultValueSql("gen_random_uuid()");
        ticket.Property(t => t.Number).IsRequired();
        ticket.Property(t => t.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        ticket.Property(t => t.Facets).HasColumnType("jsonb");
        
        var service = builder.Entity<Service>();
        service.ToTable("services");
        service.HasKey(s => s.Id);
        service.Property(s => s.Id).HasDefaultValueSql("gen_random_uuid()");
        service.Property(s => s.Name).IsRequired().HasMaxLength(50);
        service.Property(s => s.Description).IsRequired().HasMaxLength(200);
        service.Property(s => s.Letter).HasMaxLength(1);
        service.HasOne(s => s.Parent).WithMany(s => s.Children).HasForeignKey(s => s.ParentId).IsRequired(false);

        service.HasData(
            new {Id = Guid.Parse("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"), Name = "Регистратура", Description = "Запись на первичный прием, заведение медицинских карт и предоставление справочной информации о работе клиники.", IconName="Book", Letter = "A", ParentId = (Guid?)null },
            new {Id = Guid.Parse("99c48a22-122d-4821-afea-2b2b345e592c"), Name = "Платные услуги", Description = "Оформление и оплата медицинских услуг, не входящих в программу ОМС.", IconName="Ruble", Letter = "B", ParentId = (Guid?)null },
            new {Id = Guid.Parse("7370aa38-cbb9-4220-915d-ce042194f24e"), Name = "Анализы", Description = "Лабораторная диагностика от общих анализов крови до генетических исследований.", IconName="Lab", Letter = "C", ParentId = (Guid?)null },
            new {Id = Guid.Parse("9d78a673-efa3-4af3-9828-55515d26e134"), Name = "Запись на прием к врачу", Description = "Выбор специалиста и бронирование подходящего времени визита.", IconName="Clock", ParentId = Guid.Parse("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc") },
            new {Id = Guid.Parse("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"), Name = "Оформление больничного", Description = "Официальное подтверждение временной нетрудоспособности.", IconName="CheckBook", ParentId = Guid.Parse("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc") }
        );

        var users = builder.Entity<Users>();
        users.ToTable("users");
        users.HasKey(u => u.Id);
        users.Property(u => u.Id).HasDefaultValueSql("gen_random_uuid()");
        users.Property(u => u.Name).HasMaxLength(30);
        users.Property(u => u.Surname).HasMaxLength(50);
        users.Property(u => u.MiddleName).HasMaxLength(50);
        users.Property(u => u.Email).IsRequired().HasMaxLength(30);
        users.Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);
        users.Property(u => u.Status).IsRequired().HasMaxLength(10);
        users.HasOne(u => u.Service).WithMany().HasForeignKey(u => u.ServiceId).IsRequired();

        var roles = builder.Entity<Roles>();
        roles.ToTable("roles");
        roles.HasKey(r => r.Id);
        roles.Property(r => r.Id).HasDefaultValueSql("gen_random_uuid()");
        roles.Property(r => r.Title).IsRequired().HasMaxLength(20);

        var userRoles = builder.Entity<UserRoles>();
        userRoles.ToTable("user_roles");
        userRoles.HasKey(ur => new { ur.UserId, ur.RoleId });
        userRoles.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
        userRoles.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();


    } 
}