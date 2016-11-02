using System.Collections.Generic;

namespace Chicken.Domain.Models
{
    public class Account : Entity
    {
        public int ProfileId { get; set; }

        public string Avatar { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public int Coins { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
