using Screenshoter.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Screenshoter.Viewe
{
	public partial class SASelector : Window
	{
		public SASelector()
		{
			InitializeComponent();
			Width = SystemParameters.VirtualScreenWidth;
			Height = SystemParameters.VirtualScreenHeight;
			Left = SystemParameters.VirtualScreenLeft;
			Top = SystemParameters.VirtualScreenTop;
			SelectMinSize = SASelectorViewModel.SelectMinSize;
			SizeBox.MinWidth = SelectMinSize.x;
			SizeBox.MinHeight = SelectMinSize.y;

			SizeBox.MouseMove += new MouseEventHandler(Element_MouseMove);
			SizeBox.MouseLeftButtonDown += new MouseButtonEventHandler(Element_MouseLeftButtonDown);
			SizeBox.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
			point1.MouseMove += new MouseEventHandler(Point1_MouseMove);
			point1.MouseLeftButtonDown += new MouseButtonEventHandler(Point1_MouseLeftButtonDown);
			point1.MouseLeftButtonUp += new MouseButtonEventHandler(Point1_MouseLeftButtonUp);
			point2.MouseMove += new MouseEventHandler(Point2_MouseMove);
			point2.MouseLeftButtonDown += new MouseButtonEventHandler(Point2_MouseLeftButtonDown);
			point2.MouseLeftButtonUp += new MouseButtonEventHandler(Point2_MouseLeftButtonUp);
			AreaHeight.Text = ((int)SelectMinSize.y).ToString();
			AreaWight.Text = ((int)SelectMinSize.x).ToString();
		}

		private (double x, double y) SelectMinSize;
		private bool IsDragDrop, IsDragPoint1, IsDragPoint2;
		private Point Pos = new Point();
		/// <summary> Закрытие Окна на левую кнопку мышки по фону. </summary>
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => Close();
		/// <summary> Переместить выделеную область на это место. </summary>
		private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (IsDragDrop || IsDragPoint1 || IsDragPoint2)
				return;
			Pos = e.GetPosition(null);
			SizeBox.Margin = new Thickness(Pos.X - SizeBox.Width / 2, Pos.Y - SizeBox.Height / 2, 0, 0);
		}
		/// <summary> Изменение размера выделеной облости за левую верхнюю точку. </summary>
		private void Point1_MouseMove(object sender, MouseEventArgs e)
		{
			if (!IsDragPoint1)
				return;
			(double x, double y) mouse = (e.GetPosition(null).X - Pos.X, e.GetPosition(null).Y - Pos.Y);
			(double x, double y) marging = (SizeBox.Margin.Left + mouse.x, SizeBox.Margin.Top + mouse.y);
			(double x, double y) size = (SizeBox.Width - mouse.x, SizeBox.Height - mouse.y);
			SizeBox.Margin = new Thickness(size.x < SelectMinSize.x ? SizeBox.Margin.Left : marging.x,
											size.y < SelectMinSize.y ? SizeBox.Margin.Top : marging.y, 0, 0);
			SizeBox.Width = size.x < SelectMinSize.x ? SelectMinSize.x : size.x;
			SizeBox.Height = size.y < SelectMinSize.y ? SelectMinSize.y : size.y;
			AreaHeight.Text = ((int)SizeBox.Height).ToString();
			AreaWight.Text = ((int)SizeBox.Width).ToString();
			Pos = e.GetPosition(null);
		}
		/// <summary> Начало изменения размера за левую верхнюю точку. </summary>
		private void Point1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			IsDragPoint1 = true;
			Pos = e.GetPosition(null);
			point1.CaptureMouse();
		}
		/// <summary> Конец изменения размера за левую верхнюю точеку. </summary>
		private void Point1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (!IsDragPoint1)
				return;
			IsDragPoint1 = false;
			point1.ReleaseMouseCapture();
		}
		/// <summary> Изменение размера выделеной области за правую нижнюю точку. </summary>
		private void Point2_MouseMove(object sender, MouseEventArgs e)
		{
			if (!IsDragPoint2)
				return;
			(double x, double y) mouse = (e.GetPosition(null).X - Pos.X, e.GetPosition(null).Y - Pos.Y);
			(double x, double y) size = (SizeBox.Width + mouse.x, SizeBox.Height + mouse.y);
			SizeBox.Width = size.x < SelectMinSize.x ? SelectMinSize.x : size.x;
			SizeBox.Height = size.y < SelectMinSize.y ? SelectMinSize.y : size.y;
			AreaHeight.Text = ((int)SizeBox.Height).ToString();
			AreaWight.Text = ((int)SizeBox.Width).ToString();
			Pos = e.GetPosition(null);
		}
		/// <summary> Начало изменения размера за правую нижнюю точку. </summary>
		private void Point2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			IsDragPoint2 = true;
			Pos = e.GetPosition(null);
			point2.CaptureMouse();
		}
		/// <summary> Конец изменения размера за правую нижнюю точку. </summary>
		private void Point2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (!IsDragPoint2)
				return;
			IsDragPoint2 = false;
			point2.ReleaseMouseCapture();

		}
		/// <summary> Перемещение выделеной области мышкой. </summary>
		private void Element_MouseMove(object sender, MouseEventArgs e)
		{
			if (!IsDragDrop)
				return;
			var currEle = sender as FrameworkElement;
			double xPos = e.GetPosition(null).X - Pos.X + currEle.Margin.Left;
			double yPos = e.GetPosition(null).Y - Pos.Y + currEle.Margin.Top;
			currEle.Margin = new Thickness(xPos, yPos, 0, 0);
			Pos = e.GetPosition(null);

		}
		/// <summary> Начало Перемещения выделеной области мышкой. </summary>
		private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (IsDragPoint1 || IsDragPoint2)
				return;
			FrameworkElement fEle = sender as FrameworkElement;
			IsDragDrop = true;
			Pos = e.GetPosition(null);
			fEle.CaptureMouse();
		}
		/// <summary> Конец Перемещения выделеной области мышкой. </summary>
		private void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (IsDragDrop)
			{
				FrameworkElement ele = sender as FrameworkElement;
				IsDragDrop = false;
				ele.ReleaseMouseCapture();
			}
		}
	}
}
