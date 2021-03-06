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
using DustInTheWind.MedicX.Common.Entities;

namespace DustInTheWind.MedicX.Wpf.Areas.CurrentItemSelection.VewModels
{
    internal class ClinicListItemViewModel : ViewModelBase
    {
        private string name;
        private string address;

        public Clinic Clinic { get; }

        public string Name
        {
            get => name;
            private set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => address;
            private set
            {
                address = value;
                OnPropertyChanged();
            }
        }

        public ClinicListItemViewModel(Clinic clinic)
        {
            Clinic = clinic ?? throw new ArgumentNullException(nameof(clinic));

            UpdateDisplayedText();
            UpdateDisplayedAddress();

            clinic.NameChanged += HandleMedicNameChanged;
            clinic.AddressChanged += HandleAddressChanged;
        }

        private void HandleMedicNameChanged(object sender, EventArgs e)
        {
            UpdateDisplayedText();
        }

        private void HandleAddressChanged(object sender, EventArgs eventArgs)
        {
            UpdateDisplayedAddress();
        }

        private void UpdateDisplayedText()
        {
            string name = Clinic.Name;

            Name = string.IsNullOrEmpty(name)
                ? "<no name>"
                : name;
        }

        private void UpdateDisplayedAddress()
        {
            Address = Clinic.Address.ToString();
        }
    }
}