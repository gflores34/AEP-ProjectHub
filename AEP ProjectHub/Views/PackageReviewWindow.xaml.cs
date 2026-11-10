using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AEP_ProjectHub.Models;
using AEP_ProjectHub.Services;

namespace AEP_ProjectHub.Views
{
	public partial class PackageReviewWindow : Window
	{
		private readonly ObservableCollection<PackageDocument> _documents;

		private string District =>
			txtDistrict.Text.Trim();
		private readonly string _sourceFolder;

		public PackageReviewWindow(
			string wr,
			string job,
			string dcp,
			string sourceFolder,
			List<PackageDocument> documents)
		{
			InitializeComponent();

			_sourceFolder = sourceFolder;

			_documents =
				new ObservableCollection<PackageDocument>(
					documents
						.OrderBy(x => x.FileName)
						.ToList());

			foreach (var document in _documents)
			{
				ApplyPacketDefaults(document);
			}


			lblWR.Text = wr;
			lblJob.Text = job;
			lblDcp.Text = dcp;
			txtDistrict.Text = string.Empty;


			LoadStandardDocuments();

			SortDocuments();

			lstDocuments.ItemsSource = _documents;
		}

		private string GetProjectFolder()
		{
			return Path.Combine(
				_sourceFolder,
				$"{District.PadLeft(4, '0')}-000{lblWR.Text}-01");
		}

		private string GetStormsFolder()
		{
			return Path.Combine(
				@"Y:\2026",
				District.PadLeft(4, '0'));
		}

		private void LoadStandardDocuments()
		{
			string folder = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory,
				"Data",
				"StandardDocuments");

			if (!Directory.Exists(folder))
				return;

			foreach (string file in Directory.GetFiles(folder, "*.pdf"))
			{
				if (_documents.Any(x =>
					x.FileName.Equals(
						Path.GetFileName(file),
						StringComparison.OrdinalIgnoreCase)))
				{
					continue;
				}

				var document =
				new PackageDocument
				{
					FileName = Path.GetFileName(file),
					FilePath = file,
					IsStandardDocument = true
				};

				ApplyPacketDefaults(document);

				_documents.Add(document);
			}
		}

		private bool ValidatePacket()
		{
			if (string.IsNullOrWhiteSpace(lblWR.Text))
			{
				MessageBox.Show(
					"WR Number is required.",
					"Validation",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);

				return false;
			}

			if (string.IsNullOrWhiteSpace(District))
			{
				MessageBox.Show(
					"District Number is required.",
					"Validation",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);

				return false;
			}

			if (_documents.Count == 0)
			{
				MessageBox.Show(
					"No PDFs selected.",
					"Validation",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);

				return false;
			}

			return true;
		}

		private void BuildPacket(
			string fileName,
			List<PackageDocument> documents)
		{
			if (!documents.Any())
				return;

			Directory.CreateDirectory(
				GetProjectFolder());

			string outputFile =
				Path.Combine(
					GetProjectFolder(),
					$"{fileName}.pdf");

			PdfMergeService merger = new();

			merger.MergePdfs(
				documents,
				outputFile);
		}

