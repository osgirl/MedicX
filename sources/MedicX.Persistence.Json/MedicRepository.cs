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
using System.Collections.Generic;
using DustInTheWind.MedicX.Persistence.Json.Translators;
using Medic = DustInTheWind.MedicX.Common.Entities.Medic;

namespace DustInTheWind.MedicX.Persistence.Json
{
    internal class MedicRepository : IMedicRepository
    {
        private readonly JsonDatabase database;

        public MedicRepository(JsonDatabase database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            this.database = database;
        }

        public List<Medic> GetAll()
        {
            return database.Data.Medics.Translate();
        }
    }
}
