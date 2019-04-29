using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace hotelAdm
{
    class ConfigChangeException : ConfigurationErrorsException
    {
        public ConfigChangeException(string message)
            : base(message)
        { }
    }
}
