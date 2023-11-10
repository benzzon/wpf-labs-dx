﻿//using CommunityToolkit.Mvvm.ComponentModel; //Previous name "Microsoft.Toolkit.Mvvm.ComponentModel"
//using CommunityToolkit.Mvvm.Input; // Previous name "Microsoft.Toolkit.Mvvm.Input"
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using LabsUI.Models;
using Notifications.Wpf.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using System;
//using YourApp.Models;

namespace LabsUI.ViewModels
{
    [POCOViewModel]
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<PersonModel> People { get; set; } = new ObservableCollection<PersonModel>();

        const double defaultWinHeight = 300;
        const double defaultWinWidth = 600;

        private PersonModel _selectedP;
        [BindableProperty]
        public virtual PersonModel SelectedPerson
        {
            get { return _selectedP; }
            set { 
                SetProperty(ref _selectedP, value, nameof(_selectedP));
                RaisePropertyChanged(nameof(SelectedPerson));
            }
        }
        public MainWindowViewModel()
        {
            Application.Current.MainWindow.Left = LabsUI.Properties.Settings.Default.WinLeft;
            Application.Current.MainWindow.Top = LabsUI.Properties.Settings.Default.WinTop;
            Application.Current.MainWindow.Height = LabsUI.Properties.Settings.Default.WinHeight == 0 ? defaultWinHeight : LabsUI.Properties.Settings.Default.WinHeight;
            Application.Current.MainWindow.Width = LabsUI.Properties.Settings.Default.WinWidth == 0 ? defaultWinWidth : LabsUI.Properties.Settings.Default.WinWidth;

            // Initialize the ObservableCollection
            People = new ObservableCollection<PersonModel>();

            // Initialize the SelectedPerson
            //SelectedPerson = new PersonModel();

            genders = new string[] { "Male", "Female" };
        }

        public ICommand DoNewCommand => new DelegateCommand(DoNew);
        private void DoNew()
        {
            SelectedPerson = new PersonModel();
            RaisePropertyChanged(nameof(SelectedPerson));
            ShowNotification("New Person", "Enter person data, and then click the button 'Add'.", NotificationType.Information);
        }
        private async void ShowNotification(string title, string message, NotificationType notifType)
        {
            NotificationManager notificationManager = new NotificationManager();

            var notificationContent = new NotificationContent
            {
                Title = title,
                Message = message,
                Type = notifType
            };

            await notificationManager.ShowAsync(notificationContent, areaName: "WindowArea");
        }

        public ICommand DoAddCommand => new DelegateCommand(DoAdd);
        private void DoAdd()
        {
            if (SelectedPerson?.PersonName == null) // Name is required..
            {
                return;
            }

            // Create a new PersonModel and add it to the ObservableCollection
            var newPerson = new PersonModel
            {
                PersonName = SelectedPerson.PersonName,
                Email = SelectedPerson.Email,
                Gender = SelectedPerson.Gender,
                PhoneNumber = SelectedPerson.PhoneNumber
            };

            People.Add(newPerson);

            // Clear the input fields
            SelectedPerson = new PersonModel();
            ShowNotification("Added person", "Person was added to the list of persons..", NotificationType.Success);
        }

        public ICommand DoDeleteCommand => new DelegateCommand(DoDelete);
        private void DoDelete()
        {
            People.Remove(SelectedPerson);
            SelectedPerson = new PersonModel(); // Clear the input fields
            ShowNotification("Deleted person", "Person was removed from the list of persons..", NotificationType.Warning);
        }

        public ICommand DoLoadCommand => new DelegateCommand(DoLoad);
        private void DoLoad()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<PersonModel>));

                List<PersonModel> temp = new List<PersonModel>();

                using (XmlReader reader = XmlReader.Create("people.xml"))
                {
                    temp = (List<PersonModel>)ser.Deserialize(reader);
                }

                People.Clear();
                temp.ForEach(x => People.Add(x));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occurred..");
            }
            ShowNotification("Loaded persons", "Data was loaded to the list of persons..", NotificationType.Success);
        }

        public ICommand DoSaveCommand => new DelegateCommand(DoSave);
        private void DoSave()
        {
            if (People == null) 
            {
                return; 
            }

            // Serialize and save the data to an XML file.
            var serializer = new XmlSerializer(typeof(ObservableCollection<PersonModel>));
            using (var writer = new System.IO.StreamWriter("people.xml"))
            {
                serializer.Serialize(writer, People);
            }

            CollectionViewSource.GetDefaultView(People).Refresh(); // Force refresh of grid to show any changes to person-fields..
            ShowNotification("Saved persons", "Data for persons was saved..", NotificationType.Success);
        }

        public ICommand DoSearchCommand => new DelegateCommand<string>(DoSearch);
        private void DoSearch(string textToSearch)
        {
            var coll = CollectionViewSource.GetDefaultView(People);
            if (!string.IsNullOrWhiteSpace(textToSearch))
                coll.Filter = c => ((PersonModel)c).PersonName.ToLower().Contains(textToSearch.ToLower());
            else
                coll.Filter = null;
        }

        public ICommand DoCloseCommand => new DelegateCommand(DoClose);
        private void DoClose()
        {
            var result = MessageBox.Show("Close application?", "Close", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var w = Application.Current.Windows[0];
                w.Close();
            }
        }

        private IEnumerable<string> genders;
        public IEnumerable<string> Genders
        {
            get { return genders; }
            set
            {
                genders = value;
                //OnPropertyChanged("Currencies");
            }
        }
    }
}
