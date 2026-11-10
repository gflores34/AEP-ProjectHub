using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AEP_ProjectHub.Models;
using AEP_ProjectHub.Services;
using Microsoft.Win32;

namespace AEP_ProjectHub.Views
{
	public partial class PackageManagerPage : Page
	{
		private readonly List<PackageDocument> _selectedFiles = new();

		private string _sourceFolder = string.Empty;

		public PackageManagerPage()
		{
			InitializeComponent();
		}

		private void btnAddFiles_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
				Filter = "PDF Files (*.pdf)|*.pdf",
				Multiselect = true
			};

			if (dialog.ShowDialog() == true)
			{
				foreach (string file in dialog.FileNames)
				{
					if (!_selectedFiles.Any(x => x.FilePath == file))
					{
						if (string.IsNullOrWhiteSpace(_sourceFolder))
						{
							_sourceFolder =
								Path.GetDirectoryName(file)!;
						}

						_selectedFiles.Add(new PackageDocument
						{
							FileName = Path.GetFileName(file),
							FilePath = file
						});

						lstFiles.Items.Add(Path.GetFileName(file));

						ProcessFile(file);
					}
				}
			}
		}

		private void btnBuildPackage_Click(object sender, RoutedEventArgs e)
		{
			PackageReviewWindow reviewWindow =
				new PackageReviewWindow(
					txtWrNumber.Text,
					txtJobNumber.Text,
					txtDcpNumber.Text,
					_sourceFolder,
					_selectedFiles.ToList());

			reviewWindow.Owner =
				Window.GetWindow(this);

			reviewWindow.ShowDialog();
		}

		private void DropArea_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effects = DragDropEffects.Copy;
			}
			else
			{
				e.Effects = DragDropEffects.None;
			}
		}

		private void DropArea_Drop(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop))
				return;

			string[] files =
				(string[])e.Data.GetData(DataFormats.FileDrop);

			foreach (string file in files)
			{
				if (!file.EndsWith(".pdf",
					StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				if (!_selectedFiles.Any(x => x.FilePath == file))
				{
					if (string.IsNullOrWhiteSpace(_sourceFolder))
					{
						_sourceFolder =
							Path.GetDirectoryName(file)!;
					}

					_selectedFiles.Add(new PackageDocument
					{
						FileName = Path.GetFileName(file),
						FilePath = file
					});

					lstFiles.Items.Add(Path.GetFileName(file));

					ProcessFile(file);
				}
			}
		}

		private void ProcessFile(string filePath)
		{
			string fileName = Path.GetFileName(filePath);

			if (fileName.Contains(
				"04 Job Card",
				StringComparison.OrdinalIgnoreCase))
			{
				ParseJobCard(filePath);
			}
		}

		private void ParseJobCard(string filePath)
		{
			MessageBox.Show(
				"Job Card detected\nAttempting to extract WR, Job Type, and WO");

			PdfTextExtractor extractor = new();
			JobCardParser parser = new();

			string pdfText =
				extractor.ExtractText(filePath);

			var metadata =
				parser.Parse(pdfText);

			txtWrNumber.Text =
				metadata.WorkRequestNumber;

			txtJobNumber.Text =
				metadata.JobNumber;

			txtDcpNumber.Text =
				metadata.DcpNumber;
		}



	}
}