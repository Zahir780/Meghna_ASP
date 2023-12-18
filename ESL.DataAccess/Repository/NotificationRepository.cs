
using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESL.DataAccess.Repository
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly ApplicationDbContext _db;

        public NotificationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

       
    }
}
