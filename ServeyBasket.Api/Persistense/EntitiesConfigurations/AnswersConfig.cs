using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServeyBasket.Persistense.EntitiesConfigurations;

public class AnswersConfig : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasIndex(x => new { x.QuestionsId, x.Contant }).IsUnique();
        builder.Property(x => x.Contant)
            .HasMaxLength(100);
    }
}
