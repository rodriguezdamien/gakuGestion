using System.Collections.ObjectModel;

namespace Gaku;

public partial class PageGestionCommandes : ContentPage
{
    ObservableCollection<Commande> lesCommandes;
    private bool isChanging = false;
	public PageGestionCommandes(ObservableCollection<Commande> desCommandes)
	{
		InitializeComponent();
        lesCommandes = desCommandes;
        this.colViewCommandes.ItemsSource = lesCommandes;
    }

    private async void colViewCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!isChanging)
        {
            isChanging = true;
            Commande commandeSelectionne = (Commande)e.CurrentSelection.FirstOrDefault();
            await Navigation.PushAsync(new PageDetailCommande(commandeSelectionne));
            this.colViewCommandes.SelectedItem = null;
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

    private void entryRecherche_TextChanged(object sender, TextChangedEventArgs e)
    {
        this.colViewCommandes.ItemsSource = (from Commande in lesCommandes
                                             where Commande.Id.ToString().Contains(entryRecherche.Text) || Commande.NomDestinataire.Contains(entryRecherche.Text)
                                             select Commande).ToList();
    }

    private void pickTrier_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch(pickTrier.SelectedIndex)
        {
            case 0:
                this.colViewCommandes.ItemsSource = lesCommandes.OrderBy(commande => commande.getDateCommande()).ToList();
                break;
            case 1:
                this.colViewCommandes.ItemsSource = lesCommandes.OrderByDescending(commande => commande.getDateCommande()).ToList();
                break;
            case 2:
                this.colViewCommandes.ItemsSource = lesCommandes.OrderBy(commande => commande.GetDateDernierStatut()).ToList();
                break;
            case 3:
                this.colViewCommandes.ItemsSource = lesCommandes.OrderByDescending(commande => commande.GetDateDernierStatut()).ToList();
                break;
        }
    }
}
