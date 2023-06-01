using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionAppWebhook.Model
{
    public class PutEWtoSBB
    {
        public int id { get; set; }
        public string ersteller { get; set; }
        public string endkunde { get; set; }
        public string status { get; set; }
        public DateTime erstellDatum { get; set; }
        public versandort versandort { get; set; }
        public empfangsort empfangsort { get; set; }
        public string vertragsNummer { get; set; }
        public versandzeit versandzeit { get; set; }
        public empfangszeit empfangszeit { get; set; }
        public string absender { get; set; }
        public string empfaenger { get; set; }
        public string referenzKundensystem { get; set; }
        public int referenzAnbietersystem { get; set; }
        public MrnNummer mrnNummer { get; set; }
        public List<positionen> positionen { get; set; }
    }
}

public class MrnNummer
{
    public string code { get; set; }
    public string referenz { get; set; }
}

//public class Positionen
//{
//    public int laufnummer { get; set; }
//    public string Wagentyp { get; set; }
//    public string Wagennummer { get; set; }
//    public int GewichtNetto { get; set; }
//    public Gutart gutart { get; set; }
//    public Versandzeit versandzeit { get; set; }
//    public Empfangszeit empfangszeit { get; set; }
//    public List<object> ladeeinheiten { get; set; }

//}



public class Start
{
    public string uicLand { get; set; }
    public string uicNummer { get; set; }
    public string ladestelle { get; set; }
}

public class Transportplan
{
    public Start start { get; set; }
    public Ziel ziel { get; set; }
    public string zugNummer { get; set; }
}

public class Ziel
{
    public string uicLand { get; set; }
    public string uicNummer { get; set; }
    public string ladestelle { get; set; }
}
