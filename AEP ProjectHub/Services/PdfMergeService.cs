using System.Collections.Generic;
using AEP_ProjectHub.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace AEP_ProjectHub.Services
{
	public class PdfMergeService
	{
		public void MergePdfs(
			List<PackageDocument> documents,
			string outputFile)
		{
			using PdfDocument destination =
				new PdfDocument(
					new PdfWriter(outputFile));

			PdfMerger merger =
				new PdfMerger(destination);

			foreach (PackageDocument document in documents)
			{
				using PdfDocument source =
					new PdfDocument(
						new PdfReader(document.FilePath));

				merger.Merge(
					source,
					1,
					source.GetNumberOfPages());
			}
		}
	}
}