using Prism.Mvvm;

namespace AwesomeUI.ViewModel;

public partial class BaseViewModel : BindableBase
{
    // [ObservableProperty]
    // [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    // bool isBusy;
    //
    // [ObservableProperty]
    // string title;
    //
    // public bool IsNotBusy => !IsBusy;
    
    private bool _isBusy;
    
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }
    
    public bool IsNotBusy => !IsBusy;
    
    private string _title = string.Empty;
    
    
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
    
}

