using Microsoft.Extensions.DependencyInjection;
using StudentRegistration.DataAccess.Repository;
using StudentRegistration.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.DataAccess
{
    public static class ServiceRegistration
    {
        public static void AddDataAccessServices(this IServiceCollection services)
        {
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
