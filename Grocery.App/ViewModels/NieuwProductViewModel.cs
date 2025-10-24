using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;
using static Java.Util.Jar.Attributes;

namespace Grocery.App.ViewModels
{
	public partial class NewProductViewModel : BaseViewModel
	{
		private readonly IProductService _productService;
		private readonly GlobalViewModel _globalViewModel;

		[ObservableProperty]
		private string name = string.Empty;

		[ObservableProperty]
		private string stockText = "0";

		[ObservableProperty]
		private DateTime shelfLife = DateTime.Today;

		[ObservableProperty]
		private string priceText = "0";

		[ObservableProperty]
		private string errorMessage = string.Empty;

		public bool CanCreateProduct => _globalViewModel.Client?.Role == Role.Admin;

		public NewProductViewModel(IProductService productService, GlobalViewModel globalViewModel)
		{
			Title = "Nieuw product";
			_productService = productService;
			_globalViewModel = globalViewModel;
		}

		[RelayCommand]
		private async Task SaveAsync()
		{
			if (!CanCreateProduct)
			{
				await Shell.Current.DisplayAlert("Geen toegang", "Geen rechten om een product aan te maken.", "OK");
				return;
			}

			if (!Validate(out int stockValue, out decimal priceValue))
			{
				return;
			}

			try
			{
				Product product = new(0, Name.Trim(), stockValue, DateOnly.FromDateTime(ShelfLife.Date), priceValue);
				_productService.Add(product);
				await Shell.Current.GoToAsync("..");
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}
		}

		[RelayCommand]
		private async Task CancelAsync()
		{
			await Shell.Current.GoToAsync("..");
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			if (!CanCreateProduct)
			{
				_ = Shell.Current.GoToAsync("..");
			}
		}

		partial void OnNameChanged(string value) => ClearError();
		partial void OnStockTextChanged(string value) => ClearError();
		partial void OnShelfLifeChanged(DateTime value) => ClearError();
		partial void OnPriceTextChanged(string value) => ClearError();

		private bool Validate(out int stockValue, out decimal priceValue)
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				ErrorMessage = "Naam is verplicht.";
				stockValue = default;
				priceValue = default;
				return false;
			}

			if (!int.TryParse(StockText, out stockValue) || stockValue < 0)
			{
				ErrorMessage = "Voorraad moet 0 of hoger zijn.";
				priceValue = default;
				return false;
			}

			if (!decimal.TryParse(PriceText, NumberStyles.Number, CultureInfo.CurrentCulture, out priceValue) || priceValue < 0)
			{
				ErrorMessage = "Prijs moet 0 of hoger zijn.";
				priceValue = default;
				return false;
			}

			if (ShelfLife.Date < DateTime.Today)
			{
				ErrorMessage = "THT datum kan niet in het verleden liggen.";
				priceValue = default;
				stockValue = default;
				return false;
			}

			return true;
		}

		private void ClearError()
		{
			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				ErrorMessage = string.Empty;
			}
		}
	}
}