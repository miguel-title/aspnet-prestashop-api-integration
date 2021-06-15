using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtomicSeller
{
    public class PackLinkResponseModel
    {
        public class DVGetEnvoiResponse
        {
            public GetEnvoiData data { get; set; }
        }
        public class GetEnvoiData
        {
            public int id { get; set; }
            public int id_utilisateur { get; set; }
            public string date_creation { get; set; }
            public object date_impression { get; set; }
            public int id_support { get; set; }
            public string descriptif { get; set; }
            public string shipment_error { get; set; }
            public string documents_supports { get; set; }
            public GetEnvoiPli[] plis { get; set; }
        }

        public class DVPutEnvoiResponse
        {
            public PutEnvoiData data { get; set; }
        }

        public class PutEnvoiData
        {
            public int id { get; set; }
            public int id_utilisateur { get; set; }
            public string date_creation { get; set; }
            public object date_impression { get; set; }
            public int id_support { get; set; }
            public string descriptif { get; set; }
            public string shipment_error { get; set; }
            public string documents_supports { get; set; }
            public String[] plis { get; set; }
        }
        
        public class GetEnvoiPli
        {
            public string id { get; set; }
            public Support support { get; set; }
            public string date_depot { get; set; }
            public Createur createur { get; set; }
            public string numero { get; set; }
            public string reference { get; set; }
            public string reference_interne { get; set; }
            public string options { get; set; }
            public object date_retour_ar { get; set; }
            public Imputation imputation { get; set; }
            public Expediteur expediteur { get; set; }
            public Destinataire destinataire { get; set; }
            public Retour_Ar retour_ar { get; set; }
            public object documents_douaniers { get; set; }
            public Tracking tracking { get; set; }
            public Statut statut { get; set; }
            public Data1 data { get; set; }
        }
        
        public class Support
        {
            public int id { get; set; }
            public string nom { get; set; }
            public string nom_long { get; set; }
            public string code { get; set; }
            public string classname { get; set; }
        }

        public class Createur
        {
            public int id { get; set; }
            public string nom { get; set; }
            public string email { get; set; }
        }

        public class Imputation
        {
            public string libelle { get; set; }
            public int id_code_imputation { get; set; }
            public int id_utilisateur_imputation { get; set; }
        }

        public class Expediteur
        {
            public string raison_sociale { get; set; }
            public string nom { get; set; }
            public string complement_voie { get; set; }
            public string voie { get; set; }
            public string boite_postale { get; set; }
            public string code_postal_commune { get; set; }
            public string pays { get; set; }
        }

        public class Destinataire
        {
            public string raison_sociale { get; set; }
            public string nom { get; set; }
            public string complement_voie { get; set; }
            public string voie { get; set; }
            public string boite_postale { get; set; }
            public string code_postal_commune { get; set; }
            public string pays { get; set; }
        }

        public class Retour_Ar
        {
            public object raison_sociale { get; set; }
            public object nom { get; set; }
            public object complement_voie { get; set; }
            public object voie { get; set; }
            public object boite_postale { get; set; }
            public string code_postal_commune { get; set; }
            public string pays { get; set; }
        }

        public class Tracking
        {
            public string url { get; set; }
            public string api_key { get; set; }
        }

        public class Statut
        {
            public string id { get; set; }
            public string libelle { get; set; }
            public object date { get; set; }
        }

        public class Data1
        {
            public object[] emails_notifications_suivi { get; set; }
            public string email_destinataire { get; set; }
            public string reference { get; set; }
            public string envoiNotification { get; set; }
            public int poids { get; set; }
            public string preciser_notifications { get; set; }
        }

    }

    public class PackLinkErrorModel
    {

        public class PackLinkError
        {
            public Error error { get; set; }
        }

        public class Error
        {
            public int code { get; set; }
            public string message { get; set; }
            public string description { get; set; }
            public object details { get; set; }
        }

        public class Rootobject
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public string id { get; set; }
            public string[] plis { get; set; }
            public string documents_supports { get; set; }
            public object documents_douaniers { get; set; }
            public object factures { get; set; }
        }

    }

    public class PackLinkLabelModel
    {

        public class Rootobject
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public int id_utilisateur { get; set; }
            public int id_support { get; set; }
            public string descriptif { get; set; }
            public Pli[] plis { get; set; }
        }

        public class Pli
        {
            public string numero { get; set; }
            public string reference { get; set; }
            public string options { get; set; }
            public Imputation imputation { get; set; }
            public string poids { get; set; }
            public Expediteur expediteur { get; set; }
            public Destinataire destinataire { get; set; }
            public Retour_Ar retour_ar { get; set; }
            public Documents_Douaniers documents_douaniers { get; set; }
        }

        public class Imputation
        {
            public string libelle { get; set; }
            public int id_code_imputation { get; set; }
            public int id_utilisateur_imputation { get; set; }
        }

        public class Expediteur
        {
            public string raison_sociale { get; set; }
            public string nom { get; set; }
            public string complement_voie { get; set; }
            public string voie { get; set; }
            public string boite_postale { get; set; }
            public string code_postal_commune { get; set; }
            public string code_pays { get; set; }

        }

        public class Destinataire
        {
            public string raison_sociale { get; set; }
            public string nom { get; set; }
            public string complement_voie { get; set; }
            public string voie { get; set; }
            public string boite_postale { get; set; }
            public string code_postal_commune { get; set; }
            public string code_pays { get; set; }
        }

        public class Retour_Ar
        {
            public object raison_sociale { get; set; }
            public object nom { get; set; }
            public object complement_voie { get; set; }
            public object voie { get; set; }
            public object boite_postale { get; set; }
            public string code_postal_commune { get; set; }
            public string pays { get; set; }
        }

        public class Documents_Douaniers
        {
            public int envoi_commercial { get; set; }
            public int[] envoi_nature { get; set; }
            public string observation { get; set; }
            public string num_licence { get; set; }
            public string num_certificat { get; set; }
            public string num_facture { get; set; }
            public string frais_port { get; set; }
            public int quantite_total { get; set; }
            public string poids_total { get; set; }
            public float valeur_dts { get; set; }
            public string valeur_total { get; set; }
            public bool facture_fichier { get; set; }
            public Article[] articles { get; set; }
        }

        public class Article
        {
            public object code_article { get; set; }
            public string description_detaillee { get; set; }
            public string quantite { get; set; }
            public string valeur { get; set; }
            public string poids { get; set; }
            public string pays_origine { get; set; }
            public string num_tarifaire { get; set; }
        }

    }


    public class PackLinkShippingRatesModel
    {

        public class Rootobject
        {
            public int service_id { get; set; }
            public string source { get; set; }
            public from from{ get; set; }
            public to to { get; set; }
            public package[] packages { get; set; }
        }

        public class from
        {
            public string zip { get; set; }
            public string country { get; set; }

        }
        public class to
        {
            public string zip { get; set; }
            public string country { get; set; }
        }

        public class package
        {
            public float width { get; set; }
            public float height { get; set; }
            public float length { get; set; }
            public float weight { get; set; }

        }
    }
}