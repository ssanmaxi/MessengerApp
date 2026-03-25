using messengerApp.Infrastructure.InfrastructureEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messengerApp.Infrastructure.Configurations;

public class LobbyMemberConfiguration : IEntityTypeConfiguration<LobbyMemberEntity>
{
    public void Configure(EntityTypeBuilder<LobbyMemberEntity> builder)
    {
        builder.ToTable("LobbyMembers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.LobbyId).IsRequired();
        builder.Property(x => x.UserId).IsRequired();

        builder.HasIndex(x => new { x.LobbyId, x.UserId }).IsUnique();
    }
}