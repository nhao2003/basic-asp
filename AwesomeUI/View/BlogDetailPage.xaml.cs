namespace AwesomeUI.View;

public partial class BlogDetailPage : ContentPage
{
    public BlogDetailPage(BlogDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}