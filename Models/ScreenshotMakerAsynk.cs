using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Screenshoter.Models
{
	public partial class ScreenshotMaker
	{
		/// <summary> Вызывается по завершению сохранения области экрана и возвращает Bitmap. (только asynk методы) </summary>
		public static event Action<Screenshot> OnEndCreateScreenshot;
		/// <summary> Асинхроное сохранение всей области экрана. (возвращает результат в OnEndCreateScreenshot) </summary>
		public static async Task MakeFullScreenAsynk() => OnEndCreateScreenshot?.Invoke(await Task.Run(() => MakeFullScreen()));
		/// <summary> Синхроное сохранение Области экрана. (возвращает результат в OnEndCreateScreenshot) </summary>
		/// <param name="left"> Отступ от левого края экрана. </param>
		/// <param name="top"> Отступ от правого края экрана. </param>
		/// <param name="wight"> Ширина сохраняемой области. </param>
		/// <param name="height"> Высота сохраняемой области. </param>
		public static async Task MakeAreaScreenAsynk(int left, int top, int wight, int height) => OnEndCreateScreenshot?.Invoke(await Task.Run(() => MakeAreaScreen(left, top, wight, height)));

		public static async Task CutTheScreenAsynk(Screenshot screen, int left, int top, int wight, int height) => OnEndCreateScreenshot?.Invoke(await Task.Run(() => CutTheScreen(screen, left, top, wight, height)));

		public static async Task CutTheScreenAsynk(Screenshot screen, Point location, Size size) => OnEndCreateScreenshot?.Invoke(await Task.Run(() => CutTheScreen(screen, location, size)));
	}
}
