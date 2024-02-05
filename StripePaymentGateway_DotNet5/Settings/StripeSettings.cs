using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripePaymentGateway_DotNet5.Settings
{
    public class StripeSettings
    {
        public string SecretKey { set; get; }
        public string PublishableKey { set; get; }
    }
}
