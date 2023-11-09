using System.Windows;

namespace shveya_praktika
{
	public partial class Inventory : Window
	{
		public Inventory()
		{
			InitializeComponent();
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			StorekeeperScreen storekeeperScreen = new StorekeeperScreen();
			storekeeperScreen.Show();
			this.Close();
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			CurrentUser.Id = 0;
			Authorization authorization = new Authorization();
			authorization.Show();
			this.Close();
		}
	}
}
