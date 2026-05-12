namespace ServeyBasket.Persistense.EntitiesConfigurations;

public class VoteAnswerConfigrations : IEntityTypeConfiguration<VoteAnswer>
{
    public void Configure(EntityTypeBuilder<VoteAnswer> builder)
    {
        builder.HasIndex(v => new { v.VoteId, v.QuestionId }).IsUnique();
    }
}
