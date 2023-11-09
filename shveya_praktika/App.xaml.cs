using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace shveya_praktika
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{

	}
	public static class CurrentUser
	{
		public static int Id { get; set; } = 0;
	}

	public static class BasketItems
	{
		public static List<BasketLine> Lines { get; set; } = new List<BasketLine>();
		public static void AddItem(Изделия product, int quantity = 1)
		{
			BasketLine line = Lines.Where(p => p.Product.Артикул == product.Артикул).FirstOrDefault();
			if (line == null)
			{
				Lines.Add(new BasketLine
				{
					Product = product,
					Quantity = quantity
				});
			}
			else
			{
				line.Quantity += quantity;
			}
		}
	}

	public class BasketLine
	{
		public Изделия Product { get; set; }
		public int Quantity { get; set; }
	}

	public class OrderForDataGrid
	{
		public OrderForDataGrid(int id)
		{
			using (praktika1Entities db = new praktika1Entities())
			{
				var order = db.Заказы.Where(x => x.Id == id).FirstOrDefault();
				Id = id;
				Date = order.Дата.ToString("dd-MM-yyyy");
				Customer = order.Заказчик;
				Manager = order.Менеджер;

				int count;
				count = db.ЗаказанныеИзделия.Where(x => x.IdЗаказа == id).Sum(x => x.Количество);
				Count = count;
				Status = order.Этап;
			}
		}
		public int Id { get; set; }
		public string Date { get; set; }
		public int Count { get; set; }
		public int Customer { get; set; }
		public int? Manager { get; set; }
		public string Status { get; set; }
	}
}
