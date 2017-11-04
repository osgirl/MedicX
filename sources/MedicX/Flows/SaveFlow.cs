﻿// MedicX
// Copyright (C) 2017 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using DustInTheWind.MedicX.Persistence.Json;
using DustInTheWind.MedicX.Utils;

namespace DustInTheWind.MedicX.Flows
{
    public class SaveFlow : IFlow
    {
        private readonly UnitOfWork unitOfWork;

        public SaveFlow(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            this.unitOfWork = unitOfWork;
        }

        public void Run()
        {
            unitOfWork.Save();
            CustomConsole.WriteLineSuccess("Changes were successfully saved.");
        }
    }
}