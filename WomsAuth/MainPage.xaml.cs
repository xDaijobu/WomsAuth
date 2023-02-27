using System.Diagnostics;
using WomsAuth.ViewModels;

namespace WomsAuth;

public partial class MainPage
{
    private readonly int _duration = 30;
    private readonly ProgressArc _progressArc;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private double _progress;
    private readonly DateTime _startTime;
    public MainPage()
    {
        InitializeComponent();

        BindingContext = new MainPageViewModel();
        _progressArc = new ProgressArc();

        _startTime = DateTime.Now;
        _cancellationTokenSource = new CancellationTokenSource();
        UpdateArc();
    }

    private async void UpdateArc()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            var elapsedTime = DateTime.Now - _startTime;
            var secondsRemaining = (int)(_duration - elapsedTime.TotalSeconds);

            ProgressButton.Text = $"{secondsRemaining}";

            _progress = Math.Ceiling(elapsedTime.TotalSeconds);
            _progress %= _duration;
            _progressArc.Progress = _progress / _duration;
            ProgressView.Invalidate();

            if (secondsRemaining == 0)
            {
                _cancellationTokenSource.Cancel();
                return;
            }

            await Task.Delay(500);
        }

        ResetView();
    }

    private void ResetView()
    {
        _progress = 0;
        _progressArc.Progress = 100;
        ProgressView.Invalidate();
    }
}


public class ProgressArc : IDrawable
{
    public double Progress { get; set; } = 100;
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // Angle of the arc in degrees
        var endAngle = 90 - (int)Math.Round(Progress * 360, MidpointRounding.AwayFromZero);
        // Drawing code goes here
        // canvas.StrokeColor = Color.FromRgba("6599ff");
        canvas.StrokeColor = Color.FromRgba("6599ff");
        canvas.StrokeSize = 4;
        Debug.WriteLine($"The rect width is {dirtyRect.Width} and height is {dirtyRect.Height}");
        canvas.DrawArc(5, 5, (dirtyRect.Width - 10), (dirtyRect.Height - 10), 90, endAngle, false, false);
    }
}

