using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Models
{
    public sealed class Keys
    {
        public const string API = "hubwareAPI";
        public const string APIScope = "hubwareStoreScopeAPI";
        public const string APIScopeRead = "hubwareStoreScopeAPI.read";
        public const string APIScopeWrite = "hubwareStoreScopeAPI.write";
        public const string APIScopeRegister = "hubwareStoreScopeAPI.register";
        public const string APIScopeAdminReadWrite = "hubwareStoreScopeAPI.adminReadWrite";

        public const string APICoreScope = "hubwareStoreCoreScopeAPI";
        public const string APICoreScopeRead = "hubwareStoreCoreAPIScope.read";
        public const string APICoreScopeWrite = "hubwareStoreCoreAPIScope.write";

        public const string AccessDashboard = "Dashboard";
        public const string AccessPOS = "POS";

        public const string AppClient = "appClient";
        public const string RegisterClient = "registerClient";
        //public const string ForgotPasswordClient = "forgotPasswordClient";
        public const string hybridClient = "hybridClient";
        public const string spaCodeClient = "spaCodeClient";
        public const string spaCodeAdminClient = "spaCodeAdminClient";
        public const int RegisterExpiration = 60 * 10; // 10 minutes

        public const string ClientNamePOS = "Hubware Inventory POS";
        public const string ClientNameADMIN = "Hubware Inventory Dashboard";

        public const string ClaimUserName = "username";
        public const string ClaimIsEmailValidated = "isemailvalidated";
        public const string ClaimRole = "role";
        public const string ClaimFacebookId = "facebookid";
        public const string ClaimFacebookName = "facebookname";
        public const string ClaimFacebookFirstName = "facebookfirstname";
        public const string ClaimFacebookLastName = "facebooklastname";
        public const string ClaimFacebookEmail = "facebookemail";
        public const string ClaimFacebookPicture = "facebookpicture";
        public const string ClaimStoreId = "storeid";
        public const int DefaultWarningStocks = 20;

        //User Tokens
        public const string AccessToken = "AccessToken";
        public const string RefreshToken = "RefreshToken";
        public const string AccessTokenExpiration = "AccessTokenExpiration";
        public const string RefreshTokenExpiration = "RefreshTokenExpiration";

        //SignalR keys
        public const string Signal_Online = "Signal_Online";
        public const string Signal_POS_OrderQued = "Signal_POS_OrderQued";
        public const string Signal_POS_RefreshQued = "Signal_POS_OrderQued";
        public const string Signal_ADMIN_NewOrderForDelivery = "Signal_ADMIN_NewOrderForDelivery";
        public const string Signal_ADMIN_OrderStatusUpdated = "Signal_ADMIN_OrderStatusUpdated";
        public const string Signal_ADMIN_DashBoardUpdate = "Signal_ADMIN_DashBoardUpdate";
        public const string Signal_ADMIN_RefreshNavMenu = "Signal_ADMIN_RefreshNavMenu";

        public const string Signal_POS_Discount_ASK = "Signal_POS_Discount_ASK";
        public const string Signal_POS_Discount_APPROVED = "Signal_POS_Discount_APPROVED";
        public const string Signal_POS_Discount_DENIED = "Signal_POS_Discount_DENIED";


    }
}
