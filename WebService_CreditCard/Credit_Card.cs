using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService_CreditCard
{
    public class Credit_Card
    {
        public string Number { get; set; }
        public string Owner_ID { get; set; }
        public string Provider { get; set; }
        public string CVV { get; set; }
        public string DateOfExpiration { get; set; }
        public string Balance { get; set; }

        public Credit_Card (string Number, string Owner_ID, string Provider, string CVV, string DateOfExpiration, string Balance)
        {
            this.Number = Number;
            this.Owner_ID = Owner_ID;
            this.Provider = Provider;
            this.CVV = CVV;
            this.DateOfExpiration = DateOfExpiration;
            this.Balance = Balance;
        }
    }
}