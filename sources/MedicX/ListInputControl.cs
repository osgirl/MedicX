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
using DustInTheWind.ConsoleTools;

namespace DustInTheWind.MedicX
{
    internal class ListInputControl
    {
        public string Separator { get; set; } = ":";

        public List<string> Read(string label)
        {
            CustomConsole.WriteEmphasies(label);
            CustomConsole.WriteLineEmphasies(Separator);

            List<string> values = new List<string>();

            while (true)
            {
                Console.Write(" - ");
                string value = Console.ReadLine();

                if (string.IsNullOrEmpty(value))
                    break;

                values.Add(value);
            }

            return values;
        }
    }
}