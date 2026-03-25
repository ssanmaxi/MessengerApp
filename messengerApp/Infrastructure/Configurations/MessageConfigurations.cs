using messengerApp.Infrastructure.InfrastructureEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messengerApp.Infrastructure.Configurations;

public class MessageConfigurations : IEntityTypeConfiguration<MessagesEntity>
{
    public void Configure(EntityTypeBuilder<MessagesEntity> en)
    {
        en.ToTable("Messages");
        en.HasKey(en=>en.Id);
        en.Property(en => en.Sender);
        en.Property(en => en.Text);
        en.Property(en => en.Roomname);
        en.Property(en => en.Date);
    }
}