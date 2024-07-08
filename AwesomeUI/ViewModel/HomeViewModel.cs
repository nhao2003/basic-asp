using AwesomeUI.Services;
using AwesomeUI.View;
using Prism.Commands;

namespace AwesomeUI.ViewModel;

public partial class HomeViewModel : BaseViewModel
{
    public ObservableCollection<Blog> Blogs { get; } = new();
    private readonly BlogService _blogService;
    public DelegateCommand GetBlogsCommand { get; private set; }
    public HomeViewModel( BlogService blogService)
    {
        _blogService = blogService;
        Title = "Blog";
        GetBlogsCommand = new DelegateCommand(async () => await GetBlogsAsync());
    }

    private bool _isRefreshing;
    
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    public async Task GetBlogsAsync()
    {
        if (IsBusy)
            return;

        try
        {
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
    
    [RelayCommand]
    async Task OpenQrCodePage()
    {
        await Shell.Current.Navigation.PushAsync(new QrCodePage());
    }
}