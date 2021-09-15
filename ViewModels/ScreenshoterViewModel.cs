using M2ViewModelLib.ViewModels.Basic;
using Screenshoter.Models;
using Screenshoter.Viewe;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Screenshoter.ViewModels
{
	internal class ScreenshoterViewModel : ViewModel
	{
		public ScreenshoterViewModel()
		{
			ScreenstotterWindow = Application.Current.Windows.OfType<Window>().First();
			UIThread = Dispatcher.CurrentDispatcher;

			ScreenshotMaker.UIThread = UIThread;
			ScreenshotMaker.StartScreenshotting += HideWindow;
			ScreenshotMaker.EndScreenshotting += ShowWindow;
			ScreenshotMaker.OnEndCreateScreenshot += EndCreateScreenshot;
		}
		/// <summary> Поток интерфейса (этот поток) </summary>
		private Dispatcher UIThread;
		/// <summary> Главное окно (меню) </summary>
		private Window ScreenstotterWindow;

		public ICommand FullScreen => _FullScreen ??= new RelayCommand(MakeFullScreen);
		private ICommand _FullScreen;
		public ICommand AreaScreen => _AreaScreen ??= new RelayCommand(MakeAreaScreen);
		private ICommand _AreaScreen;
		public ICommand OpenExplorer => _OpenExplorer ??= new RelayCommand(FileSaver.OpenExplorer);
		private ICommand _OpenExplorer;

		/// <summary> Создать скриншот из области экрана. </summary>
		private void MakeAreaScreen()
		{
			var vm = new SASelectorViewModel();
			var selector = new SASelector() { DataContext = vm };
			vm.ThisWindow = selector;
			ScreenstotterWindow.Visibility = Visibility.Hidden;
			selector.ShowDialog();
		}
		/// <summary> Создать скриншот из всего экрана. </summary>
		private void MakeFullScreen() => ScreenshotMaker.MakeFullScreenAsynk();

		/// <summary> По завершению создания скриншота. </summary>
		/// <param name="screenshot"> Возвращаемая картинка. </param>
		private void EndCreateScreenshot(Bitmap screenshot)
		{
			var vm = new ViewResultViewModel() { Screenshot = screenshot };
			ViewResult VrWindow = new() { DataContext = vm };
			vm.ThisWindow = VrWindow;
			VrWindow.Show();
		}

		/// <summary> Показать окно (после создания скриншота) </summary>
		private void ShowWindow() => ScreenstotterWindow.Visibility = Visibility.Visible;
		/// <summary> Спрятать окно (перед созданием скриншота) </summary>
		private void HideWindow() => ScreenstotterWindow.Visibility = Visibility.Hidden;
	}
}
