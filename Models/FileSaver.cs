using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Screenshoter.Models
{
	internal class FileSaver
	{
		/// <summary> Пудь до Папки в которую надо сохранять готовые изображения при быстром сохранении. </summary>
		public static string DefaultPath { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\\Screenshots";
		/// <summary> Открыть папку по умолчанию. </summary>
		public static void OpenExplorer()
		{
			System.Diagnostics.Process.Start("explorer.exe", $"{DefaultPath}");
		}
		/// <summary> Быстрое сохранение изображения в папку по умолчанию. </summary>
		/// <param name="bitmap"> Изображение которое надо сохранить. </param>
		public void FastSave(Bitmap bitmap)
		{
			;
			if (!Directory.Exists(DefaultPath))
				Directory.CreateDirectory(DefaultPath);
			var filePath = $"{DefaultPath}\\{DateTime.Now.ToString("u").Replace(':', '-')}.png";
			bitmap.Save(filePath, ImageFormat.Png);
		}

		/// <summary> Копировать в буфер обмена. </summary>
		/// <param name="source"> Картинка. </param>
		public void CopyToClipboard(BitmapSource source) => Clipboard.SetImage(source);
		/// <summary> Сохранить изображение в выбраную папку. </summary>
		/// <param name="bitmap"> Картинка. </param>
		public void Save(Bitmap bitmap)
		{
			SaveFileDialog saveDialog = new()
			{
				Filter = "Image File|*.*|Png Image|*.png|Jpeg Image|*.jpge|Tiff Image|*.tiff|Gif Image|*.gif|BMP Image" +
						"|*.bmp|Icon|*.ico|Exif File|*.exif|WMF (Windows Meta File)|*.wmf|EMF (Extensible Meta File)|*.emf",
				Title = "Save Screenshot",
				DefaultExt = ".png"
			};
			saveDialog.ShowDialog();
			if (saveDialog.FileName == string.Empty)
				return;
			var imgG = Path.GetExtension(saveDialog.FileName) switch
			{
				".jpeg" => ImageFormat.Jpeg,
				".tiff" => ImageFormat.Tiff,
				".gif" => ImageFormat.Gif,
				".bmp" => ImageFormat.Bmp,
				".ico" => ImageFormat.Icon,
				".wmf" => ImageFormat.Wmf,
				".emf" => ImageFormat.Emf,
				".exif" => ImageFormat.Exif,
				_ or ".png" => ImageFormat.Png,
			};
			bitmap.Save(saveDialog.FileName, imgG);
		}

	}
}
