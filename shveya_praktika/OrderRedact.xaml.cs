using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace shveya_praktika
{
	public partial class OrderRedact : Window
	{
		int orderId;
		public List<ЗаказанныеИзделия> positionOrders = new List<ЗаказанныеИзделия>();
		public List<ЗаказанныеИзделия> positionOrdersSource = new List<ЗаказанныеИзделия>();
		public OrderRedact(int id)
		{
			InitializeComponent();

			orderId = id;
			using (praktika1Entities db = new praktika1Entities())
			{
				foreach (var positionOrder in db.ЗаказанныеИзделия.Where(x => x.IdЗаказа == orderId).ToList())
				{
					positionOrders.Add(positionOrder);
					positionOrdersSource.Add(positionOrder);
				}
			}

			using (praktika1Entities db = new praktika1Entities())
			{
				var products = db.Изделия.ToList();
				ProductList.ItemsSource = products;
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			StackPanelLoad();
			UpdateSum();
		}

		private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
		{
			using (praktika1Entities db = new praktika1Entities())
			{
				var products = db.Изделия.Where(x => x.Наименование.Contains(SearchText.Text) || x.Артикул.Contains(SearchText.Text)).ToList();
				ProductList.ItemsSource = products;
			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			if (ProductList.SelectedIndex != -1)
			{
				positionOrders.Add(new ЗаказанныеИзделия
				{
					АртикулИзделия = ((Изделия)ProductList.SelectedItem).Артикул,
					Количество = 1
				});
				StackPanelLoad();
				UpdateSum();
			}
		}

		public void UpdateSum()
		{
			decimal sum = 0;
			using (praktika1Entities db = new praktika1Entities())
			{
				foreach (var item in positionOrders)
				{
					sum += item.Количество * 100;
				}
			}
			Sum.Text = String.Format("Стоимость: {0:0.00}", sum) + " Рублей";
		}

		public void StackPanelLoad()
		{
			PositionsPlace.Children.Clear();

			foreach (var positionOrder in positionOrders)
			{
				Изделия product = new Изделия();
				using (praktika1Entities db = new praktika1Entities())
				{
					product = db.Изделия.Where(x => x.Артикул == positionOrder.АртикулИзделия).FirstOrDefault();
				}

				Border border = new Border();
				DockPanel dockPanel = new DockPanel();
				StackPanel stackPanel = new StackPanel();
				StackPanel stackPanelInner = new StackPanel();
				TextBox TextBoxQuantity = new TextBox();
				Label labelName = new Label();
				Label labelPrice = new Label();
				Button buttonMinus = new Button();
				Button buttonPlus = new Button();

				DockPanel.SetDock(labelName, Dock.Left);
				DockPanel.SetDock(stackPanel, Dock.Right);

				dockPanel.LastChildFill = false;

				border.BorderBrush = new SolidColorBrush(Color.FromRgb(181, 213, 202));
				border.Background = Brushes.White;
				border.BorderThickness = new Thickness(2);
				border.Width = 800;
				border.Height = 175;
				border.HorizontalAlignment = HorizontalAlignment.Center;
				border.Margin = new Thickness(0, 65, 0, 0);

				buttonMinus.Background = new SolidColorBrush(Color.FromRgb(224, 169, 175));
				buttonPlus.Background = new SolidColorBrush(Color.FromRgb(224, 169, 175));

				stackPanel.Orientation = Orientation.Vertical;
				stackPanelInner.Orientation = Orientation.Horizontal;

				buttonMinus.Width = 65;
				buttonMinus.Height = 35;
				buttonMinus.Content = "-";
				buttonMinus.Margin = new Thickness(0);

				buttonPlus.Width = 65;
				buttonPlus.Height = 35;
				buttonPlus.Content = "+";
				buttonPlus.Margin = new Thickness(0);

				labelName.FontSize = 16;
				labelPrice.FontSize = 16;

				buttonMinus.FontSize = 12;
				buttonPlus.FontSize = 12;
				TextBoxQuantity.FontSize = 12;
				TextBoxQuantity.Width = 66;
				TextBoxQuantity.Height = 35;
				TextBoxQuantity.Margin = new Thickness(0);

				labelName.Margin = new Thickness(34, 25, 0, 0);
				labelPrice.Margin = new Thickness(0, 25, 0, 0);

				stackPanelInner.Margin = new Thickness(0, 56, 48, 5);

				dockPanel.MinWidth = 800;
				dockPanel.Height = 175;
				dockPanel.HorizontalAlignment = HorizontalAlignment.Center;

				labelName.Content = product.Артикул.Trim() + " " + product.Наименование;
				labelPrice.Content = String.Format("{0:0.00}", 100) + " Руб";
				TextBoxQuantity.Text = positionOrder.Количество.ToString();
				TextBoxQuantity.HorizontalContentAlignment = HorizontalAlignment.Center;
				TextBoxQuantity.VerticalContentAlignment = VerticalAlignment.Center;


				stackPanelInner.Children.Add(buttonMinus);
				stackPanelInner.Children.Add(TextBoxQuantity);
				stackPanelInner.Children.Add(buttonPlus);

				stackPanel.Children.Add(labelPrice);
				stackPanel.Children.Add(stackPanelInner);

				dockPanel.Children.Add(labelName);
				dockPanel.Children.Add(stackPanel);

				border.Child = dockPanel;

				PositionsPlace.Children.Add(border);

				buttonPlus.Click += (s, e) =>
				{
					positionOrder.Количество++;

					UpdateSum();
					StackPanelLoad();
				};
				buttonMinus.Click += (s, e) =>
				{
					int quant = --positionOrder.Количество;
					if (quant > 0)
					{
						StackPanelLoad();
					}
					else
					{
						positionOrders.Remove(positionOrder);
						StackPanelLoad();
					}
					UpdateSum();
				};
				TextBoxQuantity.LostFocus += (s, e) =>
				{
					int quantity = Convert.ToInt32(TextBoxQuantity.Text);

					if (quantity > 0)
					{
						positionOrder.Количество = quantity;
					}
					else
					{
						positionOrders.Remove(positionOrder);
						StackPanelLoad();
					}
					UpdateSum();
				};
				TextBoxQuantity.KeyDown += (s, e) =>
				{
					if (!(e.Key >= Key.D0 && e.Key <= Key.D9))
					{
						e.Handled = true;
					}
				};
			}
		}
		private void ButtonOrderRedact_Click(object sender, RoutedEventArgs e)
		{
			if (positionOrders.Count > 0)
			{
				using (praktika1Entities db = new praktika1Entities())
				{
					foreach (var positionOrder in positionOrdersSource.Where(x => x.IdЗаказа == orderId))
					{
						db.Entry(positionOrder).State = System.Data.Entity.EntityState.Deleted;
						db.SaveChanges();
					}

					foreach (var item in positionOrders)
					{
						ЗаказанныеИзделия positionOrder = new ЗаказанныеИзделия();
						positionOrder.IdЗаказа = orderId;
						positionOrder.АртикулИзделия = item.АртикулИзделия;
						positionOrder.Количество = item.Количество;
						db.ЗаказанныеИзделия.Add(positionOrder);
						db.SaveChanges();
					}
				}

				Orders orders = new Orders();
				orders.Show();
				this.Close();
			}
			else
			{
				MessageBox.Show("Нет выбранных изделий");
			}
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			CurrentUser.Id = 0;
			Authorization authorization = new Authorization();
			authorization.Show();
			this.Close();
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			Orders orders = new Orders();
			orders.Show();
			this.Close();
		}
	}
}
