﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Repository.Interface
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        IImportRepository ImportRepository();
        IExaminationRepository ExaminationRepository();
        int SaveChanges();
        DbContext GetContext();
    }
}
