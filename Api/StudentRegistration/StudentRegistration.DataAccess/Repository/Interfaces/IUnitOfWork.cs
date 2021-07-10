﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.DataAccess.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        ICourseRepository Courses { get; set; }
    }
}
