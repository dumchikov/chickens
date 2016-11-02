using System.Collections.Generic;

namespace Chicken.Web.Models
{
    public class SuggestModel
    {
        public string Description { get; set; }

        public string GroupDomainName { get; set; }

        public ICollection<string> PhotosUrls { get; set; }
    }
}