using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Exam_Project.Api.Models
{
    public class ClaimsPrincipalUser : ClaimsPrincipal
    {
        const string NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public ClaimsUser User { get; private set; }

        public ClaimsPrincipalUser(ClaimsPrincipal user) : base(user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var id = user.Claims.Where(w => w.Type == "sub").FirstOrDefault()?.Value;
            var name = user.Claims.Where(w => w.Type == "name").FirstOrDefault()?.Value;
            int.TryParse(user.Claims.Where(w => w.Type == Keys.ClaimStoreId).FirstOrDefault()?.Value, out var storeId);
            var userid = id ?? user.Claims.FirstOrDefault(c => c.Type == NameClaimType)?.Value;
            var username = name ?? user.Claims.FirstOrDefault(c => c.Type == Keys.ClaimUserName)?.Value;
            bool.TryParse(user.Claims.FirstOrDefault(c => c.Type == Keys.ClaimIsEmailValidated)?.Value, out var isEmailValidated);

            User = new ClaimsUser
            {
                Id = Convert.ToInt64(userid),
                UserName = username,
                StoreId = storeId,
                IsEmailValidated = isEmailValidated
            };

        }

        public ClaimsPrincipalUser(ClaimsUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public ClaimsPrincipalUser(IPrincipal principal, ClaimsUser user) : base(principal)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }

    public class ClaimsUser
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string AccessToken { get; set; }

        public bool IsEmailValidated { get; set; }

        public long StoreId { get; set; }
    }
}
