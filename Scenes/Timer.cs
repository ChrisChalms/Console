#pragma warning disable 649

using System;
using UnityEngine;
using UnityEngine.UI;
using CC.Console;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text _timeText;

    private TimeSpan _time;
    private TimeSpan _timerIncrementAmount = new TimeSpan(0, 0, 1);
    private bool _isTicking;

    #region MonoBehaviour

    // Initialize
    private void Awake()
    {
        ConsoleCommands.AddCommand("Timer.Start", timerStart, "Starts the timer");
        ConsoleCommands.AddCommand("Timer.Pause", timerPause, "Pauses the timer");
        ConsoleCommands.AddCommand("Timer.Stop", timerStop, "Stops the timer");
        ConsoleCommands.AddCommand("Timer.TickBy", tickBy, "Changes how much the timer ticks by");
        ConsoleCommands.AddCommand("Disco", disco, "Disco Mode!");
        ConsoleCommands.AddCommand("Disco.Stop", discoStop, "Stops discomode");
    }

    #endregion

    #region Timer Controls

    // Start the timer
    private void timerStart(string[] args)
    {
        if (IsInvoking("tick"))
        {
            Debug.LogWarning("The timer's already going! This is a warning");
            return;
        }

        InvokeRepeating("tick", 0.01f, 1f);
        _isTicking = true;
    }

    // Pause the timer
    private void timerPause(string[] args)
    {
        if (!_isTicking)
        {
            Debug.LogError("The timer hasn't been started! This is an error");
            return;
        }

        CancelInvoke("tick");
        _isTicking = false;
    }

    // Stops the timer
    private void timerStop(string[] args)
    {
        if (!_isTicking)
        {
            Debug.Log("The timer hasn't been started!");
            return;
        }

        CancelInvoke("tick");
        _isTicking = false;
        _time = new TimeSpan();
        updateUI();
    }

    // Change how much the timer ticks by
    private void tickBy(string[] args)
    {
        if(args.Length != 1)
        {
            Debug.LogError($"Trying to change the timer's tick, received {args.Length} arguments, expected 1");
            return;
        }

        var newSeconds = 0;
        if (!int.TryParse(args[0], out newSeconds))
            Debug.LogError("The argument passed wasn't valid");
        else
        {
            _timerIncrementAmount = new TimeSpan(0, 0, newSeconds);
            Debug.Log($"The tick amount has been changed to {_timerIncrementAmount}");
        }
    }

    // Tick the timer
    private void tick()
    {
        _time = _time.Add(_timerIncrementAmount);
        updateUI();
    }

    // Update the text
    private void updateUI() => _timeText.text = _time.ToString(@"hh\:mm\:ss");

    #endregion

    // Starts the disco mode
    private void disco(string[] args) => InvokeRepeating("changeColour", 0.1f, 1);
    private void discoStop(string[] args) => Debug.Log("You can't stop the disco!");

    // Changes the colour of the text to something random
    private void changeColour() => _timeText.color = UnityEngine.Random.ColorHSV(0, 1);
}
