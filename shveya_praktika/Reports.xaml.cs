using System.Windows;

namespace shveya_praktika
{
	public partial class Reports : Window
	{
		public Reports()
		{
			InitializeComponent();
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			DirectorScreen directorScreen = new DirectorScreen();
			directorScreen.Show();
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
