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
    public class ShiftRepository : Repository<Shift>, IRShiftRepository
    {
        private readonly ApplicationDbContext _db;

        public ShiftRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
