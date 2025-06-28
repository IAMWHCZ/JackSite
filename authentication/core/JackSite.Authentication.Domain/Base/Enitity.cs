namespace JackSite.Authentication.Base;

public abstract class Entity : BaseEntity<Guid>
{
    protected Entity() : base(Guid.CreateVersion7())
    {
        Id = Guid.CreateVersion7();
    }
}