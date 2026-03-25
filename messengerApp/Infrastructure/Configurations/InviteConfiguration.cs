using messengerApp.Infrastructure.InfrastructureEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messengerApp.Infrastructure.Configurations;

public class InviteConfiguration : IEntityTypeConfiguration<InviteEntity>
{
    public void Configure(EntityTypeBuilder<InviteEntity> builder)
    {
        builder.ToTable("Invites");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Token).IsRequired();
        builder.HasIndex(b => b.Token).IsUnique();
        builder.Property(b => b.LobbyId).IsRequired();
        builder.Property(b => b.ExpiresAt).IsRequired();
        builder.Property(b => b.UsesCount).HasDefaultValue(0);
        builder.Property(b => b.MaxUses).HasDefaultValue(1);
        builder.Property(b => b.Revoked).HasDefaultValue(false);
    }
}