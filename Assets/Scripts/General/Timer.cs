using System;
using Cysharp.Threading.Tasks;
#pragma warning disable CS4014

public class Timer
{
    public bool isFinished;
    public event Action timerStartEvent;
    public event Action timerFinishEvent;
    
    private void CallTimerStartEvent()
    {
        timerStartEvent?.Invoke();
    }
    private void CallTimerFinishEvent()
    {
        timerFinishEvent?.Invoke();
    }

    /// <summary>
    /// Start timer (Call by other script)
    /// </summary>
    /// <param name="duration">timer duration</param>
    public void StartTimer(float duration)
    {
        TimerAction(duration);
    }
    
    private async UniTask TimerAction(float duration)
    {
        isFinished = false;
        
        CallTimerStartEvent();
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        CallTimerFinishEvent();
        
        isFinished = true;
    }
}
