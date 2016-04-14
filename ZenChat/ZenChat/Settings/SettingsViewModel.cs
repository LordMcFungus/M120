// Copyright (c) 2016 
// All rights reserved

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Annotations;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Settings
{
    internal class SettingsViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _phonenumber;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Phonenumber
        {
            get { return _phonenumber; }
            set
            {
                _phonenumber = value;
                OnPropertyChanged();
            }
        }


        public SettingsViewModel()
        {
            Username = Session.Username;
            Phonenumber = Session.PhoneNumber;
            Change = new DelegateCommand(SaveChanges);
        }


        public DelegateCommand Change { get; set; }

        public async void SaveChanges()
        {
            var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
            await client.ChangeUsernameAsync(Session.UserID, Username);
            await client.ChangePhoneNumberAsync(Session.UserID, Phonenumber);  
            var user = client.GetUserAsync(Phonenumber);
            Phonenumber = user.Result.PhoneNumber;
            Username = user.Result.Name;
            Session.PhoneNumber = user.Result.PhoneNumber;
            Session.Username = user.Result.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

}
