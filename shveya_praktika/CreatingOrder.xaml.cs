using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace shveya_praktika
{
	public partial class CreatingOrder : Window
	{
		public CreatingOrder()
		{
			InitializeComponent();

			using (praktika1Entities db = new praktika1Entities())
			{
				var products = db.Изделия.ToList();
				ProductList.ItemsSource = products;
			}
		}

		public void UpdateSum()
		{
			decimal sum = BasketItems.Lines.Sum(x => 100 * x.Quantity);
			Sum.Text = String.Format("Стоимость: {0:0.00}", sum) + " Рублей";
		}

		public void StackPanelLoad()
		{
			PositionsPlace.Children.Clear();

			foreach (var basketLine in BasketItems.Lines)
			{
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
				TextBoxQuantity.Margin = new Thickness(0, 0, 0, 0);

				labelName.Margin = new Thickness(34, 25, 0, 0);
				labelPrice.Margin = new Thickness(0, 25, 0, 0);

				stackPanelInner.Margin = new Thickness(0, 56, 48, 5);

				dockPanel.MinWidth = 800;
				dockPanel.Height = 175;
				dockPanel.HorizontalAlignment = HorizontalAlignment.Center;

				labelName.Content = basketLine.Product.Артикул.Trim() + " " + basketLine.Product.Наименование;
				labelPrice.Content = String.Format("{0:0.00}", 100) + " Руб";
				TextBoxQuantity.Text = basketLine.Quantity.ToString();
				TextBoxQuantity.HorizontalContentAlignment = HorizontalAlignment.Center;


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
					basketLine.Quantity++;

					UpdateSum();
					StackPanelLoad();
				};
				buttonMinus.Click += (s, e) =>
				{
					int quant = --basketLine.Quantity;
					if (quant > 0)
					{
						StackPanelLoad();
					}
					else
					{
						BasketItems.Lines.Remove(basketLine);
						StackPanelLoad();
					}
					UpdateSum();
				};
				TextBoxQuantity.LostFocus += (s, e) =>
				{
					int quantity = Convert.ToInt32(TextBoxQuantity.Text);

					if (quantity > 0)
					{
						basketLine.Quantity = quantity;
					}
					else
					{
						BasketItems.Lines.Remove(basketLine);
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

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			if (ProductList.SelectedIndex != -1)
			{
				BasketItems.AddItem((Изделия)ProductList.SelectedItem);
				StackPanelLoad();
				UpdateSum();
			}
		}

		public void QuantityPlus(object sender, RoutedEventArgs e)
		{
			string idString = (sender as Button).Name.Remove(0, 4);

			int quant = ++BasketItems.Lines.Where(x => x.Product.Артикул == idString).First().Quantity;

			UpdateSum();
			StackPanelLoad();

		}
		public void QuantityMinus(object sender, RoutedEventArgs e)
		{
			string idString = (sender as Button).Name.Remove(0, 5);

			int quant = --BasketItems.Lines.Where(x => x.Product.Артикул == idString).First().Quantity;

			if (quant > 0)
			{
				StackPanelLoad();
			}
			else
			{
				BasketItems.Lines.Remove(BasketItems.Lines.Where(x => x.Product.Артикул == idString).First());
				StackPanelLoad();
			}
			UpdateSum();

		}

		public void ChangeQuantity(object sender, RoutedEventArgs e)
		{
			int quantity = Convert.ToInt32((sender as TextBox).Text);
			string Id = (sender as TextBox).Name.Remove(0, 7);

			if (quantity > 0)
			{
				BasketItems.Lines.Where(x => x.Product.Артикул == Id).First().Quantity = quantity;
			}
			else
			{
				BasketItems.Lines.Remove(BasketItems.Lines.Where(x => x.Product.Артикул == Id).First());
				StackPanelLoad();
			}
			UpdateSum();
		}

		private void BuyButton_Click(object sender, RoutedEventArgs e)
		{
			if (BasketItems.Lines.Count > 0)
			{
				using (praktika1Entities db = new praktika1Entities())
				{
					Заказы order = new Заказы();
					order.Дата = DateTime.Now;
					order.Этап = "Новый";
					order.Заказчик = CurrentUser.Id;

					db.Заказы.Add(order);
					db.SaveChanges();

					foreach (var line in BasketItems.Lines)
					{
						ЗаказанныеИзделия productOrder = new ЗаказанныеИзделия();
						productOrder.IdЗаказа = order.Id;
						productOrder.АртикулИзделия = line.Product.Артикул;
						productOrder.Количество = line.Quantity;
						db.ЗаказанныеИзделия.Add(productOrder);
						db.SaveChanges();
					}
					BasketItems.Lines.Clear();
					CustomerScreen customerScreen = new CustomerScreen();
					customerScreen.Show();
					this.Close();
				}
			}
			else
			{
				MessageBox.Show("Вы не выбрали ни одно изделие");
			}
		}

		private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
		{
			using (praktika1Entities db = new praktika1Entities())
			{
				var products = db.Изделия.Where(x => x.Наименование.Contains(SearchText.Text) || x.Артикул.Contains(SearchText.Text)).ToList();
				ProductList.ItemsSource = products;
			}
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			CurrentUser.Id = 0;
			BasketItems.Lines.Clear();
			Authorization authorization = new Authorization();
			authorization.Show();
			this.Close();
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			BasketItems.Lines.Clear();
			CustomerScreen customerScreen = new CustomerScreen();
			customerScreen.Show();
			this.Close();
		}
	}
}
