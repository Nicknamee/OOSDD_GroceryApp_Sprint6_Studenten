using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Grocery.App.ViewModels
{
	public partial class ProductViewModel : BaseViewModel
	{
        private readonly IProductService _productService;
		private readonly GlobalViewModel _globalViewModel;
		public ObservableCollection<Product> Products { get; }

		public Client? Client => _globalViewModel.Client;

		public bool CanCreateProduct => Client?.Role == Role.Admin;

		public ProductViewModel(IProductService productService, GlobalViewModel globalViewModel)

        {
			Title = "Producten";
			_productService = productService;
			_globalViewModel = globalViewModel;
			Products = new ObservableCollection<Product>();
			LoadProducts();
		}

		[RelayCommand]
		private async Task CreateNewProductAsync()
		{
			if (!CanCreateProduct)
			{
				return;
			}

			await Shell.Current.GoToAsync(nameof(NewProductView));
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			LoadProducts();
			OnPropertyChanged(nameof(Client));
			OnPropertyChanged(nameof(CanCreateProduct));
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Products.Clear();
		}

		private void LoadProducts()
		{
			Products.Clear();
			foreach (Product product in _productService.GetAll())
			{
				Products.Add(product);
			}
		}
    }
}
