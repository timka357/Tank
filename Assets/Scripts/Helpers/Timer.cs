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

    public void Update()
    {
        if (IsActive && !TimeOver)
        {
            if(_temp>0)
            {
                _temp -= Time.deltaTime;
            }
            if(_temp<= 0)
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
        _temp = _elapsed;
        TimeOver = false;
    }

    public void InitTimer(float elapsed)
    {
        Off();
        _elapsed = elapsed;
    }
}
