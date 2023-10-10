namespace CleanSample.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entitiesAssembly = typeof(ApplicationDbContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(entitiesAssembly);

        RegisterAllEntities<IEntity>(modelBuilder, entitiesAssembly);
    }

    public static void RegisterAllEntities<TBaseType>(ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        var types = assemblies
            .SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(TBaseType).IsAssignableFrom(c));

        Pluralizer pluralize = new();

        foreach (var type in types)
            modelBuilder.Entity(type, b => b.ToTable(pluralize.Pluralize(type.Name)));
    }
}