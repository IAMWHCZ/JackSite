namespace JackSite.Identity.Server.Entities.Bases
{
    public class BaseProprietyEntity:BaseEntity
    {
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}