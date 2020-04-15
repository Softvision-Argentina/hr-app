using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class EmployeeRepository : Repository<Employee, DataBaseContext>
    {
        public EmployeeRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {

        }

        public override IQueryable<Employee> Query()
        {
            return base.Query().Include(employee => employee.User).Include(employee => employee.Role).Include(employee => employee.Reviewer); 
        }

        public override IQueryable<Employee> QueryEager()
        {
            return Query().Include(employee => employee.User).Include(employee => employee.Role).Include(employee => employee.Reviewer);
        }

        public override Employee Update(Employee emp)
        {
            if (_dbContext.Entry(emp).State == EntityState.Detached)
            {
                _dbContext.Set<Employee>().Attach(emp);

                _dbContext.Entry(emp).State = EntityState.Modified;
            }

            var employee = Query().Include(x => x.Reviewer).Where(c => c.Id == emp.Id).FirstOrDefault();

            _dbContext.Entry(employee).CurrentValues.SetValues(emp);

            return employee;
        }

    }
}
