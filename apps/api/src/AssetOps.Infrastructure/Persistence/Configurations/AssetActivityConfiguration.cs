using AssetOps.Domain.Assets;

namespace AssetOps.Infrastructure.Persistence.Configurations;

public sealed class AssetActivityConfiguration : IEntityTypeConfiguration<AssetActivity>
{
    public void Configure(EntityTypeBuilder<AssetActivity> builder)
    {
        builder.ToTable("AssetActivities");

        builder.HasKey(ac => ac.Id);

        builder.Property(ac => ac.AssetId)
            .IsRequired();

        builder.Property(ac => ac.Type)
            .HasConversion<string>()
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(ac => ac.Holder)
            .HasMaxLength(200);

        builder.Property(ac => ac.OccurredAt)
            .IsRequired();
    }
}
