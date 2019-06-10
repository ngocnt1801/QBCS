﻿using QBCS.Entity;
using QBCS.Service.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface ICourseService
    {
        CourseViewModel GetCourseById(int id);
        bool AddNewCourse(CourseViewModel model);
        List<CourseViewModel> GetAllCourses();
        List<CourseViewModel> GetAvailableCourse(int userId);
        List<CourseViewModel> GetAllCoursesByUserId(int id);
        List<Course> GetCoursesByName(string name);
        List<CourseViewModel> SearchCourseByNameOrCode(string searchContent);
        bool UpdateDisable(int id);
        CourseViewModel GetDetailCourseById(int id);
    }
}
