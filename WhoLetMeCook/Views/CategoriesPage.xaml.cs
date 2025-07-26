using WhoLetMeCook.ViewModels;

namespace WhoLetMeCook.Views;

public partial class CategoriesPage : ContentPage
{
    private readonly CategoriesViewModel _viewModel;
    public CategoriesPage(CategoriesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadDataCommand.CanExecute(null))
        {
            await _viewModel.LoadDataCommand.ExecuteAsync(null);
        }
    }
}
