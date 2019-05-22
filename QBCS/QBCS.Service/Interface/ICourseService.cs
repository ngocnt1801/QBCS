﻿using QBCS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    interface ICourseService
    {
        List<Course> GetCoursesByName(string name);
    }
}
