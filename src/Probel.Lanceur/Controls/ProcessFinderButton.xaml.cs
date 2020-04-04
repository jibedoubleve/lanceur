using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Probel.Lanceur.Controls
{
    /// <summary>
    /// Interaction logic for ProcessFinderButton.xaml
    /// </summary>
    public partial class ProcessFinderButton : UserControl
    {
        #region Fields

        private static readonly Point cursorHotSpot = new Point(16, 20);

        private readonly Cursor crosshairsCursor;

        public static DependencyProperty ProcessNameProperty = DependencyProperty.Register(
            "ProcessName",
            typeof(string),
            typeof(ProcessFinderButton),
            null);

        public static DependencyProperty NotifyerProperty = DependencyProperty.Register(
            "Notifyer",
            typeof(IUserNotifyer),
            typeof(ProcessFinderButton),
            null);

        public IUserNotifyer Notifyer
        {
            get => (IUserNotifyer)GetValue(NotifyerProperty);
            set => SetValue(NotifyerProperty, value);
        }

        #endregion Fields

        #region Constructors

        public ProcessFinderButton()
        {
            InitializeComponent();
            crosshairsCursor = ConvertToCursor(WindowInfoControl, cursorHotSpot);
        }

        #endregion Constructors

        #region Properties

        public string ProcessName
        {
            get => (string)GetValue(ProcessNameProperty);
            set => SetValue(ProcessNameProperty, value);
        }

        public WindowInfoControl WindowInfoControl { get; } = new WindowInfoControl();

        #endregion Properties

        #region Methods

        // https://stackoverflow.com/a/27077188/122048
        private static Cursor ConvertToCursor(UIElement control, Point hotSpot = default)
        {
            // convert FrameworkElement to PNG stream
            using (var pngStream = new MemoryStream())
            {
                control.InvalidateMeasure();
                control.InvalidateArrange();
                control.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                var rect = new Rect(0, 0, control.DesiredSize.Width, control.DesiredSize.Height);

                control.Arrange(rect);
                control.UpdateLayout();

                var rtb = new RenderTargetBitmap((int)control.DesiredSize.Width, (int)control.DesiredSize.Height, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(control);

                var png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(rtb));
                png.Save(pngStream);

                // write cursor header info
                using (var cursorStream = new MemoryStream())
                {
                    cursorStream.Write(new byte[2]
                                       {
                                           0x00,
                                           0x00
                                       }, 0, 2); // ICONDIR: Reserved. Must always be 0.
                    cursorStream.Write(new byte[2]
                                       {
                                           0x02,
                                           0x00
                                       }, 0, 2); // ICONDIR: Specifies image type: 1 for icon (.ICO) image, 2 for cursor (.CUR) image. Other values are invalid
                    cursorStream.Write(new byte[2]
                                       {
                                           0x01,
                                           0x00
                                       }, 0, 2); // ICONDIR: Specifies number of images in the file.
                    cursorStream.Write(new byte[1]
                                       {
                                           (byte)control.DesiredSize.Width
                                       }, 0, 1); // ICONDIRENTRY: Specifies image width in pixels. Can be any number between 0 and 255. Value 0 means image width is 256 pixels.
                    cursorStream.Write(new byte[1]
                                       {
                                           (byte)control.DesiredSize.Height
                                       }, 0, 1); // ICONDIRENTRY: Specifies image height in pixels. Can be any number between 0 and 255. Value 0 means image height is 256 pixels.
                    cursorStream.Write(new byte[1]
                                       {
                                           0x00
                                       }, 0, 1); // ICONDIRENTRY: Specifies number of colors in the color palette. Should be 0 if the image does not use a color palette.
                    cursorStream.Write(new byte[1]
                                       {
                                           0x00
                                       }, 0, 1); // ICONDIRENTRY: Reserved. Should be 0.
                    cursorStream.Write(new byte[2]
                                       {
                                           (byte)hotSpot.X,
                                           0x00
                                       }, 0, 2); // ICONDIRENTRY: Specifies the horizontal coordinates of the hotspot in number of pixels from the left.
                    cursorStream.Write(new byte[2]
                                       {
                                           (byte)hotSpot.Y,
                                           0x00
                                       }, 0, 2); // ICONDIRENTRY: Specifies the vertical coordinates of the hotspot in number of pixels from the top.
                    cursorStream.Write(new byte[4]
                                       {
                                           // ICONDIRENTRY: Specifies the size of the image's data in bytes
                                           (byte)(pngStream.Length & 0x000000FF),
                                           (byte)((pngStream.Length & 0x0000FF00) >> 0x08),
                                           (byte)((pngStream.Length & 0x00FF0000) >> 0x10),
                                           (byte)((pngStream.Length & 0xFF000000) >> 0x18)
                                       }, 0, 4);
                    cursorStream.Write(new byte[4]
                                       {
                                           // ICONDIRENTRY: Specifies the offset of BMP or PNG data from the beginning of the ICO/CUR file
                                           0x16,
                                           0x00,
                                           0x00,
                                           0x00
                                       }, 0, 4);

                    // copy PNG stream to cursor stream
                    pngStream.Seek(0, SeekOrigin.Begin);
                    pngStream.CopyTo(cursorStream);

                    // return cursor stream
                    cursorStream.Seek(0, SeekOrigin.Begin);
                    return new Cursor(cursorStream);
                }
            }
        }

        private void StartTargetsSearch()
        {
            CaptureMouse();
            Keyboard.Focus(btnStartWindowsSearch);
            Cursor = crosshairsCursor;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            try
            {
                base.OnMouseLeftButtonUp(e);
                _iconCrossHair.Visibility = Visibility.Visible;

                var ps = ProcessHelper.GetExecutablePath();
                ProcessName = ps.FileName;

                ReleaseMouseCapture();
                Cursor = null;
            }
            catch (Exception ex) { Notifyer?.NotifyError(ex.Message); }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _iconCrossHair.Visibility = Visibility.Hidden;
            StartTargetsSearch();
            e.Handled = true;

            base.OnPreviewMouseLeftButtonDown(e);
        }

        #endregion Methods
    }
}