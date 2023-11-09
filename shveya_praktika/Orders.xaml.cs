using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace shveya_praktika
{
	public partial class Orders : Window
	{
		public Orders()
		{
			InitializeComponent();

			List<OrderForDataGrid> orderForDataGrids = new List<OrderForDataGrid>();

			using (praktika1Entities db = new praktika1Entities())
			{
				var orders = db.Заказы.Where(x => x.Заказчик == CurrentUser.Id);
				foreach (var item in orders)
				{
					orderForDataGrids.Add(new OrderForDataGrid(item.Id));
				}
			}

			OrderList.ItemsSource = orderForDataGrids;
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			OrderRedact orderRedact = new OrderRedact(((OrderForDataGrid)OrderList.SelectedItem).Id);
			orderRedact.Show();
			this.Close();
		}

		private void OrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (OrderList.SelectedItem != null)
			{
				if (((OrderForDataGrid)OrderList.SelectedItem).Status == "Новый")
				{
					Edit.IsEnabled = true;
					Pay.IsEnabled = false;
				}
				else if (((OrderForDataGrid)OrderList.SelectedItem).Status == "К оплате")
				{
					Pay.IsEnabled = true;
					Edit.IsEnabled = false;
				}
				else
				{
					Edit.IsEnabled = false;
					Pay.IsEnabled = false;
				}
			}
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			CurrentUser.Id = 0;
			Authorization authorization = new Authorization();
			authorization.Show();
			this.Close();
		}

		private void Pay_Click(object sender, RoutedEventArgs e)
		{
			using (praktika1Entities db = new praktika1Entities())
			{
				var order = db.Заказы.Where(x => x.Id == ((OrderForDataGrid)OrderList.SelectedItem).Id).First();
				order.Этап = "Готов";
				db.SaveChanges();
			}

			List<OrderForDataGrid> orderForDataGrids = new List<OrderForDataGrid>();

			using (praktika1Entities db = new praktika1Entities())
			{
				var orders = db.Заказы.Where(x => x.Менеджер == CurrentUser.Id || x.Менеджер == null);
				foreach (var item in orders)
				{
					orderForDataGrids.Add(new OrderForDataGrid(item.Id));
				}
			}

			OrderList.ItemsSource = orderForDataGrids;

			Edit.IsEnabled = false;
			Pay.IsEnabled = false;
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			CustomerScreen customerScreen = new CustomerScreen();
			customerScreen.Show();
			this.Close();
		}
	}
}
