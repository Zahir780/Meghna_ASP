using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
         ISP_Call SP_Call { get; }
         ICompanyRepository Company { get; }
         IApplicationUserRepository ApplicationUser { get; }
         INotificationRepository Notification { get; }
         IUserAuthenticationRepository UserAuthentication { get; }
         IChatMessageRepository ChatMessage { get; }
         IBankRepository Bank { get; }
         IBankBranchRepository BankBranch { get; }
        void Save();
    }
}
