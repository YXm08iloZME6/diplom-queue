using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class QueueDbContext : DbContext
{
    public QueueDbContext(DbContextOptions<QueueDbContext> options) : base(options) { }
    
    public DbSet<Ticket>? Tickets { get; set; }
    public DbSet<Service>? Services { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Host=localhost;Port=5432;Database=queuedb;Username=user;Password=pass");
    }
    
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
        service.Property(s => s.Letter).HasMaxLength(1);
        service.HasMany(s => s.Children).WithMany(s => s.Parents).UsingEntity(e => e.ToTable("parent_services").HasData(
            new {ChildrenId = Guid.Parse("9d78a673-efa3-4af3-9828-55515d26e134"), ParentsId = Guid.Parse("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc")},
            new {ChildrenId = Guid.Parse("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"), ParentsId = Guid.Parse("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc")}
            ));

        service.HasData(
            new {Id = Guid.Parse("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"), Name = "Регистратура", Letter = "A" },
            new {Id = Guid.Parse("99c48a22-122d-4821-afea-2b2b345e592c"), Name = "Платные услуги", Letter = "B" },
            new {Id = Guid.Parse("7370aa38-cbb9-4260-915d-ce042194f24e"), Name = "Анализы", Letter = "C" },
            new {Id = Guid.Parse("9d78a673-efa3-4af3-9828-55515d26e134"), Name = "Запись на прием к врачу" },
            new {Id = Guid.Parse("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"), Name = "Оформление больничного"  }
        );
    } 
}