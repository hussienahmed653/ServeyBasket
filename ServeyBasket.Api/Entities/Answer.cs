namespace ServeyBasket.Entities;

public sealed class Answer
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int QuestionsId { get; set; }
    public Question Questions { get; set; } = default!;
}
