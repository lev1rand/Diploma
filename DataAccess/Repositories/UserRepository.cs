﻿using DataAccess.Entities;
using DataAccess.Interfaces.Repositories;

namespace DataAccess.Repositories
{
    public class UserRepository : CommonRepository<User>, IUserRepository
    {
        private readonly AppContext context;

        public UserRepository(AppContext context) : base(context)
        {
            this.context = context;
        }
    }
}
