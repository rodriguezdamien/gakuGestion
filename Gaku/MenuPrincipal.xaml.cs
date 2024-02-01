namespace Gaku;

public partial class MenuPrincipal : ContentPage
{
	public MenuPrincipal()
	{
		InitializeComponent();
	}

	private async void btnGestionStock_Clicked(object sender, EventArgs e) {
		try
		{
			loadingIndicator.IsVisible = true;
			loadingBg.IsVisible = true;
			await loadingBg.FadeTo(0.5, 50);
			PageGestionStock gestionStock = new PageGestionStock(await Contexte.GetProduits());
			await Navigation.PushAsync(gestionStock);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erreur", "Une erreur est survenue.\nDétails : \n" + ex.Message, "OK");
		}
		finally
		{
            loadingBg.IsVisible = false;
            loadingBg.Opacity = 0;
            loadingIndicator.IsVisible = false;
        }
    }

	private async void btnGestionCommandes_Clicked(object sender, EventArgs e)
	{
		try
		{
			loadingIndicator.IsVisible = true;
			loadingBg.IsVisible = true;
			await loadingBg.FadeTo(0.5, 50);
			await Navigation.PushAsync(new PageGestionCommandes(await Contexte.GetCommandes()));
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erreur", "Une erreur est survenue.\nDétails : \n" + ex.Message, "OK");
		}
		finally
		{
			loadingBg.IsVisible = false;
			loadingBg.Opacity = 0;
			loadingIndicator.IsVisible = false;
		}
	}
}