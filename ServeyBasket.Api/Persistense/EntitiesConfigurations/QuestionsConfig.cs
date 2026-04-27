using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServeyBasket.Persistense.EntitiesConfigurations;

public class QuestionsConfig : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasIndex(x => new { x.PollId, x.Contant }).IsUnique();
        builder.Property(x => x.Contant)
            .HasMaxLength(1000);
    }
}
