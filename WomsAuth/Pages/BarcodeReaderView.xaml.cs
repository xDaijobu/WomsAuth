using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web;
using WomsAuth.Models;
using ZXing.Net.Maui;

namespace WomsAuth.Pages;

public partial class BarcodeReaderView
{
    private AuthModel authModel;
    private TaskCompletionSource<AuthModel> tcsAuth;
	
    public BarcodeReaderView(TaskCompletionSource<AuthModel> tcsAuth)
	{
		InitializeComponent();
        this.tcsAuth = tcsAuth;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        tcsAuth.SetResult(authModel);
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        Debug.WriteLine($"CRIS: {e.Results[0].Format}->{e.Results[0].Format}");
        barcodeView.IsDetecting = false;

        authModel = DecodePayload(e.Results[0].Value);

        Navigation.PopAsync();
    }

    public AuthModel DecodePayload(string payload)
    {
        try
        {
            var uri = new Uri(payload);
            if (uri.Scheme == "otpauth")
            {
                var queryString = HttpUtility.ParseQueryString(uri.Query);
                var secret = queryString["secret"];
                var account = uri.LocalPath.StartsWith("/") ? uri.LocalPath.Substring(1) : uri.LocalPath;
                var issuer = queryString["issuer"];
                //var title = account.Split(":").FirstOrDefault();
                var description = account.Split(":").LastOrDefault();
                var auth = new AuthModel(secret, issuer, description);

                return auth;
            }
        }
        catch (Exception e)
        {
            return null;
        }

        return null;
    }
}