using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti
{
    public class HttpUtility
    {
        public static string BuildPostData(NameValueCollection data)
        {
            string adder = string.Empty;

            StringBuilder builder = new StringBuilder();

            foreach (string key in data.AllKeys)
            {
                builder.Append(adder);
                builder.Append(Uri.EscapeDataString(key));
                builder.Append("=");
                builder.Append(Uri.EscapeDataString(data[key]));
                adder = "&";
            }

            return builder.ToString();
        }
    }
}
