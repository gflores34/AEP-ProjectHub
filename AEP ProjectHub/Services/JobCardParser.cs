using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using AEP_ProjectHub.Models;

namespace AEP_ProjectHub.Services
{
	public class JobCardParser
	{
		public PackageMetadata Parse(string text)
		{
			PackageMetadata metadata = new();

			Match wrMatch =
				Regex.Match(
					text,
					@"9\d{7}");

			Match dcpMatch =
				Regex.Match(
					text,
					@"DCP\d{7}");

			Match jobMatch =
				Regex.Match(
					text,
					@"Type: (S[a-z,A-Z,0-9]*)");

			if (wrMatch.Success)
				metadata.WorkRequestNumber = wrMatch.Value;

			if (dcpMatch.Success)
				metadata.DcpNumber = dcpMatch.Value;

			if (jobMatch.Success)
				metadata.JobNumber = jobMatch.Groups[1].Value;

			return metadata;
		}
	}
}
