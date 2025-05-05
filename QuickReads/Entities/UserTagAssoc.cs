namespace QuickReads.Entities;

public class UserTagAssoc : BaseEntity
{
    public int LikeCount { get; set; }
    public int UserId { get; set; }
    public int TagId { get; set; }
    public UserEntity User { get; set; }
    public Tag Tag { get; set; }
}
