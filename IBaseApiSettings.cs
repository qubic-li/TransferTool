using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace li.qubic.transfertool
{
    public interface IBaseApiSettings
    {
        public string BaseUrl { get; set; }
        public string AccessToken { get; set; }
        public int ApiRequestTimeout { get; set; }
    }
}
