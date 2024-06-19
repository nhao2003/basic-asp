namespace AwesomeUI.ViewModel;

[QueryProperty(nameof(Blog), "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    readonly IMap _map;
    public MonkeyDetailsViewModel(IMap map)
    {
        this._map = map;
    }

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
