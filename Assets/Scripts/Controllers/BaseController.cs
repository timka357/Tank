using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected bool Enable {get;set;}

    public virtual void On()
    {
        Enable = true;
    }

    public virtual void Off()
    {
        Enable = false;
    }

    public virtual void Dispose() { }

    public virtual void Init() { }

    public virtual void Pause() { }
    public virtual void UnPause() { }
}
