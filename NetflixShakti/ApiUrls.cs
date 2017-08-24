using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti
{
    static class ApiUrls
    {
        public const string baseAPIUrl = "https://www.netflix.com/api/shakti/";
        public const string GetLists = "/preflight?batchImages=true&lolomoid={LOLOMOID}_ROOT&fromRow=4&toRow=50&opaqueImageExtension=webp&transparentImageExtension=webp";
    }
}
