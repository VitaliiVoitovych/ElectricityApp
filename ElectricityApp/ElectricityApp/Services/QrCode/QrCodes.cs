namespace ElectricityApp.Services.QrCode;

public class QrCodes
{
    private const string Version = "1.2.13";
    private const string AppDownloadUrl = $@"https://github.com/VitaliiVoitovych/ElectricityApp/releases/download/v{Version}/ElectricityApp-v{Version}.apk";


    public static readonly ImageSource ShareAppQrCode = QrCodeService.GenerateQrCode(AppDownloadUrl);
}