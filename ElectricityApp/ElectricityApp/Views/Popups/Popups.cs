using ElectricityApp.Services.QrCode;

namespace ElectricityApp.Views.Popups;

public static class Popups
{
    public static QrCodePopup GetShareAppQrCodePopup()
    {
        var qrCodePopup = new QrCodePopup(QrCodes.ShareAppQrCode);
        return qrCodePopup;
    }
}
