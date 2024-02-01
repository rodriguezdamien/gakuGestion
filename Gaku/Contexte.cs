using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gaku
{
    public class Contexte
    { 
        public static string UrlServiceWeb
        {
            get
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                    return "http://10.0.2.2/gakuAPI";
                else //Windows
                    return "http://localhost/gakuAPI";
            }
        }
        public static HttpClient httpClient = new HttpClient();

        public static async Task<ObservableCollection<Commande>> GetCommandes()
        {
            string urlAPI = UrlServiceWeb + "/commandes/";

            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                ObservableCollection<Commande> lesCommandes = JsonSerializer.Deserialize<ObservableCollection<Commande>>(contenu, optionJson);

                return lesCommandes;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode);
            }
        }

        public static async Task<ObservableCollection<Commande>> GetCommandesProduit(int id)
        {
            string urlAPI = UrlServiceWeb + "/albums/" + id +"/commandes/" ;

            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                ObservableCollection<Commande> lesCommandes = JsonSerializer.Deserialize<ObservableCollection<Commande>>(contenu, optionJson);

                return lesCommandes;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode);
            }
        }

        public static async Task<ObservableCollection<Produit>> GetProduits()
        {
            string urlAPI = UrlServiceWeb + "/albums/";

            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                ObservableCollection<Produit> lesProduits = JsonSerializer.Deserialize<ObservableCollection<Produit>>(contenu, optionJson);

                return lesProduits;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode);
            }
        }

        public static async Task<Produit> GetProduit(int id)
        {
            string urlAPI = UrlServiceWeb + "/albums/" + id + "/";

            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();
                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                ObservableCollection<Produit> lesProduits = JsonSerializer.Deserialize<ObservableCollection<Produit>>(contenu, optionJson);
                Produit leProduit = lesProduits[0];

                return leProduit;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode);
            }
        }

        /// <summary>
        /// Retourne le client de la commande passée en paramètre.
        /// </summary>
        /// <param name="id">ID du client</param>
        /// <exception cref="Exception">Erreur survenue durant la récupération du client</exception>
        public static async Task<Client> GetLeClient(int id)
        {
            string urlAPI = UrlServiceWeb + "/utilisateur/" + id + "/";
            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();
                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                Client leClient = JsonSerializer.Deserialize<Client>(contenu, optionJson);
                return leClient;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode);
            }
        }
        /// <summary>
        /// Récupère les produits d'une commande.
        /// </summary>
        /// <param name="id">ID de la commande.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<ObservableCollection<ProduitCommande>> GetProduitsCommande(int id)
        {
            string urlAPI = UrlServiceWeb + "/commandes/" + id + "/contenu/";

            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                ObservableCollection<ProduitCommande> lesProduits = JsonSerializer.Deserialize<ObservableCollection<ProduitCommande>>(contenu, optionJson);

                return lesProduits;
            }
            else
            {
                throw new Exception("Erreur au chargement des commandes du produit : " + resultatRequete.StatusCode);
            }
        }

        public static async Task<ObservableCollection<Statut>> GetStatutsCommande(int id)
        {
            string urlAPI = UrlServiceWeb + "/commandes/" + id + "/statuts/";

            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                ObservableCollection<Statut> lesStatuts = JsonSerializer.Deserialize<ObservableCollection<Statut>>(contenu, optionJson);

                return lesStatuts;
            }
            else
            {
                throw new Exception("Erreur au chargement des statuts des commandes : " + resultatRequete.StatusCode);
            }
        }

        /// <summary>
        /// Modifie le seuil d'alerte d'un produit (album)
        /// </summary>
        /// <param name="id">id du produit à modifier</param>
        /// <param name="newSeuil">Nouveau seuil</param>
        /// <returns>Le nouveau seuil confirmé par l'API.</returns>
        public static async Task<int> EditSeuilProduit(int id,int newSeuil)
        {
            string urlAPI = UrlServiceWeb + "/seuil/";
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(id.ToString()), name: "idAlbum");
            form.Add(new StringContent(newSeuil.ToString()), name: "newSeuil");
            HttpResponseMessage reponse = await Contexte.httpClient.PostAsync(new Uri(urlAPI), form);
            if (reponse.IsSuccessStatusCode) {
                return int.Parse(await reponse.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(reponse.StatusCode.ToString());
            }
        }

        public static async Task<ObservableCollection<Event>> GetEvents()
        {
            string urlAPI = UrlServiceWeb + "/events/";

            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                ObservableCollection<Event> lesEvents = JsonSerializer.Deserialize<ObservableCollection<Event>>(contenu, optionJson);

                return lesEvents;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode);
            }
        }

        public static async Task<ObservableCollection<EditionEvent>> GetEditionsEvenement(string id)
        {
            string urlAPI = UrlServiceWeb + "/events/" + id + "/editions/";

            HttpResponseMessage resultatRequete = await Contexte.httpClient.GetAsync(new Uri(urlAPI));
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();

                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                ObservableCollection<EditionEvent> lesEditionsEvent = JsonSerializer.Deserialize<ObservableCollection<EditionEvent>>(contenu, optionJson);

                return lesEditionsEvent;
            }
            else
            {
                throw new Exception("Erreur au chargement des données : " + resultatRequete.StatusCode);
            }
        }

        public static async Task<Statut> UpdateCommande(int id,DateTime dateStatut)
        {
            string urlAPI = UrlServiceWeb + "/commandes/" + id + "/update/";
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(dateStatut.ToString("yyyy-MM-dd HH:mm:ss")), name: "dateStatut");
            HttpResponseMessage resultatRequete = await Contexte.httpClient.PostAsync(new Uri(urlAPI), form);
            if (resultatRequete.IsSuccessStatusCode)
            {
                string contenu = await resultatRequete.Content.ReadAsStringAsync();
                JsonSerializerOptions optionJson = new JsonSerializerOptions();
                optionJson.PropertyNameCaseInsensitive = true;
                Statut dernierStatut = JsonSerializer.Deserialize<Statut>(contenu, optionJson);
                return dernierStatut;
            }
            else { throw new Exception("Erreur lors de la mise à jour de la commande : " +  resultatRequete.StatusCode); }
        }
    }


}