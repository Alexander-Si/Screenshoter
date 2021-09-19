using M2ViewModelLib.ViewModels.Basic;
using Screenshoter.Models;
using Screenshoter.Viewe;
using System;
using System.ComponentModel;
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
			ThisWindow.Loaded += OnEndLoadWindow;
			ThisWindow.Closing += OnStartCloseWindow;

			ScreenshotMaker.OnEndCreateScreenshot += EndCreateScreenshot;
			if (AppSettings.IsBackgroundProcess)
			{
				ThisWindow.Visibility = Visibility.Hidden;
				ThisWindow.ShowInTaskbar = false;
			}
			KeyListener.OnPressedKey += KeyboardPressedKey;
			KeyListener.SetHook();
		}

		private void OnEndLoadWindow(object sender, RoutedEventArgs e)
		{
			NotifyIcon = new(ThisWindow, new(@"F:\OneDrive\Screenshoter\screenshoterIcon.ico"));
			NotifyIcon.LeftClick += MakeAreaScreen;
			NotifyIcon.RightClick += ShowOrHideWindow;
			NotifyIcon.IsChangedClick = true;
		}

		public static NotifyIcon NotifyIcon;
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
			StartControllLock();
			var vm = new SASelectorViewModel();
			var selector = new SASelector() { DataContext = vm };
			vm.ThisWindow = selector;
			selector.Closing += AreaScreenClosing;
			if (AppSettings.IsFreezeScreen)
			{
				vm.Screenshot = ScreenshotMaker.MakeFullScreen();
				selector.ShowDialog();
			}
			else
			{
				selector.ShowDialog();
			}
		}
		private void AreaScreenClosing(object sender, CancelEventArgs e) => EndControllLock();

		/// <summary> Создать скриншот из всего экрана. </summary>
		private void MakeFullScreen()
		{
			StartControllLock();
			ScreenshotMaker.MakeFullScreenAsynk();
		}

		/// <summary> По завершению создания скриншота. </summary>
		/// <param name="screenshot"> Возвращаемая картинка. </param>
		private void EndCreateScreenshot(Screenshot screenshot)
		{
			EndControllLock();
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

		private void ShowOrHideWindow() => ThisWindow.Visibility = ThisWindow.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

		private void StartControllLock()
		{
			KeyListener.ClearHook();
			NotifyIcon.IsChangedClick = false;
			HideWindow();
		}

		private void EndControllLock()
		{
			KeyListener.SetHook();
			NotifyIcon.IsChangedClick = true;
			if (!AppSettings.IsBackgroundProcess)
				ShowWindow();
		}

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

		private void OnStartCloseWindow(object sender, CancelEventArgs e)
		{
			NotifyIcon.Hide();
			NotifyIcon.Dispose();
		}
	}
}
