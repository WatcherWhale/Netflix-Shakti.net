using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti.Json
{
    public class JsonUtilities
    {
        public static string RemoveProperties(string json,string prop)
        {
            string newJson = json;
            while (newJson.Contains(prop))
            {
                int start = newJson.IndexOf($"\"{prop}\":");
                int stop1 = newJson.IndexOf('}', start);
                int stop2 = newJson.IndexOf(',', start + 1);

                int stop = 0;
                if (stop1 > stop2)
                {
                    stop = stop2 + 1;
                }
                else
                {
                    stop = stop1;
                }

                newJson = newJson.Substring(0, start) + newJson.Substring(stop);
            }

            return newJson;
        }
    }
}
