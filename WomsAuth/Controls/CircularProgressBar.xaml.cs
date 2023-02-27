namespace WomsAuth.Controls;

public partial class CircularProgressBar
{
    public static readonly BindableProperty DurationProperty =
        BindableProperty.Create(nameof(Duration), typeof(int), typeof(CircularProgressBar), 0, propertyChanged: OnDurationChanged);

    public int Duration
    {
        get { return (int)GetValue(DurationProperty); }
        set { SetValue(DurationProperty, value); }
    }

    private DateTime _startTime;

    private ProgressArc _progressArc;
    private CancellationTokenSource _cancellationTokenSource = new();
    private double _progress;

    static void OnDurationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CircularProgressBar)bindable;
        control._startTime = DateTime.Now;
        control._cancellationTokenSource = new CancellationTokenSource();
        control.UpdateArc();
    }

    public CircularProgressBar()
    {
        InitializeComponent();

        _progressArc = new ProgressArc();
        ProgressView.Drawable = _progressArc;
    }
    
    private async void UpdateArc()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            var elapsedTime = DateTime.Now - _startTime;
            var secondsRemaining = (int)(Duration - elapsedTime.TotalSeconds);

            Dispatcher.Dispatch(() =>
            {
                _progress = Math.Ceiling(elapsedTime.TotalSeconds);
                _progress %= Duration;
                _progressArc.Progress = _progress / Duration;

                ProgressView.Invalidate();
            });

            if (secondsRemaining == 0)
            {
                _startTime = DateTime.Now;
                ResetView();
                //_cancellationTokenSource.Cancel();
                //return;
            }

            await Task.Delay(500);
        }

    }

    private void ResetView()
    {
        _progress = 0;
        _progressArc.Progress = 100;
        ProgressView.Invalidate();
    }
}