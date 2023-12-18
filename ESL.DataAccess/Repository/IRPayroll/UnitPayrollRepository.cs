using ESL.DataAccess.Data;
using ESL.DataAccess.Repository;
using ESL.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.DataAccess.Repository
{
    public class UnitPayrollRepository : UnitPayrollInterface
    {
        private readonly ApplicationDbContext _db;
        public UnitPayrollRepository(ApplicationDbContext db)
        {
            _db = db;
            SP_Call = new SP_Call(_db);
            Department = new DepartmentRepository(_db);
            Section = new SectionRepository(_db);
            DesignationRank = new DesignationRankRepository(_db);
            Designation = new DesignationRepository(_db);
            Shift = new ShiftRepository(_db);
            Employee = new EmployeeRepository(_db);
        }
        public ISP_Call SP_Call { get; private set; }
        public IRDepartmentRepository Department { get; private set; }
        public IRSectionRepository Section { get; private set; }
        public IRDesignationRankRepository DesignationRank { get; private set; }
        public IRDesignationRepository Designation { get; private set; }
        public IRShiftRepository Shift { get; private set; }
        public IREmployeeRepository Employee { get; private set; }

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
