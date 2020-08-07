// <copyright file="EmployeeRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

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
            return this.Query().Include(employee => employee.User).Include(employee => employee.Role).Include(employee => employee.Reviewer);
        }

        public override Employee Update(Employee entity)
        {
            if (this.DbContext.Entry(entity).State == EntityState.Detached)
            {
                this.DbContext.Set<Employee>().Attach(entity);

                this.DbContext.Entry(entity).State = EntityState.Modified;
            }

            var employee = this.Query().Include(x => x.Reviewer).Where(c => c.Id == entity.Id).FirstOrDefault();

            this.DbContext.Entry(employee).CurrentValues.SetValues(entity);

            return employee;
        }
    }
}
