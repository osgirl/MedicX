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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using DustInTheWind.MedicX.Common.Entities;
using DustInTheWind.MedicX.Wpf.Areas.CurrentItemSelection.Commands;

namespace DustInTheWind.MedicX.Wpf.Areas.CurrentItemSelection.VewModels
{
    internal class MedicalEventsViewModel : ViewModelBase
    {
        private readonly MedicXProject medicXProject;
        private ViewModelBase selectedMedicalEvent;
        private readonly CollectionViewSource medicalEventsSource;
        private string searchText;

        public ICollectionView MedicalEvents { get; }

        public ViewModelBase SelectedMedicalEvent
        {
            get => selectedMedicalEvent;
            set
            {
                if (selectedMedicalEvent == value)
                    return;

                selectedMedicalEvent = value;
                OnPropertyChanged();

                switch (selectedMedicalEvent)
                {
                    case ConsultationListItemViewModel consultationListItemViewModel:
                        medicXProject.CurrentItem = consultationListItemViewModel.Value;
                        break;

                    case InvestigationListItemViewModel investigationListItemViewModel:
                        medicXProject.CurrentItem = investigationListItemViewModel.Value;
                        break;

                    default:
                        medicXProject.CurrentItem = null;
                        break;
                }
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();

                if (searchText == null)
                {
                    if (MedicalEvents.Filter != null) MedicalEvents.Filter = null;
                }
                else
                {
                    if (MedicalEvents.Filter == null)
                        MedicalEvents.Filter = FilterMedicalEvent;
                    else
                        MedicalEvents.Refresh();
                }
            }
        }

        public AddConsultationCommand AddConsultationCommand { get; }

        public AddInvestigationCommand AddInvestigationCommand { get; }

        public MedicalEventsViewModel(MedicXProject medicXProject)
        {
            this.medicXProject = medicXProject ?? throw new ArgumentNullException(nameof(medicXProject));

            AddConsultationCommand = new AddConsultationCommand(medicXProject);
            AddInvestigationCommand = new AddInvestigationCommand(medicXProject);

            medicalEventsSource = new CollectionViewSource
            {
                Source = new ObservableCollection<ViewModelBase>(medicXProject.MedicalEvents
                    .Select<MedicalEvent, ViewModelBase>(x =>
                    {
                        switch (x)
                        {
                            case Consultation consultation:
                                return new ConsultationListItemViewModel(consultation);

                            case Investigation investigation:
                                return new InvestigationListItemViewModel(investigation);

                            default:
                                return null;
                        }
                    })
                    .ToList())
            };
            MedicalEvents = medicalEventsSource.View;
            medicXProject.MedicalEvents.Added += HandleMedicalEventAdded;

            medicXProject.CurrentItemChanged += HandleCurrentItemChanged;
        }

        private void HandleMedicalEventAdded(object sender, MedicalEventAddedEventArgs e)
        {
            if (medicalEventsSource.Source is ObservableCollection<ViewModelBase> medicalEvents)
            {
                switch (e.MedicalEvent)
                {
                    case Consultation consultation:
                        ConsultationListItemViewModel consultationToBeAdded = new ConsultationListItemViewModel(consultation);
                        medicalEvents.Add(consultationToBeAdded);
                        break;

                    case Investigation investigation:
                        InvestigationListItemViewModel invetigationToBeAdded = new InvestigationListItemViewModel(investigation);
                        medicalEvents.Add(invetigationToBeAdded);
                        break;
                }
            }
        }

        private void HandleCurrentItemChanged(object sender, EventArgs e)
        {
            MedicalEvent medicalEvent = medicXProject.CurrentItem as MedicalEvent;

            if (medicalEvent == null)
                return;

            IEnumerable<ViewModelBase> clinicsViewModels = medicalEventsSource.Source as IEnumerable<ViewModelBase>;

            if (clinicsViewModels == null)
                return;

            SelectedMedicalEvent = clinicsViewModels.FirstOrDefault(x =>
            {
                switch (x)
                {
                    case ConsultationListItemViewModel consultation:
                        return consultation.Value == medicalEvent;

                    case InvestigationListItemViewModel investigation:
                        return investigation.Value == medicalEvent;

                    default:
                        return false;
                }
            });
        }

        private bool FilterMedicalEvent(object o)
        {
            if (string.IsNullOrEmpty(searchText))
                return true;

            switch (o)
            {
                case ConsultationListItemViewModel consultation:
                    return consultation.Value?.Contains(searchText) ?? false;

                case InvestigationListItemViewModel investigation:
                    return investigation.Value?.Contains(searchText) ?? false;

                default:
                    return false;
            }
        }
    }
}