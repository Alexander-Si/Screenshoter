using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Screenshoter.Viewe
{
	public partial class InfoWindow : Window
	{
		public InfoWindow() => InitializeComponent();

		private void Button_Click(object sender, RoutedEventArgs e) => Close();
		private void Move(object sender, MouseButtonEventArgs e) => DragMove();

		private void Project(object sender, RoutedEventArgs e) => Process.Start(new ProcessStartInfo("https://github.com/Alexander-Si/Screenshoter") { UseShellExecute = true });
		private void GitHub(object sender, RoutedEventArgs e) => Process.Start(new ProcessStartInfo("https://github.com/Alexander-Si") { UseShellExecute = true });
		private void VK(object sender, RoutedEventArgs e) => Process.Start(new ProcessStartInfo("https://vk.com/alexander_si_w") { UseShellExecute = true }); 
	}
}
