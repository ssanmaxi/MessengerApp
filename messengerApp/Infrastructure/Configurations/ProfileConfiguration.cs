using messengerApp.Infrastructure.InfrastructureEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messengerApp.Infrastructure.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.ToTable("Profiles");
       
        builder.HasKey(en => en.Id);
       
        builder.Property(en => en.Name).IsRequired();
        builder.Property(en => en.Age).IsRequired();
        
        builder.HasIndex(en => en.UserId).IsUnique(); //ensures that a certain user has only 1 profile

        builder.HasOne(en => en.User)
            .WithOne(p => p.Profile) //1-1 relation
            .HasForeignKey<ProfileEntity>(p => p.UserId) 
            .OnDelete(DeleteBehavior.Cascade); //delete user = delete profile
    }
}