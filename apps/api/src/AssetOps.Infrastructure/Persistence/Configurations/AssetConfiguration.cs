using AssetOps.Domain.Assets;

namespace AssetOps.Infrastructure.Persistence.Configurations;

public sealed class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Tag)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(a => a.Tag)
            .IsUnique();

        builder.Property(a => a.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(a => a.CurrentHolder)
            .HasMaxLength(200);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.RetiredAt);

        builder.HasMany(a => a.Activities)
            .WithOne()
            .HasForeignKey(ac => ac.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(a => a.Activities)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
