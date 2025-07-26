using System.ComponentModel;
using WhoLetMeCook.ViewModels;

namespace WhoLetMeCook.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadHomepageDataCommand.CanExecute(null))
        {
            await _viewModel.LoadHomepageDataCommand.ExecuteAsync(null);
        }
    }

    private async void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(HomeViewModel.IsCategoryListVisible))
        {
            if (_viewModel.IsCategoryListVisible)
            {
                CategoryListContainer.InputTransparent = false;
                await CategoryListContainer.ScaleYTo(1, 250, Easing.SinOut);
                await CategoryListContainer.FadeTo(1, 100);
            }
            else
            {
                await CategoryListContainer.FadeTo(0, 100);
                await CategoryListContainer.ScaleYTo(0, 250, Easing.SinIn);
                CategoryListContainer.InputTransparent = true;
            }
        }
    }
}
