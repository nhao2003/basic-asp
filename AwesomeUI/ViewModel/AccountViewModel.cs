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
        private readonly UserService _userService;

        public AccountViewModel(UserService userService)
        {
            _userService = userService;
            Title = "Account";
            UpdateAccountCommand = new DelegateCommand(async () => await UpdateAccount());
            PickImageCommand = new DelegateCommand(async () => await PickImage());
            OpenCameraCommand = new DelegateCommand(async () => await OpenCamera());
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
                ProfilePicture = user?.Avatar  == null ? null : ImageSource.FromUri(new Uri(user.Avatar));
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


        private ImageSource? _profilePicture;

        public ImageSource? ProfilePicture
        {
            get => _profilePicture;
            set => SetProperty(ref _profilePicture, value);
        }

        public DelegateCommand PickImageCommand { get; private set; }
        
        public DelegateCommand OpenCameraCommand { get; private set; }
    
        private bool _isUploadingImage = false;
        
        public bool IsUploadingImage
        {
            get => _isUploadingImage;
            set => SetProperty(ref _isUploadingImage, value);
        }
       private async Task UploadImage(FileResult file)
        {
            try
            {
                IsUploadingImage = true;
                var response = await _userService.UploadProfilePictureAsync(file);
                if (response)
                {
                    await Shell.Current.DisplayAlert("Success!", "Image uploaded successfully.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error!", "Unable to upload image.", "OK");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsUploadingImage = false;
            }
        }

        async Task PickImage()
        {
            try
            {
                // Open file picker to select image
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please pick a photo"
                });

                // Check if an image was selected
                if (result != null)
                {
                    // Open a stream to the selected file
                    var stream = await result.OpenReadAsync();

                    // Set the selected image to ProfilePicture property
                    ProfilePicture = ImageSource.FromStream(() => stream);
                    
                    // Upload the selected image
                    await UploadImage(result);
                }
            }
            catch (Exception ex)
            {
                // Handle error here
                Console.WriteLine($"PickPhotoAsync THREW: {ex.Message}");
            }
        }

        private async Task OpenCamera()
        {
            try
            {
                // Open camera to take a photo
                var options = new MediaPickerOptions()
                {
                    Title = "Please take a photo",
                };
                var result = await MediaPicker.CapturePhotoAsync(options);

                // Check if an image was taken
                if (result != null)
                {
                    // Open a stream to the taken photo
                    var stream = await result.OpenReadAsync();

                    // Set the taken image to ProfilePicture property
                    ProfilePicture = ImageSource.FromStream(() => stream);
                    
                    // Upload the taken image
                    await UploadImage(result);
                }
            }
            catch (Exception ex)
            {
                // Handle error here
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }
    }
}