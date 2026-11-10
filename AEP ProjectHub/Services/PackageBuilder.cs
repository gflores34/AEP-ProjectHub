using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEP_ProjectHub.Models;

namespace AEP_ProjectHub.Services
{
	public class PackageBuilder
	{
		public void CreatePacket(
			string outputFolder,
			List<PackageDocument> documents)
		{
			Directory.CreateDirectory(outputFolder);

			foreach (var document in documents)
			{
				string destinationFile =
					Path.Combine(
						outputFolder,
						document.FileName);

				File.Copy(
					document.FilePath,
					destinationFile,
					true);
			}
		}
	}
}
