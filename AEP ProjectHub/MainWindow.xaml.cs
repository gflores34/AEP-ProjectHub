using System.Windows;

namespace AEP_ProjectHub
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

            MainFrame.Navigate(new Views.DashboardPage());
		}
	}
}