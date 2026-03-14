namespace Queue.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<User>? Users { get; set; }
    public DbSet<Client>? Clients { get; set; }
    public DbSet<Ticket>? Tickets { get; set; }
    public DbSet<Service>? Services { get; set; }
    public DbSet<Status>? Statuses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        foreach (var model in builder.Model.GetEntityTypes())
        {
            var tableName = model.GetTableName()?.ToLower();
            model.SetTableName(tableName);

            foreach (var property in model.GetProperties())
            {
                property.SetColumnName(property.Name.ToLower());
            }
        }
    } 
}