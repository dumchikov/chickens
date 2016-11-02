using System.Collections.Generic;
using System.Web.Mvc;
using Chicken.Domain.Models;

namespace Chicken.Web.Models
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
    }

    public class MainViewModel
    {
        public string CurrentGroup { get; set; }

        public string CurrentGroupName { get; set; }

        public int CurrentPage { get; set; }

        public int NumberOfPages { get; set; }

        public IEnumerable<PostsViewModel> Posts { get; set; }

        public SelectList GroupsSelectList { get; set; }

        public string Keyword { get; set; }
    }
}