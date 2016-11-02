using System.Collections.Generic;
using System.Linq;
using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;

namespace Chicken.Services
{
    public class AccountService
    {
        private readonly IRepository<Account> _accounts;

        public AccountService(IRepository<Account> accounts)
        {
            _accounts = accounts;
        }

        public CurrentAccount Get(int id)
        {
            return new CurrentAccount
                {
                    Account = _accounts.Query().SingleOrDefault(x => x.Id == id)
                };
        }
    }
}
