using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Timer = System.Timers.Timer;
using WomsAuth.Models;
using Debug = System.Diagnostics.Debug;

namespace WomsAuth.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<AuthModel> auths;

    private Timer timer;

    private Totp totp;

    public MainPageViewModel()
    {
        Dummy();
    }

    private void Dummy()
    {
        var auth = new AuthModel("MY3WEYJTMI4TMNZQMZRTQYJWHE2TCMZYGZRDKZJTG5SWKNRYMIYTMNBYMY3DEOJV",
            "Semen3roda.com Cristover Wurangian", "IIA7");

        Auths = new ObservableCollection<AuthModel>
        {
            auth,
            auth,
            auth,
            auth,
            auth,
            auth
        };

        timer = new Timer
        {
            Interval = 1000
        };
        timer.Start();
        timer.Elapsed += (s, e) =>
        {
            //Debug.WriteLine("Elapsed: " + e.SignalTime);

            Parallel.ForEach(Auths, auth => { auth.UpdateRemainingSeconds(); });
        };
    }
}