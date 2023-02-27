using OtpNet;

namespace WomsAuth.Models;

[INotifyPropertyChanged]
public partial class AuthModel
{
    private readonly string secretKey;
    private readonly Totp totp;


    public AuthModel(string secretKey, string title, string description)
    {
        Title = title;
        Description = description;
        this.secretKey = secretKey;
        totp = new Totp(Base32Encoding.ToBytes(this.secretKey));
        RemainingSeconds = totp.RemainingSeconds();
        UpdateCode();
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Code { get; private set; }
    public int RemainingSeconds { get; private set; }
    public float CurrentProgress { get; private set; }
    public int DefaultStep { get; } = 30;

    public void UpdateRemainingSeconds()
    {
        RemainingSeconds = totp.RemainingSeconds();
        OnPropertyChanged(nameof(RemainingSeconds));
        //System.Diagnostics.Debug.WriteLine(RemainingSeconds);
        if (RemainingSeconds == 0)
            UpdateCode();

        CurrentProgress = (float)Math.Round(RemainingSeconds / (float)DefaultStep, 1);
        //System.Diagnostics.Debug.WriteLine(CurrentProgress);
        OnPropertyChanged(nameof(CurrentProgress));
    }

    private void UpdateCode()
    {
        Code = totp.ComputeTotp();
        OnPropertyChanged(nameof(Code));
    }
}