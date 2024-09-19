using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace UnpaidpostpaidBills
{
    public class FacturationEncaissement
    {
        //[Required(ErrorMessage = "La date est requise.")]
        //[DataType(DataType.Date)]
        public DateTime DATE { get; set; }

        //[Required(ErrorMessage = "Le numéro de facture est requis.")]
        public string? NUM_FACT { get; set; }

        //[StringLength(10, MinimumLength = 1, ErrorMessage = "La longueur du cycle doit être entre 1 et 10 caractères.")]
        public string? CYCLE { get; set; }

        [Required(ErrorMessage = "Le Code du client est requis.")]
        public string? CLIENT { get; set; }

        //[Required(ErrorMessage = "L'intitulé du client est requis.")]
        public string? INTITULE_CLIENT { get; set; }

        //[Range(2000, int.MaxValue, ErrorMessage = "L'année doit être supérieure à 2000.")]
        public int ANNEE { get; set; }

        //[Range(1, 12, ErrorMessage = "La période doit être comprise entre 1 et 12.")]
        public int PERIODE { get; set; }

        //[Range(0, long.MaxValue, ErrorMessage = "Le montant de la facture doit être positif.")]
        public long MONTANT_FACT { get; set; }

        //[Range(0, long.MaxValue, ErrorMessage = "Le montant réglé doit être positif.")]
        public long MONTANT_REGLE { get; set; }

        //[Range(0, long.MaxValue, ErrorMessage = "Le solde doit être positif.")]
        public long SOLDE { get; set; }
        // Ajoutez d'autres propriétés selon votre vue SQL Server
    }
}
