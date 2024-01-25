using System;
using Cysharp.Threading.Tasks;
#pragma warning disable CS4014

public class Timer
{
    public bool isFinished;
    public event Action timerFinishedEvent;

    private void CallTimerFinishedEvent()
    {
        timerFinishedEvent?.Invoke();
    }

    /// <summary>
    /// Start timer (Call by other script)
    /// </summary>
    /// <param name="duration">timer duration</param>
    public void StartTimer(float duration)
    {
        TimerAction(duration);
    }

    
    /// <summary>
    /// Timer Action
    /// </summary>
    /// <param name="duration"></param>
    private async UniTask TimerAction(float duration)
    {
        isFinished = false;
        await UniTask.Delay(TimeSpan.FromSeconds(duration));

        // Execute delegate 
        CallTimerFinishedEvent();
        isFinished = true;
    }
}
