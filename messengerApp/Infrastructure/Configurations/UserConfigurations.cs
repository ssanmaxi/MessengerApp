using messengerApp.Infrastructure.InfrastructureEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messengerApp.Infrastructure.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
            builder.ToTable("Users");
            builder.HasKey(en => en.Id);
            builder.Property(en => en.Name);
            builder.Property(en => en.Email);
            builder.Property(en => en.PasswordHash);
            builder.Property(en => en.TwoFactorEnabled);
            builder.Property(en => en.TwoFactorSecret);
            builder.Property(en => en.Provider);
            builder.Property(en => en.ProviderUserId);
    }
}