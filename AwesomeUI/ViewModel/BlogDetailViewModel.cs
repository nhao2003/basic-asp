namespace AwesomeUI.ViewModel;

[QueryProperty(nameof(Blog), "Blog")]
public partial class BlogDetailViewModel : BaseViewModel
{
    private Blog? _blog;
    
    public Blog? Blog
    {
        get => _blog;
        set => SetProperty(ref _blog, value);
    }
}