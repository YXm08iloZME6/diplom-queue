using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class QueueDbContext : DbContext
{
    public QueueDbContext(DbContextOptions<QueueDbContext> options) : base(options) { }
    
    public DbSet<Ticket>? Tickets { get; set; }
    public DbSet<Service>? Services { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<UserRole>? UserRoles { get; set; }
    public DbSet<Role>? Roles { get; set; }
    public DbSet<Window>? Windows { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var ticket = builder.Entity<Ticket>();
        ticket.ToTable("tickets");
        ticket.HasKey(t => t.Id);
        ticket.Property(t => t.Id).HasDefaultValueSql("gen_random_uuid()");
        ticket.Property(t => t.Number).IsRequired();
        ticket.Property(t => t.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        ticket.Property(t => t.Facets).HasColumnType("jsonb");
        ticket.Property(t => t.CreatedAt).HasColumnType("timestamp without time zone");
        ticket.Property(t => t.CalledAt).HasColumnType("timestamp without time zone");
        ticket.Property(t => t.StartedAt).HasColumnType("timestamp without time zone");
        ticket.Property(t => t.CompletedAt).HasColumnType("timestamp without time zone");
        ticket.HasOne<Window>().WithMany().HasForeignKey(t => t.WindowId).IsRequired(false);

        var service = builder.Entity<Service>();
        service.ToTable("services");
        service.HasKey(s => s.Id);
        service.Property(s => s.Id).HasDefaultValueSql("gen_random_uuid()");
        service.Property(s => s.Name).IsRequired().HasMaxLength(50);
        service.Property(s => s.Description).IsRequired().HasMaxLength(200);
        service.Property(s => s.Letter).HasMaxLength(1);

        service.Property(s => s.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        service.Property(s => s.IsNeedFacets)
            .IsRequired()
            .HasDefaultValue(true);

        service.Property(s => s.NeedMoreInfo)
            .IsRequired()
            .HasDefaultValue(false);

        service.HasOne(s => s.Parent)
            .WithMany(s => s.Children)
            .HasForeignKey(s => s.ParentId)
            .IsRequired(false);

        service.HasData(
            new
            {
                Id = Guid.Parse("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"),
                Name = "Регистратура",
                Description = "Запись на первичный прием, заведение медицинских карт и предоставление справочной информации о работе клиники.",
                IconName = "Book",
                Letter = "A",
                IsActive = true,
                IsNeedFacets = true,
                NeedMoreInfo = false,
                ParentId = (Guid?)null
            },
            new
            {
                Id = Guid.Parse("99c48a22-122d-4821-afea-2b2b345e592c"),
                Name = "Платные услуги",
                Description = "Оформление и оплата медицинских услуг, не входящих в программу ОМС.",
                IconName = "Ruble",
                Letter = "B",
                IsActive = true,
                IsNeedFacets = true,
                NeedMoreInfo = false,
                ParentId = (Guid?)null
            },
            new
            {
                Id = Guid.Parse("7370aa38-cbb9-4220-915d-ce042194f24e"),
                Name = "Анализы",
                Description = "Лабораторная диагностика от общих анализов крови до генетических исследований.",
                IconName = "Lab",
                Letter = "C",
                IsActive = true,
                IsNeedFacets = true,
                NeedMoreInfo = false,
                ParentId = (Guid?)null
            },
            new
            {
                Id = Guid.Parse("ef30bd6a-f192-4b25-8885-f7d679c6b313"),
                Name = "Просто спросить",
                Description = "Мне просто спросить",
                IconName = "Lab",
                Letter = "D",
                IsActive = true,
                IsNeedFacets = true,
                NeedMoreInfo = true,
                ParentId = (Guid?)null
            },
            new
            {
                Id = Guid.Parse("9d78a673-efa3-4af3-9828-55515d26e134"),
                Name = "Запись на прием к врачу",
                Description = "Выбор специалиста и бронирование подходящего времени визита.",
                IconName = "Clock",
                IsActive = true,
                IsNeedFacets = true,
                NeedMoreInfo = false,
                ParentId = Guid.Parse("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc")
            },
            new
            {
                Id = Guid.Parse("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"),
                Name = "Оформление больничного",
                Description = "Официальное подтверждение временной нетрудоспособности.",
                IconName = "CheckBook",
                IsActive = true,
                IsNeedFacets = true,
                NeedMoreInfo = false,
                ParentId = Guid.Parse("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc")
            }
        );

        var users = builder.Entity<User>();
        users.ToTable("users");
        users.HasKey(u => u.Id);
        users.Property(u => u.Id).HasDefaultValueSql("gen_random_uuid()");
        users.Property(u => u.Name).HasMaxLength(30);
        users.Property(u => u.Surname).HasMaxLength(50);
        users.Property(u => u.MiddleName).HasMaxLength(50);
        users.Property(u => u.Email).IsRequired().HasMaxLength(30);
        users.Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);
        users.Property(u => u.Status).IsRequired().HasMaxLength(10);
        users.HasOne(u => u.Window).WithMany().HasForeignKey(u => u.WindowId);

        var roles = builder.Entity<Role>();
        roles.ToTable("roles");
        roles.HasKey(r => r.Id);
        roles.Property(r => r.Id).HasDefaultValueSql("gen_random_uuid()");
        roles.Property(r => r.Title).IsRequired().HasMaxLength(20);

        roles.HasData(
            new { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Title = "operator" },
            new { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Title = "admin" }
            );

        var userRoles = builder.Entity<UserRole>();
        userRoles.ToTable("user_roles");
        userRoles.HasKey(ur => new { ur.UserId, ur.RoleId });
        userRoles.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
        userRoles.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();

        
        var window = builder.Entity<Window>();
        window.ToTable("windows");
        window.HasKey(w => w.Id);
        window.Property(w => w.Id).HasDefaultValueSql("gen_random_uuid()");
        window.Property(w => w.Title).IsRequired().HasMaxLength(50);
        window.Property(w => w.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        window.HasOne(w => w.Service).WithMany().HasForeignKey(w => w.ServiceId).IsRequired();
        window.HasMany(w => w.Operators).WithOne(u => u.Window).HasForeignKey(u => u.WindowId);

        var testWindowId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var serviceId = Guid.Parse("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc");

        window.HasData(new
        {
            Id = testWindowId,
            Title = "Регистратура",
            Status = WindowStatus.Open,
            ServiceId = serviceId
        });

    } 
}