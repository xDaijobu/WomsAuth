using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using WomsAuth.Database;
using Timer = System.Timers.Timer;
using WomsAuth.Models;
using WomsAuth.Pages;
using Debug = System.Diagnostics.Debug;

namespace WomsAuth.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<AuthModel> auths;

    private AuthDatabase authDatabase = new AuthDatabase();
    private Timer timer;

    private Totp totp;

    public MainPageViewModel()
    {
        App.Current.MainPage.Dispatcher.Dispatch(async () =>
        {
            await GetAuths();

            InitTimer();
        });
    }

    [RelayCommand]
    private async Task AddClicked()
    {
        //var auth = new AuthModel("MY3WEYJTMI4TMNZQMZRTQYJWHE2TCMZYGZRDKZJTG5SWKNRYMIYTMNBYMY3DEOJV",
        //    "Semen3roda.com Cristover Wurangian", "IIA7");
        //
        //Auths.Add(auth);

        TaskCompletionSource<AuthModel> tcsAuth = new TaskCompletionSource<AuthModel>();
        await App.Current.MainPage.Navigation.PushModalAsync(new BarcodeReaderView(tcsAuth));

        var auth = await tcsAuth.Task;

        if (auth != null)
        {
            Auths.Add(auth);

            await authDatabase.SaveItemAsync(auth);
        }
    }

    [RelayCommand]
    private async Task DeleteAuth(AuthModel auth)
    {
        await authDatabase.DeleteItemAsync(auth);

        Auths.Remove(Auths.FirstOrDefault(i => i.Id == auth.Id));
    }

    private async Task GetAuths()
    {
        var auths = await authDatabase.GetItemsAsync();
        Auths = new ObservableCollection<AuthModel>(auths);
    }


    private void InitTimer()
    {
        timer = new Timer
        {
            Interval = 1000
        };
        timer.Start();
        timer.Elapsed += (s, e) =>
        {
            Debug.WriteLine("Elapsed: " + e.SignalTime);

            _ = Task.Run(() =>
            {
                Parallel.ForEach(Auths, auth => { auth.UpdateRemainingSeconds(); });
            });
        };
    }
    
}