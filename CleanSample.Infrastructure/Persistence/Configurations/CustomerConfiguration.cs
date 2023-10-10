namespace CleanSample.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(e => e.Id);

        builder.OwnsOne(e => e.FirstName, opt => opt.Property(v => v.Value).HasMaxLength(50));
        builder.OwnsOne(e => e.LastName, opt => opt.Property(v => v.Value).HasMaxLength(50));
        builder.OwnsOne(e => e.DateOfBirth);
        builder.OwnsOne(e => e.Phone, opt => opt.Property(v => v.Value).HasMaxLength(20));
        builder.OwnsOne(e => e.BankAccount, opt => opt.Property(v => v.Number).HasMaxLength(30));
        builder.OwnsOne(e => e.Email, opt =>
        {
            opt.Property(v => v.Value).HasMaxLength(100);
            opt.HasIndex(v => v.Value).IsUnique();
        });
    }
}