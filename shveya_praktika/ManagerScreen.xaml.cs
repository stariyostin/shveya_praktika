using System.Windows;

namespace shveya_praktika
{
	public partial class ManagerScreen : Window
	{
		public ManagerScreen()
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

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Materials materials = new Materials();
			materials.Show();
			this.Close();
		}

		private void OrdersButton_Click(object sender, RoutedEventArgs e)
		{
			ManagerOrders managerOrders = new ManagerOrders();
			managerOrders.Show();
			this.Close();
		}
	}
}
