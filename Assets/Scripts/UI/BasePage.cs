using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePage : MonoBehaviour
{
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Dispose() {}

    public virtual void Init() { }

    public virtual void Pause() { }

    public virtual void UnPause() { }
}