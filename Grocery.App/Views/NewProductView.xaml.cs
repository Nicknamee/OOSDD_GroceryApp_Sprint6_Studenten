sing Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class NewProductView : ContentPage
{
	public NewProductView(NewProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is NewProductViewModel bindingContext)
		{
			bindingContext.OnAppearing();
		}
	}
}