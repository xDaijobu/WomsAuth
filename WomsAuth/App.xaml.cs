namespace WomsAuth;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTAyNzY5NUAzMjMwMmUzNDJlMzBMMk5oQnJodGZweGlCcUh2RHBEdWJxT2w0NGozWkIzNzdQTlhQSzYrVnJVPQ==");
        MainPage = new AppShell();
	}
}
