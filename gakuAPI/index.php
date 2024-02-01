<?php

/*
                                TODO :
                                 - Gérer le changement de statut d'une commande (page détail produit)

                                */

header("Cache-Control: no-cache, must-revalidate");
header("pragma: no-cache");


try {
        $pdo_options[PDO::ATTR_ERRMODE] = PDO::ERRMODE_EXCEPTION;
        $bdd = new PDO('mysql:host=127.0.0.1;dbname=gakuDB', 'gaku_view', 'stopl0okingatp4sswd!');
        $bdd->query("SET NAMES utf8");
        if ($_SERVER['REQUEST_METHOD'] == 'GET') {
                $fetchAll = true;
                switch ($_GET['data']) {
                        case 'commandes':
                                //y a une faille de cyber-sécurité ici, mais est-ce vraiment un problème dans un projet scolaire ?
                                if (isset($_GET['contenu']) && $_GET['contenu'] == true) {
                                        $query = "SELECT album.nom,idAlbum,COALESCE(artiste.nom, label.nom) AS nomCreateur,uriImage,Commander.qte
                                        FROM Commander 
                                        JOIN Album ON Album.id = Commander.idAlbum
                                        LEFT JOIN artiste ON artiste.id = album.idArtiste
                                        LEFT JOIN label ON label.id = album.idLabel
                                        WHERE idCommande = :id";
                                        $req = $bdd->prepare($query);
                                        $req->bindParam("id", $_GET['idcommande'], PDO::PARAM_INT);
                                } else if (isset($_GET['statuts']) && $_GET['statuts'] == true) {
                                        $query = "SELECT idStatut, dateStatut FROM Avancer WHERE idCommande = :id";
                                        $req = $bdd->prepare($query);
                                        $req->bindParam("id", $_GET['idcommande'], PDO::PARAM_INT);
                                } else {
                                        $query = "SELECT id, dateHeure, adresseLivraison, prenomDestinataire,
                                        nomDestinataire, cpLivraison, villeLivraison, numeroTel,idUtilisateur, note,
                                        max(avancer.idStatut) as idStatutActuel, max(avancer.dateStatut) as dateDernierStatut
                                        FROM Commande
                                        LEFT JOIN AVANCER ON AVANCER.idCommande = Commande.id
                                        GROUP BY commande.id";
                                        if (isset($_GET['idcommande']) && !empty($_GET['idcommande'])) {
                                                $query .= " WHERE id = :id";
                                                $req = $bdd->prepare($query);
                                                $req->bindParam("id", $_GET['idcommande'], PDO::PARAM_INT);
                                        } else {
                                                $req = $bdd->prepare($query);
                                        }
                                }
                                $req->execute();
                                $results = $req->fetchAll(PDO::FETCH_ASSOC);
                                print(json_encode($results));
                                break;
                        case 'albums':
                                if (isset($_GET['commandes']) && $_GET['commandes'] == true) {
                                        $query = 'SELECT commande.id as id, nomDestinataire,prenomDestinataire,dateHeure,
                                        max(avancer.idStatut) as idStatutActuel
                                        FROM Commande
                                        JOIN COMMANDER ON Commander.idCommande = commande.id
                                        LEFT JOIN AVANCER ON AVANCER.idCommande = Commande.id
                                        where Commander.idAlbum = :id
                                        GROUP BY commande.id';
                                        $req = $bdd->prepare($query);
                                        $req->bindParam("id", $_GET['idalbum'], PDO::PARAM_INT);
                                } else {
                                        $query = 'SELECT album.id, album.nom, COALESCE(artiste.nom, label.nom) AS nomCreateur, description, uriImage, 
                                        COALESCE(album.idLabel, album.idArtiste) AS idCreateur, prix, qte, alerteSeuil,idEvent,numEdition,
                                        IFNULL((SELECT SUM(qte) FROM Commander WHERE idAlbum = album.id GROUP BY idAlbum),0) as qteCommande
                                        FROM
                                            album
                                        JOIN Provenir ON Provenir.idAlbum = album.id
                                        LEFT JOIN artiste ON artiste.id = album.idArtiste
                                        LEFT JOIN label ON label.id = album.idLabel
                                         ';
                                        if (isset($_GET['idalbum']) && !empty($_GET['idalbum'])) {
                                                $query .= 'WHERE album.id = :id';
                                                $req = $bdd->prepare($query);
                                                $req->bindParam("id", $_GET['idalbum'], PDO::PARAM_INT);
                                        } else {
                                                $req = $bdd->prepare($query);
                                        }
                                }
                                $req->execute();
                                $results = $req->fetchAll(PDO::FETCH_ASSOC);

                                print(json_encode($results));
                                break;
                        case 'utilisateur':
                                if (isset($_GET['idutilisateur']) && !empty($_GET['idutilisateur'])) {
                                        $query = 'SELECT id,prenom,nom,mail FROM utilisateur where id = :id';
                                        $req = $bdd->prepare($query);
                                        $req->bindParam("id", $_GET['idutilisateur'], PDO::PARAM_INT);
                                        $req->execute();
                                        $results = $req->fetch(PDO::FETCH_ASSOC);
                                        print(json_encode($results));
                                } else {
                                        print("aucun ID n'a été spécifié.");
                                }
                                break;
                        case 'events':
                                if (isset($_GET['editions']) && $_GET['editions'] == true) {
                                        $query = "SELECT idEvent,numEdition FROM Edition_Evenement where idEvent = :id";
                                        $req = $bdd->prepare($query);
                                        $req->bindParam("id", $_GET['idEvent'], PDO::PARAM_STR);
                                } else {
                                        $query = "SELECT id,nom FROM Evenement ORDER BY 1 DESC";
                                        $req = $bdd->prepare($query);
                                }
                                $req->execute();
                                $results = $req->fetchAll(PDO::FETCH_ASSOC);
                                print(json_encode($results));
                                break;
                }
        } else if ($_SERVER['REQUEST_METHOD'] == 'POST') {
                switch ($_GET['action']) {
                        case 'seuil':
                                if (isset($_POST['idAlbum']) && is_numeric($_POST['idAlbum']) && isset($_POST['newSeuil']) && is_numeric($_POST['newSeuil'])) {
                                        //Pourquoi on fait une transaction ? bonne question jsp
                                        $bdd->beginTransaction();
                                        $query = "UPDATE album SET alerteSeuil = :newSeuil WHERE id = :id";
                                        $req = $bdd->prepare($query);
                                        $req->bindParam("id", $_POST["idAlbum"]);
                                        $req->bindParam("newSeuil", $_POST["newSeuil"]);
                                        $req->execute();
                                        $bdd->commit();
                                        print($_POST['newSeuil']);
                                } else {
                                        http_response_code(400);
                                        print("Oops : Il semblerait que vous ayez oublié quelque chose...");
                                }
                        case 'updateCommande':
                                $bdd->beginTransaction();
                                $query = "INSERT INTO Avancer(idCommande,idStatut,dateStatut) VALUES (:idCommande,0,:dateStatut)";
                                $req = $bdd->prepare($query);
                                $req->bindParam("idCommande", $_GET["idcommande"]);
                                $req->bindParam("dateStatut", $_POST["dateStatut"]);
                                $req->execute();
                                $query = "SELECT max(idStatut) as idStatut,max(dateStatut) as dateStatut FROM Avancer where idCommande  = :id";
                                $req = $bdd->prepare($query);
                                $req->bindParam("id", $_GET["idcommande"]);
                                $req->execute();
                                $result = $req->fetch(PDO::FETCH_ASSOC);
                                print(json_encode($result));
                                $bdd->commit();
                }
        }
} catch (Exception $ex) {
        http_response_code(400);
        //A ENLEVER EN PRODUCTION!
        print($ex->getMessage());
}
