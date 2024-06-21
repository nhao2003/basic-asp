using System;
using System.Windows.Input;
using AwesomeUI.DTO.User;
using AwesomeUI.Services;
using Prism.Commands;

namespace AwesomeUI.ViewModel
{
    public partial class AccountViewModel : BaseViewModel
    {
        private DateTime? _birthDate;
        private string? _fullName;
        public DelegateCommand UpdateAccountCommand { get; private set; }
        private UserService _userService;

        public AccountViewModel(UserService userService)
        {
            _userService = userService;
            Title = "Account";
            UpdateAccountCommand = new DelegateCommand(async () => await UpdateAccount());
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set => SetProperty(ref _birthDate, value);
        }

        public string? FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        async Task UpdateAccount()
        {
            try
            {
                IsBusy = true;
                var user = new UpdateProfileDto()
                {
                    FullName = FullName ?? string.Empty,
                    DateOfBirth = DateOnly.FromDateTime(BirthDate ?? DateTime.Now)
                };

                var response = await _userService.UpdateUserAsync(user);

                if (response)
                {
                    await Shell.Current.DisplayAlert("Success!", "Profile updated successfully.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error!", "Unable to update profile.", "OK");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetUser()
        {
            try
            {
                IsBusy = true;
                var user = await _userService.GetUserAsync();
                FullName = user?.FullName;
                BirthDate = user?.DateOfBirth ?? DateTime.FromOADate(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}