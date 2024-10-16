﻿using QRCoder;

namespace ElectricityApp.ViewModels;

[QueryProperty("QrCode", "QrCode")]
public partial class QrCodeViewModel : ObservableObject
{
    [ObservableProperty]
    private ImageSource _qrCode;

    public QrCodeViewModel()
    {
        QrCode = CreateQrCode();
    }

    private static ImageSource CreateQrCode()
    {
        var version = "1.2.10";
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode($@"https://github.com/VitaliiVoitovych/ElectricityApp/releases/download/v{version}/ElectricityApp-v{version}.apk", QRCodeGenerator.ECCLevel.Q);

        var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeAsPngByteArr = qrCode.GetGraphic(20);

        return ImageSource.FromStream(() => new MemoryStream(qrCodeAsPngByteArr));
    }

    [RelayCommand]
    private async Task GoToBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
