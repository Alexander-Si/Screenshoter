using System.Windows;
using System.Windows.Input;

namespace Screenshoter.Viewe
{
	public partial class ViewResult : Window
	{
		public ViewResult() => InitializeComponent();
		/// <summary> Перемещение окна. </summary>
		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
	}
}
