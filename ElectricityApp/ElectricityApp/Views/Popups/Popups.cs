using ElectricityApp.Services.QrCode;

namespace ElectricityApp.Views.Popups;

public static class Popups
{
    public static QrCodePopup ShareAppQrCodePopup => new("Поділитися застосунком", QrCodes.ShareAppQrCode);
}
