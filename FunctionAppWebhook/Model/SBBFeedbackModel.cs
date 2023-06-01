using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionAppWebhook.Model
{
    public class WebhookEvent
    {
        public string webhookEvent { get; set; }
        public SBBFeedbackModel data { get; set; }
        public string eventCreated { get; set; }
    }
    public class SBBFeedbackModel
    {
        public object id { get; set; }
        public object ersteller { get; set; }
        public object endkunde { get; set; }

        public object status { get; set; }
        public object erstellDatum { get; set; }

        public versandort versandort { get; set; }
        public empfangsort empfangsort { get; set; }
        public string vertragsNummer { get; set; }
        public versandzeit versandzeit { get; set; }
        public empfangszeit empfangszeit { get; set; }
        public object absender { get; set; }
        public object empfaenger { get; set; }
        public object referenzAbsender { get; set; }
        public object vermerkEmpfaenger { get; set; }
        public string referenzKundensystem { get; set; }

        public string referenzAnbietersystem { get; set; } ///Newly added column

        public object bemerkung { get; set; }

        public object letzteGutart { get; set; }   ///Newly added column

        public object kundenReferenz { get; set; }   ///Newly added column
        public object transportplan { get; set; }   ///Newly added column

        public object mrnNummer { get; set; }

        public List<positionen> positionen { get; set; }



    }
}
