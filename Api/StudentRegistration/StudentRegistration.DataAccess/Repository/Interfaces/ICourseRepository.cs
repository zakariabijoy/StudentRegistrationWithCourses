﻿using StudentRegistration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.DataAccess.Repository.Interfaces
{
    public interface ICourseRepository: IRepository<Course>
    {
        Task<List<Course>> GetByIdListAsync(List<int> id);
        
    }
}
