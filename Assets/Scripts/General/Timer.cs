using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Timer
{
    public bool isFinished;
    public delegate void TimerFinishedDelegate();
    public event TimerFinishedDelegate timerFinishedDelegate;

    private float timer = 0;
    
    /// <summary>
    /// Start timer (Call by other script)
    /// </summary>
    /// <param name="duration">timer duration</param>
    public void StartTimer(float duration)
    {
        timer = 0;
        
        TimerAction(duration);
    }

    private async void TimerAction(float duration)
    {
        isFinished = false;
        await Task.Delay((int) (duration * 1000));
        
        // Execute delegate 
        timerFinishedDelegate!();
        isFinished = true;
    }
}
