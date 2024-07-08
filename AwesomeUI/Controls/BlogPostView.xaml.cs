using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace AwesomeUI.Controls
{
    public partial class BlogPostView : ContentView
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command),
            typeof(ICommand),
            typeof(BlogPostView), null, propertyChanged: (bindable, value, newValue) =>
            {
                var control = (BlogPostView)bindable;
                control.TapGestureRecognizer.Command = (ICommand)newValue;
            });
        
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter",
            typeof(object),
            typeof(BlogPostView), null, propertyChanged: (bindable, value, newValue) =>
            {
                var control = (BlogPostView)bindable;
                control.TapGestureRecognizer.CommandParameter = newValue;
            });

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string),
            typeof(BlogPostView),
            propertyChanged:
            (bindable, value, newValue) =>
            {
                var control = (BlogPostView)bindable;
                control.TitleLabel.Text = (string)newValue;
            });

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description),
            typeof(string),
            typeof(BlogPostView),
            propertyChanged:
            (bindable, value, newValue) =>
            {
                var control = (BlogPostView)bindable;
                control.DescriptionLabel.Text = (string)newValue;
            });

        public string Date
        {
            get => (string)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(nameof(Date), typeof(string), typeof(BlogPostView), string.Empty, propertyChanged:
                (bindable, value, newValue) =>
                {
                    var control = (BlogPostView)bindable;
                    control.DateLabel.Text = (string)newValue;
                });

        public string Image
        {
            get => (string)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(string), typeof(BlogPostView), string.Empty, propertyChanged:
                (bindable, value, newValue) =>
                {
                    var control = (BlogPostView)bindable;
                    try
                    {
                        control.ThumbnailImage.Source = (string)newValue switch
                        {
                            string url when Uri.IsWellFormedUriString(url, UriKind.Absolute) => ImageSource.FromUri(
                                new Uri(url)),
                            _ => throw new ArgumentException("Invalid URL format"),
                        };
                    }
                    catch
                    {
                        // Handle exception, e.g., log or set a default image
                        control.ThumbnailImage.Source = ImageSource.FromFile("default_image.png");
                    }
                });

        public string Category
        {
            get => (string)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }

        public static readonly BindableProperty CategoryProperty =
            BindableProperty.Create(nameof(Category), typeof(string), typeof(BlogPostView), string.Empty,
                propertyChanged:
                (bindable, value, newValue) =>
                {
                    var control = (BlogPostView)bindable;
                    control.CategoriesLabel.Text = (string)newValue;
                });

        public BlogPostView()
        {
            InitializeComponent();
        }

    }
}