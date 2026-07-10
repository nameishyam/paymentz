namespace Server.Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid UserId { get; set; }
    public string App { get; set; }
    public double Amount { get; set; }
    public TimeOnly Time { get; set; }
    public string ToAccount { get; set; }
}