using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServeyBasket.Persistense.EntitiesConfigurations;

public class AnswersConfig : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasIndex(x => new { x.QuestionsId, x.Content }).IsUnique();
        builder.Property(x => x.Content)
            .HasMaxLength(100);
    }
}
