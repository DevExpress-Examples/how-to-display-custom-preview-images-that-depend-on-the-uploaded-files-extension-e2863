using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using DevExpress.Xpf.Controls;

namespace UploadControl_CustomPreviewImages {
    public partial class MainPage : UserControl {
        public MainPage() {
            InitializeComponent();
        }
        private void uploadControl_FileUploadCompleted(object sender, UploadItemEventArgs e) {
            ShowPreview(e.ItemInfo);
        }
        
        // Identifies the file extension to show the appropriate preview image.
        void ShowPreview(ItemInfo itemInfo) {
            string fileExtension = Path.GetExtension(itemInfo.Name.ToLower());
            if (fileExtension == ".pdf")
                ShowPredefinedPdfPreview(itemInfo);
            else if (fileExtension == ".doc")
                ShowPredefinedOfficePreview(itemInfo);
            else {
                Regex regex = 
                    new Regex(@"(\.jpg|\.jpeg|\.png|\.bmp|\.tif|\.tiff|\.gif)", 
                              RegexOptions.IgnoreCase);
                if (regex.IsMatch(fileExtension))
                    ShowImagePreview(itemInfo);
            }
        }
        
        // Shows a preview for image files.
        void ShowImagePreview(ItemInfo itemInfo) {
            Show(itemInfo, GetThumbnail(itemInfo, 100));
        }

        // Shows a preview for PDF files.
        void ShowPredefinedPdfPreview(ItemInfo itemInfo) {
            Show(itemInfo, GetImageFromContent("Images/pdf_icon.jpg"));
        }

        // Shows a preview for MS Office files.
        void ShowPredefinedOfficePreview(ItemInfo itemInfo) {
            Show(itemInfo, GetImageFromContent("Images/office_icon.jpg"));
        }

        // Shows the specified preview image within the specified upload item.
        void Show(ItemInfo itemInfo, ImageSource source) {
            uploadControl.ShowPreviewImage(itemInfo, source);
        }

        // Scales the specified image to the specified width.
        ImageSource GetThumbnail(ItemInfo itemInfo, int width) {
            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(itemInfo.Stream);
            Image image = new Image();
            image.Source = bitmap;

            double cx = width;
            double cy = bitmap.PixelHeight * (cx / bitmap.PixelWidth);

            double scaleX = cx / bitmap.PixelWidth;
            double scaleY = cy / bitmap.PixelHeight;

            WriteableBitmap thumb = new WriteableBitmap((int)cx, (int)cy);
            thumb.Render(image, new ScaleTransform() { ScaleX = scaleX, ScaleY = scaleY });
            thumb.Invalidate();

            WriteableBitmap thumbCopy = new WriteableBitmap((int)cx, (int)cy);
            for (int i = 0; i < thumbCopy.Pixels.Length; i++)
                thumbCopy.Pixels[i] = thumb.Pixels[i];
            thumbCopy.Invalidate();

            return thumbCopy;
        }

        // Opens the specified image file to show its content.
        ImageSource GetImageFromContent(string name) {
            StreamResourceInfo info = 
                Application.GetResourceStream(new Uri(name, UriKind.Relative));
            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(info.Stream);
            return bitmap;
        }
    }
}
