namespace AwesomeUI.ViewModel;

[QueryProperty(nameof(Blog), "Monkey")]
public partial class BlogDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    Blog _blog;

    [RelayCommand]
    async Task OpenMap()
    {
        try
        {
            // await _map.OpenAsync(Blog.Latitude, Blog.Longitude, new MapLaunchOptions
            // {
            //     Name = Blog.Author,
            //     NavigationMode = NavigationMode.None
            // });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            await Shell.Current.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
        }
    }
}
