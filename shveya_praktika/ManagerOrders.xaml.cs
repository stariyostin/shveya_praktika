using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace shveya_praktika
{
	public partial class ManagerOrders : Window
	{
		public ManagerOrders()
		{
			InitializeComponent();

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
		}

		private void Processing_Click(object sender, RoutedEventArgs e)
		{
			using (praktika1Entities db = new praktika1Entities())
			{
				var order = db.Заказы.Where(x => x.Id == ((OrderForDataGrid)OrderList.SelectedItem).Id).First();
				order.Этап = "К оплате";
				db.SaveChanges();

				Processing.IsEnabled = false;
				Reject.IsEnabled = false;
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
		}

		private void OrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (OrderList.SelectedItem != null)
			{
				if (((OrderForDataGrid)OrderList.SelectedItem).Status == "Новый")
				{
					using (praktika1Entities db = new praktika1Entities())
					{
						var order = db.Заказы.Where(x => x.Id == ((OrderForDataGrid)OrderList.SelectedItem).Id).First();
						order.Этап = "Обработка";
						order.Менеджер = CurrentUser.Id;
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
				}
				else if (((OrderForDataGrid)OrderList.SelectedItem).Status == "Обработка")
				{
					Processing.IsEnabled = true;
					Reject.IsEnabled = true;
				}
				else
				{
					Processing.IsEnabled = false;
					Reject.IsEnabled = false;
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

		private void Reject_Click(object sender, RoutedEventArgs e)
		{
			using (praktika1Entities db = new praktika1Entities())
			{
				var order = db.Заказы.Where(x => x.Id == ((OrderForDataGrid)OrderList.SelectedItem).Id).First();
				order.Этап = "Отклонён";
				db.SaveChanges();

				Processing.IsEnabled = false;
				Reject.IsEnabled = false;
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
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			ManagerScreen managerScreen = new ManagerScreen();
			managerScreen.Show();
			this.Close();
		}
	}
}
