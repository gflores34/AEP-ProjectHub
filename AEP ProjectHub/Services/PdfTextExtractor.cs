using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UglyToad.PdfPig;

namespace AEP_ProjectHub.Services
{
	public class PdfTextExtractor
	{
		public string ExtractText(string pdfPath)
		{
			StringBuilder text = new();

			using (PdfDocument document = PdfDocument.Open(pdfPath))
			{
				foreach (var page in document.GetPages())
				{
					text.AppendLine(page.Text);
				}
			}
			
			return text.ToString();
		}
	}
}
