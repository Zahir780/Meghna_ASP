using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.DataAccess.Repository.IRepository
{
    public interface UnitPayrollInterface : IDisposable
    {
        ISP_Call SP_Call { get; }
        IRDepartmentRepository Department { get; }
        IRSectionRepository Section { get; }
        IRDesignationRankRepository DesignationRank { get; }
        IRDesignationRepository Designation { get; }
        IRShiftRepository Shift { get; }
        IREmployeeRepository Employee { get; }

        void Save();
    }
}
