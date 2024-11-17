using ElectricityApp.Services.QrCode;

namespace ElectricityApp.Views.Popups;

public static class Popups
{
    public static QrCodePopup GetShareAppQrCodePopup()
    {
        var qrCodePopup = new QrCodePopup("Поділитися застосунком", QrCodes.ShareAppQrCode);
        return qrCodePopup;
    }
}
