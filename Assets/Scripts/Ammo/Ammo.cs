using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ammo : MonoBehaviour
{
    [SerializeField]
    protected int _damage;
    [SerializeField]
    protected float _lifeTime;
    protected Timer _lifeTimer;
    protected Rigidbody _rigidbody;
    public Action<Ammo> returnAmmoToPool;

    public float LifeTime { get { return _lifeTime; } }
    public Rigidbody GetRigidbody { get { return _rigidbody; } }
    public int Damage { get { return _damage; } }

    protected virtual void Awake()
    {
        _lifeTimer = new Timer();
        _lifeTimer.InitTimer(_lifeTime);
        _rigidbody = transform.GetComponent<Rigidbody>();
    }

    protected void OnEnable()
    {
        _lifeTimer.On();
    }

    protected void OnDisable()
    {
        _lifeTimer.Off();
    }

    protected virtual void Update()
    {
        if (isActiveAndEnabled)
        {
            _lifeTimer.Update();
            if (_lifeTimer.TimeOver)
            {
                Deactivate();
            }
        }
    }

    protected virtual void Deactivate()
    {
        _rigidbody.velocity = Vector3.zero;
        if (returnAmmoToPool != null) returnAmmoToPool(this);
        else Destroy(gameObject);
    }
}
