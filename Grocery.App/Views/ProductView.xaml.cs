using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class ProductView : ContentPage
{
	public ProductView(ProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is ProductViewModel bindingContext)
		{
			bindingContext.OnAppearing();
		}
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		if (BindingContext is ProductViewModel bindingContext)
		{
			bindingContext.OnDisappearing();
		}
	}
}