namespace ServeyBasket.Persistense.EntitiesConfigurations;

public class VoteConfigrations : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasIndex(v => new { v.PollId, v.UserId }).IsUnique();
    }
}
