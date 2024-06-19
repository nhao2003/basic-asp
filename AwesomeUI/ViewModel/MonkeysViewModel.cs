using AwesomeUI.Services;
using AwesomeUI.View;

namespace AwesomeUI.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    public ObservableCollection<Blog> Blogs { get; } = new();
    BlogService _blogService;
    IConnectivity connectivity;

    public MonkeysViewModel(BlogService blogService, IConnectivity connectivity)
    {
        Title = "Monkey Finder";
        this._blogService = blogService;
        this.connectivity = connectivity;
    }

    [ObservableProperty] bool isRefreshing;

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No connectivity!",
                    $"Please check internet and try again.", "OK");
                return;
            }

            IsBusy = true;
            var blogs = await _blogService.GetBlogs();

            if (Blogs.Count != 0)
                Blogs.Clear();

            foreach (var blog in blogs ?? Enumerable.Empty<Blog>())
                Blogs.Add(blog);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get blogs: {ex.ToString()}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task GoToDetails(Blog? monkey)
    {
        if (monkey == null)
            return;

        await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
        {
            { "Monkey", monkey }
        });
    }

    [RelayCommand]
    async Task GetClosestMonkey()
    {
        if (IsBusy || Blogs.Count == 0)
            return;
        
        try
        {
        //     // Get cached location, else get real location.
        //     var location = await geolocation.GetLastKnownLocationAsync();
        //     if (location == null)
        //     {
        //         location = await geolocation.GetLocationAsync(new GeolocationRequest
        //         {
        //             DesiredAccuracy = GeolocationAccuracy.Medium,
        //             Timeout = TimeSpan.FromSeconds(30)
        //         });
        //     }

            // var first = Monkeys.OrderBy(m => location.CalculateDistance(
            //         new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
            //     .FirstOrDefault();
            //
            // await Shell.Current.DisplayAlert("", first.Author + " " +
            //                                      first.Location, "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to query location: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
}