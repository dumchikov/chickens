namespace Chicken.Domain.Models
{
    public class Group : Entity
    {
        public string Name { get; set; }

        public string AccessToken { get; set; }

        public string GroupDomainName { get; set; }

        public int OwnerId { get; set; }

        public string Avatar { get; set; }

        public bool IsActive { get; set; }
    }
}
