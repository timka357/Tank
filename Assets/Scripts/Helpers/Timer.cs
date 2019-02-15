using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float _elapsed;
    private float _temp;

    public bool IsActive
    {
        get;set;
    }

    public bool TimeOver
    {
        get; private set;
    }

    public float GetCurrTime
    {
        get { return _temp; }
    }

    public void Update()
    {
        if (IsActive && !TimeOver)
        {
            if(_temp<_elapsed)
            {
                _temp += Time.deltaTime;
            }
            if(_temp>=_elapsed)
            {
                TimeOver = true;
            }
        }
    }

    public void On()
    {
        IsActive = true;
        ResetTimer();
    }

    public void Off()
    {
        IsActive = false;
    }

    public void ResetTimer()
    {
        _temp = 0;
        TimeOver = false;
    }

    public void InitTimer(float elapsed)
    {
        Off();
        _elapsed = elapsed;
    }
}
