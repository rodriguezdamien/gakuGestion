using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gaku;

public partial class PageGestionStock : ContentPage
{
    ObservableCollection<Produit> lesProduits;
    bool isChanging = false;
    public PageGestionStock(ObservableCollection<Produit> desProduits)
    {
        InitializeComponent();
        lesProduits = desProduits;
        colViewProduits.ItemsSource = lesProduits;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        pickEvent.ItemsSource = await Contexte.GetEvents();
        pickEvent.ItemsSource.Add(new Event());
    }

    private async void colViewProduits_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!isChanging)
        {
            isChanging = true;
            Produit produitSelectionne = (Produit)e.CurrentSelection.FirstOrDefault();
            await Navigation.PushAsync(new PageDetailProduit(produitSelectionne));
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

    //Le Task.Run() n'est qu'un test, je ne sais absolument pas si c'est plus optimisé ou plus rapide, l'objectif de départ étant d'éviter un freeze de l'UI
    private async void Filtrage(object sender, EventArgs e)
    {
        await colViewProduits.FadeTo(0.5,250,Easing.BounceIn);
        string recherche = $"{entryRecherche.Text}";
        List<Produit> lesProduitsFiltre = null;
        if (pickEvent.SelectedItem == null || ((Event)pickEvent.SelectedItem).Id == null)
        {
            lesProduitsFiltre = await Task.Run(() => (from Produit in lesProduits
                                                                    where Produit.Nom.Contains(recherche) || Produit.NomCreateur.Contains(recherche)
                                                                    select Produit).ToList());
        }
        else
        {
            string idEventFiltre = ((Event)pickEvent.SelectedItem).Id;
            if (pickEdition.SelectedItem != null && ((EditionEvent)pickEdition.SelectedItem).numEdition > 0)
            {
                lesProduitsFiltre = await Task.Run(() => (from Produit in lesProduits
                                                          where Produit.IdEvent == idEventFiltre && Produit.NumEdition == ((EditionEvent)pickEdition.SelectedItem).numEdition && (Produit.Nom.Contains(recherche) || Produit.NomCreateur.Contains(recherche))
                                                          select Produit).ToList());
            }
            else
            {
                lesProduitsFiltre = await Task.Run(() => (from Produit in lesProduits
                                                          where Produit.IdEvent == idEventFiltre && (Produit.Nom.Contains(recherche) || Produit.NomCreateur.Contains(recherche))
                                                          select Produit).ToList());
            }
        }
        
        this.colViewProduits.ItemsSource = lesProduitsFiltre;
        await colViewProduits.FadeTo(1, 250, Easing.BounceOut);
    }

    private async void pickEvent_SelectedIndexChanged(object sender, EventArgs e)
    {
        pickEdition.ItemsSource.Clear();
        if (((Event)pickEvent.SelectedItem).Id == null)
        {
            pickEdition.ItemsSource = await Contexte.GetEditionsEvenement(((Event)pickEvent.SelectedItem).Id);
            pickEdition.ItemsSource.Add(new EditionEvent());
            pickEdition.IsVisible = true;
        }
        else
        {
            pickEdition.IsVisible = false;
        }
        Filtrage(sender, e);
    }
}
