using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gaku
{
    public class Commande : INotifyPropertyChanged
    {


        //BEAUCOUP TROP de champs dont on pourrait se passer
        public int Id {  get; set; }

        public string NomDestinataire {  get; set; }
        public string PrenomDestinataire { get; set; }
        public string AppelationDestinataire { get { return $"{PrenomDestinataire} {NomDestinataire}"; } }

        private DateTime _dateHeure;
        //merci madame mon dieu enfin
        public String DateHeure
        {
            get { return _dateHeure.ToString("F"); }
            set { _dateHeure = DateTime.Parse(value); }
        }

        //ça n'a aucun sens
        public DateTime getDateCommande()
        {
            return _dateHeure;
        }
        

        public string AdresseLivraison { get; set; }
        public string VilleLivraison { get; set; }
        public string CpLivraison { get; set; }

        //pas encore implémenté
        public string NumSuivi { get; set; }
        [JsonPropertyName("numeroTel")]
        public string NumTelDestinataire {  get; set; }

        // Collection de statut ?

        private int _idStatutActuel;
        public int IdStatutActuel { 
            get {
                return _idStatutActuel;    
            } 
            set {
                _idStatutActuel = value;
                OnPropertyChanged(nameof(IdStatutActuel));
            }
        }
        
        
        private DateTime _dateDernierStatut;
        //merci madame mon dieu enfin
        public String DateDernierStatut
        {
            get { return _dateDernierStatut.ToString(); }
            set { _dateDernierStatut = DateTime.Parse(value);
                OnPropertyChanged(nameof(DateDernierStatut));
            }
        }

        public DateTime GetDateDernierStatut()
        {
            return _dateDernierStatut;
        }

        public List<Produit> LesProduits { get; set; }
        public string Note { get;set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class Client
    {

        public int Id { get; set; }
        public string Prenom { get; set; }
        public string Nom {  get; set; }
        public string Mail { get; set; }
        public string Appelation { get { return Prenom + " " + Nom ; } }
       }

    //j'ai a peu près compris le INotifyPropertyChanged mais faut vraiiiiiiiiiiment que je me penche dessus
    public class Produit : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public string UriImage { get; set; }
        public string IdEvent { get; set; }
        public int NumEdition { get; set; }
        public double Prix { get; set; }
        public string AffichageID { get {
                string affichage = "";
                if (IdEvent == "MISC")
                    affichage = $"MISC-{Id}";
                else
                    affichage = $"{IdEvent}-{NumEdition}-{Id}";
                return affichage ; } }

        [JsonPropertyName("qte")]
        public int Stock { get; set; }

        public string QteCommande { get; set; }


        private int alterteseuil;
        public int AlerteSeuil {
            get {
                return alterteseuil;
            } 
            set { 
                alterteseuil = value;
                OnPropertyChanged(nameof(AlerteSeuil)); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsEnAlerte { get => Stock < AlerteSeuil; }

        public string NomCreateur { get; set; }

        public string IdStatutActuel { get; set; }

        private DateTime dateStatutActuel;
        //merci madame mon dieu enfin
        public String DateStatutActuel
        {
            get { return dateStatutActuel.ToString(); }
            set { dateStatutActuel = DateTime.Parse(value); }
        }

        
    }

    public class ProduitCommande
    {

        [JsonPropertyName("idAlbum")]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string NomCreateur { get; set; }
        public int Qte { get; set; }
        public string UriImage { get; set; }

    }

    public class Statut
    {
        [JsonPropertyName("IdStatut")]
        public int Id { get; set; }
       
        private DateTime dateStatut;
        //merci madame mon dieu enfin
        public String DateStatut
        {
            get { return dateStatut.ToString(); }
            set { dateStatut = DateTime.Parse(value); }
        }

    }

    public class Event
    {
        public Event()
        {
            Id = null;
            Nom = "Tous";
        }

        public string Id { get; set; }
        public string Nom { get; set; }

        public override string ToString()
        {
            return Nom;
        }
    }

    public class EditionEvent
    {
        public EditionEvent()
        {
            IdEvent = null;
            numEdition = 0;
        }

        public string IdEvent { get; set; }
        public int numEdition { get; set; }

        public override string ToString()
        {
            if (numEdition == 0)
                return "Tous";
            else
                return $"{IdEvent}-{numEdition}";
        }
    }
}
