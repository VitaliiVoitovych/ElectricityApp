using CommunityToolkit.Maui.Views;

namespace ElectricityApp.Views.Popups;

public partial class QrCodePopup : Popup
{
    
    public QrCodePopup(ImageSource qrCode)
	{
        InitializeComponent();
        QrCodeImage.Source = qrCode;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await ContentBorder.TranslateTo(0, 500, 250, Easing.CubicOut);
        Close();
    }

    private async void Content_Loaded(object sender, EventArgs e)
    {
        await Task.Delay(250);
        await ContentBorder.TranslateTo(0, 0, 250, Easing.CubicIn);
    }
}