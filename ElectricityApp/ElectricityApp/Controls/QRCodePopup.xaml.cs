using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Views;
using QRCoder;

namespace ElectricityApp.Controls;

public partial class QRCodePopup : Popup
{
	public QRCodePopup()
	{
        var qrCodeByteArray = CreateQrCode();
        var imageSource = ImageSource.FromStream(() => new MemoryStream(qrCodeByteArray));

        InitializeComponent();
		QrImage.Source = imageSource;
	}

	private static byte[] CreateQrCode()
	{
		var qrGenerator = new QRCodeGenerator();
		var qrCodeData = qrGenerator.CreateQrCode(@"https://github.com/VitaliiVoitovych/ElectricityApp/releases/download/v1.0/ElectricityApp-v1.0.apk", QRCodeGenerator.ECCLevel.Q);

        var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeAsPngByteArr = qrCode.GetGraphic(20);

		return qrCodeAsPngByteArr;
    }

    private async void OkButtonClicked(object sender, EventArgs e)
    {
		var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
		await CloseAsync(cts.Token);
    }
}