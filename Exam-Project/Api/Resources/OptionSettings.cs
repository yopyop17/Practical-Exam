using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Resources
{
    public class OptionSettings
    {
    }
    public class AuthSettings
    {
        public string IdentityServerUrl { get; set; }
        public string Authority { get; set; }
        public string ClientSecret { get; set; }
        public string ScopeSecret { get; set; }
        public string POS_RedirectUri { get; set; }
        public string POS_ClientURL { get; set; }
        public string ADMIN_RedirectUri { get; set; }
        public string ADMIN_ClientURL { get; set; }

        // core 
        public string CORE_RedirectUri { get; set; }
        public string CORE_ClientURL { get; set; }
        public string CORE_WEBSHOP_RedirectUri { get; set; }
        public string CORE_WEBSHOP_ClientURL { get; set; }

    }
}
