using DatabaseFirst.Models;
using DatabaseFirst.Utils.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFirst.Service
{
    public class EmployeeService
    {

        EmployeeRecordsContext context;

        public EmployeeService()
        {
            context = new EmployeeRecordsContext();
        }

        public async Task<Response<List<Employee>>> List()
        {
            try
            {
                var result = await context.Employees
                                .ToListAsync();

                return Response<List<Employee>>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<List<Employee>>.Error(ex.Message);
            }
        }

        public async Task<Response<Employee>> GetById(int Id)
        {
            try
            {
                var result = await context.Employees
                                .FirstOrDefaultAsync(r => r.Id == Id);

                return Response<Employee>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<Employee>.Error(ex.Message);
            }
        }

        private void mapUser(Employee iEmployee, ref Employee oEmployee)
        {
            oEmployee.Id = iEmployee.Id;
            oEmployee.FirstName = iEmployee.FirstName;
            oEmployee.MiddleName = iEmployee.MiddleName;
            oEmployee.LastName = iEmployee.LastName;
        }

        private async Task<Response<Employee>> Add(Employee iEmployee)
        {
            var exists = await context.Employees
                        .AnyAsync(r => r.Id == iEmployee.Id);
            if (exists)
            {
                return Response<Employee>.Error("Record already exists.");
            }

            Employee oEmployee = new Employee();

            mapUser(iEmployee, ref oEmployee);

            context.Add(oEmployee);

            await context.SaveChangesAsync();

            oEmployee = Task.Run(async () => await GetById(oEmployee.Id)).Result.ResponseObject;

            return Response<Employee>.Success(oEmployee);
        }
        private async Task<Response<Employee>> Update(Employee iEmployee)
        {
            var oEmployee = await context.Employees
                              .FirstOrDefaultAsync(x => x.Id == iEmployee.Id);

            mapUser(iEmployee, ref oEmployee);

            context.Update(oEmployee);

            await context.SaveChangesAsync();

            oEmployee = Task.Run(async () => await GetById(oEmployee.Id)).Result.ResponseObject;

            return Response<Employee>.Success(oEmployee);
        }

        public async Task<Response<Employee>> AddUpdate(Employee item)
        {
            try
            {

                if (item.Id > 0)
                {
                    return await Update(item);
                }
                else
                {
                    return await Add(item);
                }
            }
            catch (DbUpdateException ex)
            {
                return Response<Employee>.Error(ex.InnerException.Message);
            }
        }


        public async Task<Response> Delete(int id)
        {
            try
            {
                var employee = await context.Employees.FirstOrDefaultAsync(c => c.Id == id);
                if (employee != null)
                {
                    context.Employees.Remove(employee);
                    await context.SaveChangesAsync();
                    return Response.Success();
                }
                return Response.Error("Record not found.");
            }
            catch (Exception ex)
            {
                return Response.Error(ex.Message);
            }
        }

    }
}
