namespace Gaku;

public partial class PageDetailProduit : ContentPage
{

    Produit leProduit;

	public PageDetailProduit(Produit unProduit)
	{
		InitializeComponent();
        this.leProduit = unProduit;
        this.contentPageProduit.BindingContext = this.leProduit;
	}

    private async void btnAlerteSeuil_Clicked(object sender, EventArgs e)
    {
        string resultat = await DisplayPromptAsync("Modifier le seuil d'alerte","A partir de quelle quantité de stock souhaitez-vous être alerté ?","Modifier","Annuler","Saisissez un seuil",4,Keyboard.Telephone,this.leProduit.AlerteSeuil.ToString());
        if (!String.IsNullOrEmpty(resultat))
        {
            int seuilSouhaite = int.Parse(resultat);
            int nouveauSeuil = await Contexte.EditSeuilProduit(leProduit.Id, seuilSouhaite);
            leProduit.AlerteSeuil = nouveauSeuil;
            lblSeuil.Text = nouveauSeuil.ToString();
        }
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void contentPageProduit_Loaded(object sender, EventArgs e)
    {
        this.colViewCommandes.ItemsSource = await Contexte.GetCommandesProduit(this.leProduit.Id);
    }

    private void colViewCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}