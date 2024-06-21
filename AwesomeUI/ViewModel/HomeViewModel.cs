using AwesomeUI.Services;
using AwesomeUI.View;

namespace AwesomeUI.ViewModel;

public partial class HomeViewModel : BaseViewModel
{
    public ObservableCollection<Blog> Blogs { get; } = new();
    BlogService _blogService;
    IConnectivity connectivity;

    public HomeViewModel( BlogService blogService, IConnectivity connectivity)
    {
        _blogService = blogService;
        this.connectivity = connectivity;
        Title = "Monkey Finder";
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
    async Task GoToDetails(Blog? blog)
    {
        if (blog == null)
            return;

        await Shell.Current.GoToAsync(nameof(BlogDetailPage), new Dictionary<string, object>
        {
            { "Blog", blog }
        });
    }
}