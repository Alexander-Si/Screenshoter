using M2ViewModelLib.ViewModels.Basic;
using Screenshoter.Models;
using Screenshoter.Viewe;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Screenshoter.ViewModels
{
	internal class ScreenshoterViewModel : ViewModel
	{
		public ScreenshoterViewModel()
		{
			AppSettings = Settings.Load();
			ThisWindow = Application.Current.Windows.OfType<Window>().First();

			ScreenshotMaker.OnEndCreateScreenshot += EndCreateScreenshot;
			if (AppSettings.IsBackgroundProcess)
			{
				ThisWindow.Visibility = Visibility.Hidden;
				ThisWindow.ShowInTaskbar = false;

				KeyListener.OnPressedKey += KeyboardPressedKey;
			}
			KeyListener.SetHook();
		}

		public static Settings AppSettings;
		/// <summary> Поток интерфейса (этот поток) </summary>
		private Window ThisWindow { get; init; }

		public ICommand FullScreen => _FullScreen ??= new RelayCommand(MakeFullScreen);
		private ICommand _FullScreen;
		public ICommand AreaScreen => _AreaScreen ??= new RelayCommand(MakeAreaScreen);
		private ICommand _AreaScreen;
		public ICommand OpenExplorer => _OpenExplorer ??= new RelayCommand(FileSaver.OpenExplorer);
		private ICommand _OpenExplorer;
		public ICommand OpenInfo => _OpenInfo ??= new RelayCommand(OpenInfoExe);
		private ICommand _OpenInfo;

		/// <summary> Создать скриншот из области экрана. </summary>
		private void MakeAreaScreen()
		{
			KeyListener.ClearHook();
			if (!AppSettings.IsBackgroundProcess)
				HideWindow();
			if (AppSettings.IsFreezeScreen)
			{
				var vm = new SASelectorViewModel() { Screenshot = ScreenshotMaker.MakeFullScreen() };
				var selector = new SASelector() { DataContext = vm };
				vm.ThisWindow = selector;
				selector.ShowDialog();
			}
			else
			{
				var vm = new SASelectorViewModel();
				var selector = new SASelector() { DataContext = vm };
				vm.ThisWindow = selector;
				selector.ShowDialog();
			}
		}
		/// <summary> Создать скриншот из всего экрана. </summary>
		private void MakeFullScreen()
		{
			KeyListener.ClearHook();
			if (!AppSettings.IsBackgroundProcess)
				HideWindow();
			ScreenshotMaker.MakeFullScreenAsynk();
		}

		/// <summary> По завершению создания скриншота. </summary>
		/// <param name="screenshot"> Возвращаемая картинка. </param>
		private void EndCreateScreenshot(Bitmap screenshot)
		{
			KeyListener.SetHook();
			if (!AppSettings.IsBackgroundProcess)
				ShowWindow();
			else
				KeyListener.SetHook();
			var vm = new ViewResultViewModel() { Screenshot = screenshot };
			ViewResult VrWindow = new() { DataContext = vm };
			vm.ThisWindow = VrWindow;
			VrWindow.Show();
		}

		private void OpenInfoExe()
		{
			InfoWindow VrWindow = new();
			VrWindow.ShowDialog();
		}

		/// <summary> Показать окно (после создания скриншота) </summary>
		private void ShowWindow() => ThisWindow.Visibility = Visibility.Visible;
		/// <summary> Спрятать окно (перед созданием скриншота) </summary>
		private void HideWindow() => ThisWindow.Visibility = Visibility.Hidden;

		private void KeyboardPressedKey(in KeyEventHandle key)
		{
			if (key.PressedType != KeyPressedType.WM_KEYDOWN)
				return;
			if (key.Key == AppSettings.MakeAreaScreenshot) // print screen
				MakeAreaScreen();
			else if (key.Key == AppSettings.MakeFullScreenshot) // f7
				MakeFullScreen();
			else if (key.Key == AppSettings.ShowOrHideMenu) // f8
				ThisWindow.Visibility = ThisWindow.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
			else if (key.Key == AppSettings.CloseScreenshoter) // f9
				ThisWindow.Close();
		}
	}
}
