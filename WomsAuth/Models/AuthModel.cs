using OtpNet;
using SQLite;

namespace WomsAuth.Models;

[INotifyPropertyChanged]
public partial class AuthModel
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SecreyKey { get; set; }
    private Totp totp;

    public AuthModel()
    {
    }
    public AuthModel(string secretKey, string title, string description)
    {
        Title = title;
        Description = description;
        SecreyKey = secretKey;
        totp = new Totp(Base32Encoding.ToBytes(SecreyKey));
        RemainingSeconds = totp.RemainingSeconds();
        UpdateCode();
    }

    public string Title { get; set; }
    public string Description { get; set; }
    [Ignore]
    public string Code { get; private set; }

    [Ignore]
    public int RemainingSeconds { get; private set; }
    [Ignore]
    public float CurrentProgress { get; private set; }
    [Ignore]
    public int DefaultStep { get; } = 30;

    

    public void UpdateRemainingSeconds()
    {
        //System.Diagnostics.Debug.WriteLine("UpdateRemainingSeconds FIRE");
        ShouldInitTOTP();
        //System.Diagnostics.Debug.WriteLine("Timestmap: " + CurrentTimestamp);
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
        //System.Diagnostics.Debug.WriteLine("UpdateCode FIRE");
        ShouldInitTOTP();

        Code = totp.ComputeTotp();
        OnPropertyChanged(nameof(Code));
    }

    //private static TimeCorrection Correction = new TimeCorrection(DateTime.UtcNow);
    private void ShouldInitTOTP()
    {
        if (totp is null)
        {
            totp = new Totp(Base32Encoding.ToBytes(SecreyKey));//, timeCorrection: Correction);

            Code = totp.ComputeTotp();
            OnPropertyChanged(nameof(Code));
        }
    }
}