using ESL.DataAccess.Data;
using ESL.DataAccess.Repository;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESL.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
