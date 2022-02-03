using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Commands
{
    public class BaseCommand
	{
		private int _pagesize = 20;
		private int _page = 1;

		[JsonProperty("ps")]
		public int PageSize { get { return _pagesize; } set { this._pagesize = value; } }
		[JsonProperty("pg")]
		public int Page { get { return _page; } set { this._page = value; } }

		public int SkipIndex { get { return (Page - 1) * PageSize; } }

		public long? TotalCount { get; set; }

		public long TotalPage { get { return ((TotalCount ?? PageSize) / PageSize) + 1; } }

		public string callback { get; set; }
		public bool isNG_JSON_CALLBACK { get { return callback?.Contains("ng_jsonp_callback") ?? false; } }

	}
}
