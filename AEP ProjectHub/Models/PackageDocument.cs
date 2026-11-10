using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEP_ProjectHub.Models
{
	public class PackageDocument
	{
		public string FileName { get; set; } = "";

		public string FilePath { get; set; } = "";

		public bool IsStandardDocument { get; set; }

		public bool IncludeInConstruction { get; set; } = true;

		public bool IncludeInClearing { get; set; } = true;

		public bool IncludeInStores { get; set; } = true;
	}
}
