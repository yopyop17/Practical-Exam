using Exam_Project.Api.Entities.Base;
using Exam_Project.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Entities
{
    public class Sessions : BaseEntity
    {
		[Key]
		public long Id { get; set; }
		#region Foreign Keys
		public long UserId { get; set; }
		#endregion
		public DateTime? LoggedOutDate { get; set; }
		[Required, MaxLength(100)]
		public string Token { get; set; }
		[MaxLength(250)]
		public string DeviceToken { get; set; }
		public bool DeviceTokenExpired { get; set; }
		public bool IsPushNotificationEnabled { get; set; }
		public bool NeedsChangePassword { get; set; }

		#region Persisted Grant Data
		public DateTime? Expiration { get; set; }
		[MaxLength(100)]
		public string ClientId { get; set; }
		public string GrantData { get; set; }
		[MaxLength(100)]
		public string GrantType { get; set; }
		#endregion


		[MaxLength(250)]
		public string UserAgent { get; set; }
		[MaxLength(45)]
		public string RequestIdentifier { get; set; }

		[MaxLength(50)]
		public string Version { get; set; }
		[MaxLength(30)]
		public string OSVersion { get; set; }
		[MaxLength(45)]
		public string Model { get; set; }
		[MaxLength(45)]
		public string Build { get; set; }
		[MaxLength(50)]
		public string Manufacturer { get; set; }


		#region Navigation Properties
		public User User { get; set; }
		#endregion
	}
}
