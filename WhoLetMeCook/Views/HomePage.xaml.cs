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
                // Animate In: Make it visible, then fade it in.
                CategoryBubblesContainer.IsVisible = true;
                await CategoryBubblesContainer.FadeTo(1, 250, Easing.SinOut);
            }
            else
            {
                // Animate Out: Fade it out, then make it invisible.
                await CategoryBubblesContainer.FadeTo(0, 250, Easing.SinIn);
                CategoryBubblesContainer.IsVisible = false;
            }
        }
    }
}
