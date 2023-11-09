using System.Windows;

namespace shveya_praktika
{
	public partial class DirectorScreen : Window
	{
		public DirectorScreen()
		{
			InitializeComponent();
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			CurrentUser.Id = 0;
			Authorization authorization = new Authorization();
			authorization.Show();
			this.Close();
		}

		private void MaterialsButton_Click(object sender, RoutedEventArgs e)
		{
			Materials materials = new Materials();
			materials.Show();
			Close();
		}

		private void ReportsButton_Click(object sender, RoutedEventArgs e)
		{
			Reports reports = new Reports();
			reports.Show();
			this.Close();
		}
	}
}
