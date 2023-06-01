using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionAppWebhook.Model
{
    public class PostSbbModel
    {
        public versandort Versandort { get; set; }
        public empfangsort empfangsort { get; set; }
        public string VertragsNummer { get; set; }
        public versandzeit versandzeit { get; set; }
        public empfangszeit empfangszeit { get; set; }
        public object absender { get; set; }
        public object empfaenger { get; set; }
        public object referenzAbsender { get; set; }
        public object vermerkEmpfaenger { get; set; }
        public string referenzKundensystem { get; set; }
        public string bemerkung { get; set; }
        public mrnNummer mrnNummer { get; set; }
        public List<positionen> positionen { get; set; }
    }

    public class mrnNummer
    {
        public string code { get; set; }
        public string referenz { get; set; }
    }

    public class empfangsort
    {
        public string uicLand { get; set; }
        public string uicNummer { get; set; }
        public string ladestelle { get; set; }
    }

    public class empfangszeit
    {
        public string gewuenscht { get; set; }
        public string geplant { get; set; }
        public string effektiv { get; set; }
    }

    public class GefahrgutDaten
    {
        public string spracheDeklaration { get; set; }
        public string offizielleBezeichnung { get; set; }
        public string technischeBenennung { get; set; }
        public object sondervorschrift { get; set; }
        public string vorschriftVersion { get; set; }
        public string unNummer { get; set; }
        public string gefahrKlasse { get; set; }
        public string gefahrNummer { get; set; }
        public string verpackungsgruppe { get; set; }
        public object klassifizierungCode { get; set; }
        public string gefahrZettel1 { get; set; }
        public object gefahrZettel2 { get; set; }
        public object gefahrZettel3 { get; set; }
        public object gefahrZettel4 { get; set; }
        public bool umweltgefaehrdend { get; set; }
    }

    public class Gutart
    {
        public string nhm { get; set; }
        public string kundenbezeichnung { get; set; }
        public GefahrgutDaten gefahrgutDaten { get; set; }
        public object mrnNummer { get; set; }
    }
    public class Ladeeinheiten
    {
        public object ladeeinheiten { get; set; }
    }
    public class positionen
    {
        public int laufnummer { get; set; }
        public object wagennummer { get; set; }
        public string wagentyp { get; set; }
        public int? gewichtNetto { get; set; }

        public List<Ladeeinheiten> ladeeinheiten { get; set; }
        public Gutart gutart { get; set; }
        public Gutart LetzteGutart { get; set; }
        public object KundenReferenz { get; set; }
        //public List<object> Ladeeinheiten { get; set; }
        public object mrnNummer { get; set; }
        public List<Transportplan> transportplan { get; set; }
    }

    public class versandort
    {
        public string uicLand { get; set; }
        public string uicNummer { get; set; }
        public string ladestelle { get; set; }
    }

    public class versandzeit
    {
        public string gewuenscht { get; set; }
        public string geplant { get; set; }
        public string effektiv { get; set; }

    }
}
