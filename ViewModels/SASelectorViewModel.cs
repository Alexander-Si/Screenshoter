using M2ViewModelLib.ViewModels.Basic;
using Screenshoter.Models;
using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Screenshoter.Viewe;

namespace Screenshoter.ViewModels
{
	public class SASelectorViewModel : ViewModel
	{
		public SASelectorViewModel()
		{
			WightSize = 400;
			HeightSize = 200;
			Marging = new Thickness(200, 200, 0, 0);
		}

		public Bitmap Screenshot { get => _Screenshot; set { _Screenshot = value; ScreenshotImage = BitmapConverter.ConvertToBitmapSourse(value); } }
		private Bitmap _Screenshot;
		/// <summary> Созданое представление скриншота. </summary>
		public BitmapSource ScreenshotImage { get => _ScreenshotImage; set { _ScreenshotImage = value; OnPropertyChanged(); } }
		private BitmapSource _ScreenshotImage;

		/// <summary> Минимальные размеры выделеной области. </summary>
		public static (double x, double y) SelectMinSize = (164, 100);
		/// <summary> Окно SASelector </summary>
		public Window ThisWindow;

		/// <summary> Вызывается при окончании выделения области экрана. </summary>
		public ICommand CreareArea => _CreareArea ??= new RelayCommand(CreateAreaExe);
		private ICommand _CreareArea;
		/// <summary> Ширина выделеной области. </summary>
		public string Wight
		{
			get => Size.x.ToString();
			set
			{
				if (!int.TryParse(value, out Size.x) && Size.x >= SelectMinSize.x)
					return;
				OnPropertyChanged("WightSize");
				OnPropertyChanged();
			}
		}
		/// <summary> высота выделеной области. </summary>
		public string Height
		{
			get => Size.y.ToString();
			set
			{
				if (!int.TryParse(value, out Size.y) && Size.y >= SelectMinSize.y)
					return;
				OnPropertyChanged("HeightSize");
				OnPropertyChanged();
			}
		}
		/// <summary> Ширина выделной области (Размеры). </summary>
		public double WightSize
		{
			get => Size.x;
			set
			{
				Size.x = value < SelectMinSize.x ? (int)SelectMinSize.x : (int)Math.Round(value);
				OnPropertyChanged("Wight");
				OnPropertyChanged();
			}
		}
		/// <summary> Высора выделеной области (Размеры). </summary>
		public double HeightSize
		{
			get => Size.y;
			set
			{
				Size.y = value < SelectMinSize.y ? (int)SelectMinSize.y : (int)Math.Round(value);
				OnPropertyChanged("Height");
				OnPropertyChanged();
			}
		}
		/// <summary> Отступы выделеной области (используются только левый (left) и верхний (top)). </summary>
		public Thickness Marging
		{
			get => _Marging;
			set
			{
				_Marging = value;
				OnPropertyChanged();
			}
		}

		private Thickness _Marging;
		private (int x, int y) Size;

		/// <summary> Создать Скриншот из выделеной области экрана. </summary>
		private void CreateAreaExe()
		{
			ThisWindow.Visibility = Visibility.Hidden;
			if(ScreenshoterViewModel.AppSettings.IsFreezeScreen)
			{
				ScreenshotMaker.CutTheScreenAsynk(Screenshot, (int)Marging.Left, (int)Marging.Top, Size.x, Size.y);
			}
			else
			{
				ScreenshotMaker.MakeAreaScreenAsynk((int)Marging.Left, (int)Marging.Top, Size.x, Size.y);
			}
			Screenshot?.Dispose();
			Screenshot = null;
			ScreenshotImage = null;
			ThisWindow.Close();
		}
	}
}
