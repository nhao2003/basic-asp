using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace AwesomeUI.ViewModel
{
    public class AccountViewModel : BaseViewModel
    {
        private bool _gender;
        private DateTime _birthDate;
        private string _fullName;

        public AccountViewModel()
        {
            UpdateAccountCommand = new Command(OnUpdateAccount);
        }

        public bool Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set => SetProperty(ref _birthDate, value);
        }

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public ICommand UpdateAccountCommand { get; }

        private void OnUpdateAccount()
        {
            // Logic to update account information goes here
        }
    }
}