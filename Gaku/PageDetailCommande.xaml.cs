using System.ComponentModel;

namespace Gaku;

public partial class PageDetailCommande : ContentPage
{
    bool isChanging = false;

    Commande laCommande;
	public PageDetailCommande(Commande uneCommande)
	{
		InitializeComponent();
        this.laCommande = uneCommande;
        this.contentPageCommande.BindingContext = this.laCommande;
    }

    private async void contentPageCommande_Loaded(object sender, EventArgs e)
    {
        this.colViewStatuts.ItemsSource = await Contexte.GetStatutsCommande(this.laCommande.Id);
        this.colViewProduits.ItemsSource = await Contexte.GetProduitsCommande(this.laCommande.Id);
    }

    private async void colViewProduits_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!isChanging)
        {
            isChanging = true;
            ProduitCommande produitSelectionne = (ProduitCommande)e.CurrentSelection.FirstOrDefault();
            await Navigation.PushAsync(new PageDetailProduit(await Contexte.GetProduit(produitSelectionne.Id)));
            colViewProduits.SelectedItem = null;
        }
        else
        {
            isChanging = false;
        }
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        
    }

    private void Prompt_Close()
    {
        promptBg.FadeTo(0, 25);
        promptBorder.FadeTo(0, 25);
        promptBg.IsVisible = false;
        promptBorder.IsVisible = false;
    }

    private void btnMAJ_Clicked(object sender, EventArgs e)
    {
        promptBg.FadeTo(0.5, 25);
        promptBorder.FadeTo(1, 25);
        promptBg.IsVisible = true;
        promptBorder.IsVisible = true;
    }

    private void btnAnnuler_Clicked(object sender, EventArgs e)
    {
        Prompt_Close();
    }

    private void tapPromptBg_Tapped(object sender, TappedEventArgs e)
    {
        Prompt_Close();
    }

    private async void btnValider_Clicked(object sender, EventArgs e) { 
        try
        {
            DateTime dateTimeStatut = datePickStatut.Date + timePickStatut.Time;
            Statut nouveauStatut = await Contexte.UpdateCommande(laCommande.Id, dateTimeStatut);
            laCommande.IdStatutActuel = nouveauStatut.Id;
            laCommande.DateDernierStatut = nouveauStatut.DateStatut;
            Prompt_Close();
            colViewStatuts.ItemsSource = await Contexte.GetStatutsCommande(laCommande.Id);
        }
        catch (Exception ex) {
            await DisplayAlert("Erreur", "Une erreur est survenue.\nDétails : \n" + ex.Message, "OK");
        }
    }
}