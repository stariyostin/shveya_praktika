using System.Linq;
using System.Windows;

namespace shveya_praktika
{
	public partial class Registration : Window
	{
		public Registration()
		{
			InitializeComponent();
		}

		private void ButtonReg_Click(object sender, RoutedEventArgs e)
		{
			if (Login.Text == "" || Password.Password == "" || RepeatedPassword.Password == "")
			{
				MessageBox.Show("Заполните все поля");
			}
			else
			{
				if (Password.Password != RepeatedPassword.Password)
				{
					MessageBox.Show("Пароли не совпадают");
				}
				else
				{
					using (var db = new praktika1Entities())
					{
						Пользователи user = new Пользователи
						{
							Логин = Login.Text,
							Пароль = Password.Password,
							IdРоли = 4
						};
						if (db.Пользователи.Any(x => x.Логин == Login.Text))
						{
							MessageBox.Show("Пользователь с таким логином уже зарегистрирован");
						}
						else
						{
							db.Пользователи.Add(user);
							db.SaveChanges();
							Authorization authorization = new Authorization();
							authorization.Show();
							this.Close();
						}
					}
				}
			}
		}

		private void AutButton_Click(object sender, RoutedEventArgs e)
		{
			Authorization authorization = new Authorization();
			authorization.Show();
			this.Close();
		}
	}
}
