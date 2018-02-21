﻿// MedicX
// Copyright (C) 2017-2018 Dust in the Wind
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
using System.Linq;

namespace DustInTheWind.MedicX.Common.Entities
{
    public class MedicalEvent
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public Clinic Clinic { get; set; }

        public List<string> Labels { get; set; }

        public string Comments { get; set; }

        public virtual void CopyFrom(MedicalEvent medicalEvent)
        {
            Id = medicalEvent.Id;
            Date = medicalEvent.Date;
            Clinic = medicalEvent.Clinic;
            Labels = medicalEvent.Labels.ToList();
            Comments = medicalEvent.Comments;
        }

        public virtual bool Contains(string text)
        {
            return (Clinic != null && Clinic.Contains(text)) ||
                   (Date.ToString().IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                   (Labels != null && Labels.Any(x => x != null && x.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)) ||
                   (Comments != null && Comments.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}