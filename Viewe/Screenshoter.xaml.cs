using System.Windows;
using System.Windows.Input;

namespace Screenshoter.Viewe
{
	public partial class Screenshoter : Window
	{
		public Screenshoter() => InitializeComponent();
		/// <summary> Перемещение окна. </summary>
		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
		/// <summary> Закрытие окна. </summary>
		private void CloseExe(object sender, RoutedEventArgs e) => Close();
	}
}
