﻿using QBCS.Entity;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface ILevelService
    {
        List<LevelViewModel> GetLevel();
        int GetIdByName(string levelName);
        LevelViewModel GetLevelById(int levelId);
    }
}