		private void BuildAll_Click(
			object sender,
			RoutedEventArgs e)
		{
			if (!ValidatePacket())
				return;

			try
			{
				BuildPacket(
					$"CC {lblDcp.Text} WR{lblWR.Text} Construction Packet",
					_documents
						.Where(x => x.IncludeInConstruction)
						.ToList());

				BuildPacket(
					$"WR{lblWR.Text} Clearing Packet",
					_documents
						.Where(x => x.IncludeInClearing)
						.ToList());

				BuildPacket(
					$"WR{lblWR.Text} Stores Packet",
					_documents
						.Where(x => x.IncludeInStores)
						.ToList());

				MessageBox.Show(
					"All packets created successfully.",
					"ProjectHub",
					MessageBoxButton.OK,
					MessageBoxImage.Information);

				if (chkPushToStorms.IsChecked == true)
				{
					string stormsFolder =
						GetStormsFolder();

					if (!Directory.Exists(stormsFolder))
					{
						MessageBox.Show(
							$"Storms folder not found:\n{stormsFolder}",
							"Storms",
							MessageBoxButton.OK,
							MessageBoxImage.Warning);

						return;
					}

					PushToStorms();
				}

				MessageBox.Show(
					"All packets created successfully.",
					"ProjectHub",
					MessageBoxButton.OK,
					MessageBoxImage.Information);

				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					ex.Message,
					"Packet Build Failed",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}

		private void MoveUp_Click(
			object sender,
			RoutedEventArgs e)
		{
			if (sender is not Button button)
				return;

			if (button.Tag is not PackageDocument document)
				return;

			int index =
				_documents.IndexOf(document);

			if (index <= 0)
				return;

			_documents.Move(
				index,
				index - 1);

			lstDocuments.SelectedItem =
				document;

			lstDocuments.ScrollIntoView(
				document);
		}

		private void MoveDown_Click(
			object sender,
			RoutedEventArgs e)
		{
			if (sender is not Button button)
				return;

			if (button.Tag is not PackageDocument document)
				return;

			int index =
				_documents.IndexOf(document);

			if (index >= _documents.Count - 1)
				return;

			_documents.Move(
				index,
				index + 1);

			lstDocuments.SelectedItem =
				document;

			lstDocuments.ScrollIntoView(
				document);
		}

		private void DeleteDocument_Click(
			object sender,
			RoutedEventArgs e)
		{
			if (sender is not Button button)
				return;

			if (button.Tag is not PackageDocument document)
				return;

			MessageBoxResult result =
				MessageBox.Show(
					$"Remove '{document.FileName}' from this package?",
					"Remove Document",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				_documents.Remove(document);
			}
		}

		private void Cancel_Click(
			object sender,
			RoutedEventArgs e)
		{
			Close();
		}

		private void ApplyPacketDefaults(
			PackageDocument document)
		{
			string fileName =
				document.FileName.ToUpperInvariant();

			document.IncludeInConstruction = false;
			document.IncludeInClearing = false;
			document.IncludeInStores = false;

			string prefix = string.Empty;

			if (fileName.Length >= 2)
			{
				prefix = fileName.Substring(0, 2);
			}

			switch (prefix)
			{
				case "01":
					document.IncludeInConstruction = true;
					document.IncludeInClearing = true;
					document.IncludeInStores = true;
					break;

				case "02":
					document.IncludeInConstruction = true;
					break;

				case "03":
					document.IncludeInConstruction = true;
					break;

				case "04":
					document.IncludeInConstruction = true;
					break;

				case "05":
					document.IncludeInConstruction = true;
					break;

				case "06":
					document.IncludeInConstruction = true;
					document.IncludeInStores = true;
					break;

				case "07":
					document.IncludeInConstruction = true;
					break;

				case "08":
					document.IncludeInConstruction = true;
					break;

				case "09":
					document.IncludeInConstruction = true;
					break;

				case "10":
					document.IncludeInConstruction = true;
					break;

				case "C2":
					document.IncludeInClearing = true;
					break;

				case "C3":
					document.IncludeInClearing = true;
					break;
			}
		}

		private void SortDocuments()
		{
			var sortedDocuments =
				_documents
					.OrderBy(x => x.FileName)
					.ToList();

			_documents.Clear();

			foreach (var document in sortedDocuments)
			{
				_documents.Add(document);
			}
		}

		private static void CopyDirectory(
			string sourceDirectory,
			string destinationDirectory)
		{
			Directory.CreateDirectory(
				destinationDirectory);

			foreach (string file in Directory.GetFiles(sourceDirectory))
			{
				string destinationFile =
					Path.Combine(
						destinationDirectory,
						Path.GetFileName(file));

				File.Copy(
					file,
					destinationFile,
					true);
			}

			foreach (string directory in Directory.GetDirectories(sourceDirectory))
			{
				string destinationSubdirectory =
					Path.Combine(
						destinationDirectory,
						Path.GetFileName(directory));

				CopyDirectory(
					directory,
					destinationSubdirectory);
			}
		}

		private void PushToStorms()
		{
			string stormsFolder =
				GetStormsFolder();

			if (!Directory.Exists(stormsFolder))
			{
				MessageBox.Show(
					$"District folder not found:\n{stormsFolder}",
					"Storms",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);

				return;
			}

			string packetFolder =
				GetProjectFolder();

			string destinationFolder =
				Path.Combine(
					stormsFolder,
					Path.GetFileName(packetFolder));

			if (Directory.Exists(destinationFolder))
			{
				MessageBox.Show(
					$"Packet folder already exists in Storms:\n{destinationFolder}",
					"Storms",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);

				return;
			}

			CopyDirectory(
				packetFolder,
				destinationFolder);

			MessageBox.Show(
				"Packet folder successfully copied to Storms.",
				"Storms",
				MessageBoxButton.OK,
				MessageBoxImage.Information);
		}

	}
}