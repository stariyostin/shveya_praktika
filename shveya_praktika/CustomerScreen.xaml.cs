using System.Windows;

namespace shveya_praktika
{
	public partial class CustomerScreen : Window
	{
		public CustomerScreen()
		{
			InitializeComponent();
		}

		private void OrderCreateButton_Click(object sender, RoutedEventArgs e)
		{
			CreatingOrder creatingOrder = new CreatingOrder();
			creatingOrder.Show();
			this.Close();
		}

		private void OrdersButton_Click(object sender, RoutedEventArgs e)
		{
			Orders orders = new Orders();
			orders.Show();
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
