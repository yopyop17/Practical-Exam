using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Constants
{
    public class Constant
    {
        public const int MinimumPasswordLength = 6;
        public const int PageSize = 20;
        public const int PageIndex = 1;
        public const string Default_ProfileImage_Male = "/Content/Images/img_profile_default_male@3x.png";
        public const string Default_ProfileImage_Female = "/Content/Images/img_profile_default_male@3x.png";

        public const string DefaultPlaceHolder = "/Content/Images/placeholder.jpg";
        public const string TempUpload = @"C:\hw-temp\";
        public const string TempDateDirectoryFormat = "yyyyMMdd";

        public const string PesoSign = "₱";

        public const double GeoKilometerBaseRadius = 6378.1370D;

        public const string LocalDisplayTimeDate = "dddd, dd MMMM yyyy - hh:mm:ss tt";

        public const string CachedProductCity = "products-on-{0}";

        public const string ng_json_callback = "ng_jsonp_callback_0 && ng_jsonp_callback_0([{0}])";

        public const string ProfilePictureFormat = "https://hubware.s3-ap-southeast-1.amazonaws.com/dev/v1.0/Users/{0}_15.png";
        public const string CatalogPictureFormat = "https://hubware.s3-ap-southeast-1.amazonaws.com/dev/v1.0/Catalog/{0}_15.png";
        public const string CategoryPictureFormat = "https://hubware.s3-ap-southeast-1.amazonaws.com/dev/v1.0/Category/{0}_15.png";
        public const string BrandsPictureFormat = "https://hubware.s3-ap-southeast-1.amazonaws.com/dev/v1.0/Brands/{0}_15.png";

        public const string ImagePlaceHolder = "https://hubware.com.ph/Content/img/placeholder.jpg";
    }
}
