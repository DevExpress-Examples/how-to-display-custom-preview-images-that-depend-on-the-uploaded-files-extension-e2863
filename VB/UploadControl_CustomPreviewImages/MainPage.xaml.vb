Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Resources
Imports DevExpress.Xpf.Controls

Namespace UploadControl_CustomPreviewImages
	Partial Public Class MainPage
		Inherits UserControl
		Public Sub New()
			InitializeComponent()
		End Sub
		Private Sub uploadControl_FileUploadCompleted(ByVal sender As Object, _
				ByVal e As UploadItemEventArgs)
			ShowPreview(e.ItemInfo)
		End Sub

		' Identifies the file extension to show the appropriate preview image.
		Private Sub ShowPreview(ByVal itemInfo As ItemInfo)
			Dim fileExtension As String = Path.GetExtension(itemInfo.Name.ToLower())
			If fileExtension = ".pdf" Then
				ShowPredefinedPdfPreview(itemInfo)
			ElseIf fileExtension = ".doc" Then
				ShowPredefinedOfficePreview(itemInfo)
			Else
				Dim regex As New Regex("(\.jpg|\.jpeg|\.png|\.bmp|\.tif|\.tiff|\.gif)", _
					RegexOptions.IgnoreCase)
				If regex.IsMatch(fileExtension) Then
					ShowImagePreview(itemInfo)
				End If
			End If
		End Sub

		' Shows a preview for image files.
		Private Sub ShowImagePreview(ByVal itemInfo As ItemInfo)
			Show(itemInfo, GetThumbnail(itemInfo, 100))
		End Sub

		' Shows a preview for PDF files.
		Private Sub ShowPredefinedPdfPreview(ByVal itemInfo As ItemInfo)
			Show(itemInfo, GetImageFromContent("Images/pdf_icon.jpg"))
		End Sub

		' Shows a preview for MS Office files.
		Private Sub ShowPredefinedOfficePreview(ByVal itemInfo As ItemInfo)
			Show(itemInfo, GetImageFromContent("Images/office_icon.jpg"))
		End Sub

		' Shows the specified preview image within the specified upload item.
		Private Sub Show(ByVal itemInfo As ItemInfo, ByVal source As ImageSource)
			uploadControl.ShowPreviewImage(itemInfo, source)
		End Sub

		' Scales the specified image to the specified width.
		Private Function GetThumbnail(ByVal itemInfo As ItemInfo, _
				ByVal width As Integer) As ImageSource
			Dim bitmap As New BitmapImage()
			bitmap.SetSource(itemInfo.Stream)
			Dim image As New Image()
			image.Source = bitmap

			Dim cx As Double = width
			Dim cy As Double = bitmap.PixelHeight * (cx / bitmap.PixelWidth)

			Dim scaleX As Double = cx / bitmap.PixelWidth
			Dim scaleY As Double = cy / bitmap.PixelHeight

			Dim thumb As New WriteableBitmap(CInt(Fix(cx)), CInt(Fix(cy)))
			thumb.Render(image, New ScaleTransform() With {.ScaleX = scaleX, .ScaleY = scaleY})
			thumb.Invalidate()

			Dim thumbCopy As New WriteableBitmap(CInt(Fix(cx)), CInt(Fix(cy)))
			For i As Integer = 0 To thumbCopy.Pixels.Length - 1
				thumbCopy.Pixels(i) = thumb.Pixels(i)
			Next i
			thumbCopy.Invalidate()

			Return thumbCopy
		End Function

		' Opens the specified image file to show its content.
		Private Function GetImageFromContent(ByVal name As String) As ImageSource
			Dim info As StreamResourceInfo = _
				Application.GetResourceStream(New Uri(name, UriKind.Relative))
			Dim bitmap As New BitmapImage()
			bitmap.SetSource(info.Stream)
			Return bitmap
		End Function
	End Class
End Namespace
