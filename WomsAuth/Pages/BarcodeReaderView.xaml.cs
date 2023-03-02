using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web;
using WomsAuth.Models;
using ZXing.Net.Maui;

namespace WomsAuth.Pages;

public partial class BarcodeReaderView
{
    private TaskCompletionSource<AuthModel> tcsAuth;
	public BarcodeReaderView(TaskCompletionSource<AuthModel> tcsAuth)
	{
		InitializeComponent();
        this.tcsAuth = tcsAuth;
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        Debug.WriteLine($"CRIS: {e.Results[0].Format}->{e.Results[0].Format}");
        barcodeView.IsDetecting = false;

        var auth = DecodePayload(e.Results[0].Value);

        tcsAuth.SetResult(auth);
        Navigation.PopModalAsync();
    }

    public AuthModel DecodePayload(string payload)
    {
        var uri = new Uri(payload);
        if (uri.Scheme == "otpauth")
        {
            var queryString = HttpUtility.ParseQueryString(uri.Query);
            var secret = queryString["secret"];
            var account = uri.LocalPath.StartsWith("/") ? uri.LocalPath.Substring(1) : uri.LocalPath;

            var title = account.Split(":").FirstOrDefault();
            var description = account.Split(":").LastOrDefault();
            var auth = new AuthModel(secret, title, description);

            return auth;
        }

        return null;
    }
}