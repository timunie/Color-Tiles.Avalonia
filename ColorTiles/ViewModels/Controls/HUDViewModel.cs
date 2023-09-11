using System;
using System.Threading;
using Avalonia;
using ColorTiles.ViewModels.Controls.HUD;
using ReactiveUI;

namespace ColorTiles.ViewModels.Controls;

public class HUDViewModel : ViewModelBase
{
    private int _score = 0;
    private long _penalty = 0;
    private bool _doShow = false;
    private Thickness _buttonPadding;
    private Thickness _textPadding;

    public int Score
    {
        get => _score;
        set => this.RaiseAndSetIfChanged(ref _score, value);
    }

    public long Penalty
    {
        get => _penalty;
        set => this.RaiseAndSetIfChanged(ref _penalty, value);
    }

    public bool DoShowHUD
    {
        get => _doShow;
        set => this.RaiseAndSetIfChanged(ref _doShow, value);
    }

    public Thickness ButtonPadding
    {
        get => _buttonPadding;
        set => this.RaiseAndSetIfChanged(ref _buttonPadding, value);
    }

    public Thickness TextPadding
    {
        get => _textPadding;
        set => this.RaiseAndSetIfChanged(ref _textPadding, value);
    }

    public TimerBarViewModel TimerBar { get; set; }

    public event EventHandler? ResetButtonClicked;

    public HUDViewModel()
    {
        TimerBar = new TimerBarViewModel();
    }

    public HUDViewModel(long initialTimeMilliseconds)
    {
        TimerBar = new TimerBarViewModel(initialTimeMilliseconds);

        TimerBar.TimeExpired += OnTimeExpired;

        if (OperatingSystem.IsAndroid() || OperatingSystem.IsBrowser())
        {
            ButtonPadding = new Thickness(8, 4, 8, 0);
            TextPadding = new Thickness(0);
        }
        else
        {
            ButtonPadding = new Thickness(8, -6);
            TextPadding = new Thickness(0,-8);
        }
    }

    public HUDViewModel(int initialTimeSeconds) : this((long)initialTimeSeconds * 1000) 
    { 
    }

    public HUDViewModel(long initialTimeMilliseconds, long penalty) : this(initialTimeMilliseconds)
    {
        Penalty = penalty;
    }

    public HUDViewModel(int initialTimeSeconds, long penalty) : this((long)initialTimeSeconds * 1000, penalty)
    {
    }

    public void OnResetButtonClicked()
    {
        Score = 0;
        TimerBar.ResetTimer();

        DoShowHUD = false;

        ResetButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void OnMatchFround(object? sender, int score)
    {
        Score += score;
    }

    public void OnPenalty(object? sender, EventArgs e)
    {
        TimerBar.RemainingTime -= Penalty;
    }

    public void OnTimeExpired(object? sender, EventArgs e)
    {
        DoShowHUD = false;
    }
}