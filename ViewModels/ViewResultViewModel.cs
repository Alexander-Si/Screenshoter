using M2ViewModelLib.ViewModels.Basic;
using Screenshoter.Models;
using Screenshoter.Viewe;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Screenshoter.ViewModels
{
	public class ViewResultViewModel : ViewModel
	{
		public ViewResultViewModel()
		{
			Saver = new();
		}
		/// <summary> Класс предоставляющий методы для сохранения скриншотов. </summary>
		private FileSaver Saver;
		/// <summary> Это Окно. </summary>
		public ViewResult ThisWindow;
		/// <summary> Передоваемый сюда скриншот. </summary>
		public Bitmap Screenshot { get => _Screenshot; set { _Screenshot = value; ScreenshotImage = BitmapConverter.ConvertToBitmapSourse(value); } }
		private Bitmap _Screenshot;
		/// <summary> Созданое представление скриншота. </summary>
		public BitmapSource ScreenshotImage { get => _ScreenshotImage; set { _ScreenshotImage = value; OnPropertyChanged(); } }
		private BitmapSource _ScreenshotImage;

		public ICommand SaveScreenshot => _SaveScreenshot ??= new RelayCommand(Save);
		private ICommand _SaveScreenshot;
		public ICommand FastSaveScreenshot => _FastSaveScreenshot ??= new RelayCommand(FastSave);
		private ICommand _FastSaveScreenshot;
		public ICommand CopyScreenshot => _CopyScreenshot ??= new RelayCommand(Copy);
		private ICommand _CopyScreenshot;
		public ICommand DeleteScreenshot => _DeleteScreenshot ??= new RelayCommand(Delite);
		private ICommand _DeleteScreenshot;

		/// <summary> Быстрое сохранение. </summary>
		private void FastSave()
		{
			Saver.FastSave(Screenshot);
			Delite();
		}
		/// <summary> Скопировать в буфер обмена. </summary>
		private void Copy()
		{
			Saver.CopyToClipboard(_ScreenshotImage);
			Delite();
		}
		/// <summary> Сохранение. </summary>
		private void Save()
		{
			Saver.Save(_Screenshot);
			Delite();
		}
		/// <summary> Закрытие этого окна и удаление скриншота. </summary>
		private void Delite()
		{
			Screenshot.Dispose();
			ThisWindow.Close();
		}

		~ViewResultViewModel()
		{
			Screenshot.Dispose();
			Screenshot = null;
			ScreenshotImage = null;
		}

	}
}
