﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripePaymentGateway_DotNet5.SeedData
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}
