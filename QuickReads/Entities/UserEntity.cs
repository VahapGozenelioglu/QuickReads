namespace QuickReads.Entities;

public class UserEntity : BaseEntity
{
    public List<UserTagAssoc> UserTagAssocs { get; set; }
}