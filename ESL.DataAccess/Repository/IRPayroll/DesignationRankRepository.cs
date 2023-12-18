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
    public class DesignationRankRepository : Repository<DesignationRank>, IRDesignationRankRepository
    {
        private readonly ApplicationDbContext _db;

        public DesignationRankRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
