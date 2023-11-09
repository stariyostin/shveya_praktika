using System.Windows;

namespace shveya_praktika
{
	public partial class StorekeeperScreen : Window
	{
		public StorekeeperScreen()
		{
			InitializeComponent();
		}

		private void MatButton_Click(object sender, RoutedEventArgs e)
		{
			Materials materials = new Materials();
			materials.Show();
			this.Close();
		}

		private void InventoryButton_Click(object sender, RoutedEventArgs e)
		{
			Inventory inventory = new Inventory();
			inventory.Show();
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
