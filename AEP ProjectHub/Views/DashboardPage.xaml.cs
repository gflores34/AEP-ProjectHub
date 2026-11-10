using System.Windows;
using System.Windows.Controls;

namespace AEP_ProjectHub.Views
{
	public partial class DashboardPage : Page
	{
		public DashboardPage()
		{
			InitializeComponent();
		}

		private void btnPackageManager_Click(
			object sender,
			RoutedEventArgs e)
		{
			NavigationService?.Navigate(
				new PackageManagerPage());
		}

	}
}