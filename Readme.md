<!-- default file list -->
*Files to look at*:

* [MainPage.xaml](./CS/UploadControl_CustomPreviewImages/MainPage.xaml) (VB: [MainPage.xaml.vb](./VB/UploadControl_CustomPreviewImages/MainPage.xaml.vb))
* [MainPage.xaml.cs](./CS/UploadControl_CustomPreviewImages/MainPage.xaml.cs) (VB: [MainPage.xaml.vb](./VB/UploadControl_CustomPreviewImages/MainPage.xaml.vb))
<!-- default file list end -->
# How to display custom preview images that depend on the uploaded file's extension


<p>The following example demonstrates how to show custom preview images that depend on the uploaded file's extension.</p><p>In this example, the FileUploadCompleted event is handled to display preview images. For DOC and PDF files, the upload control displays MS Office and Adobe Reader logos within upload items, respectively. When image files are uploaded, a preview thumbnail is generated from the file's content. For other file types, no preview is displayed.</p>

<br/>


