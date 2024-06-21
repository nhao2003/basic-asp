namespace AwesomeUI.ViewModel;

[QueryProperty(nameof(Blog), "Blog")]
public partial class BlogDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    Blog _blog;
}
