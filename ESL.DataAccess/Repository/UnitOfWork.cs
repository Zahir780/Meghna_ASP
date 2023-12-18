////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: UnitOfWork.cs
//FileType: Visual C# Source file
//Author : Zahid Sarmon
//Created On : 02-11-2020
//Copy Rights : Evision Soft. Ltd.
//Description : Class for Call All Transaction Form
////////////////////////////////////////////////////////////////////////////////////////////////////////

using ESL.DataAccess.Data;
using ESL.DataAccess.Repository;
using ESL.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            SP_Call = new SP_Call(_db);
            Company = new CompanyRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            Notification = new NotificationRepository(_db);
            UserAuthentication = new UserAuthenticationRepository(_db);
            ChatMessage = new ChatMessageRepository(_db);
            Bank = new BankRepository(_db);
            BankBranch = new BankBranchRepository(_db);
        }
        public ISP_Call SP_Call { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICompanyRepository Company { get; private set; }

        public INotificationRepository Notification { get; private set; }

        public IUserAuthenticationRepository UserAuthentication { get; private set; }

        public IChatMessageRepository ChatMessage { get; private set; }

        public IBankRepository Bank { get; private set; }

        public IBankBranchRepository BankBranch { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
