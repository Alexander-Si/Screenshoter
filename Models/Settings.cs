using System.IO;
using System.Text.Json;

namespace Screenshoter.Models
{
	public struct Settings
	{
		public bool IsFreezeScreen { get; set; }
		public bool IsBackgroundProcess { get; set; }
		public int MakeAreaScreenshot { get; set; }
		public int MakeFullScreenshot { get; set; }
		public int ShowOrHideMenu { get; set; }
		public int CloseScreenshoter { get; set; }

		public Settings Save()
		{
			File.WriteAllText(@"Settings.json", JsonSerializer.Serialize(this, options: new()
			{
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
				WriteIndented = true,
			}));
			return this;
		}

		public static Settings Default()
		{
			return new Settings()
			{
				IsFreezeScreen = false,
				IsBackgroundProcess = true,
				MakeAreaScreenshot = 0x2C,
				MakeFullScreenshot = 0x76,
				CloseScreenshoter = 0x78,
				ShowOrHideMenu = 0x77,
			};
		}

		public static Settings Load()
		{
			if (!File.Exists("Settings.json"))
			{
				var s = Default();
				return new Settings()
				{
					IsFreezeScreen = s.IsFreezeScreen,
					IsBackgroundProcess = s.IsBackgroundProcess,
					MakeAreaScreenshot = s.MakeAreaScreenshot,
					MakeFullScreenshot = s.MakeFullScreenshot,
					ShowOrHideMenu = s.ShowOrHideMenu,
					CloseScreenshoter = s.CloseScreenshoter,
				}.Save();
			}
			var settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(@"Settings.json"));
			return new Settings()
			{
				IsFreezeScreen = settings.IsFreezeScreen,
				IsBackgroundProcess = settings.IsBackgroundProcess,
				MakeAreaScreenshot = settings.MakeAreaScreenshot,
				MakeFullScreenshot = settings.MakeFullScreenshot,
				ShowOrHideMenu = settings.ShowOrHideMenu,
				CloseScreenshoter = settings.CloseScreenshoter,
			};
		}
	}
}
