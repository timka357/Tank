using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected float _force;

    [SerializeField]
    protected float _rechargeTime;

    [SerializeField]
    protected GameObject _ammo;

    [SerializeField]
    protected ParticleSystem fireEffect;

    [SerializeField]
    protected int _countOfBulletInPool;

    protected Transform _spawnTransform, _ammoTransform;
    protected ObjectPool<Ammo> _bulletPool;
    protected bool _isCurrentGun;

    #region Property
    public float Force
    {
        get { return _force; }
    }

    public bool IsCurrentGun
    {
        get { return _isCurrentGun; }
    }

    public Transform SpawnPosition
    {
        get { return _spawnTransform; }
    }

    public float RechargeTime
    {
        get { return _rechargeTime; }
    }

    public ObjectPool<Ammo> ObjectPool
    {
        get { return _bulletPool; }
    }
    #endregion

    protected virtual void Start()
    {
        _spawnTransform = transform.Find("SpawnObj");
        _ammoTransform = GameObject.Find("Ammo").transform;
        _bulletPool = new ObjectPool<Ammo>();
        if (_ammo != null) InitPool();
        fireEffect = Instantiate(fireEffect, transform);
        fireEffect.transform.position = _spawnTransform.position;
    }

    public void InitPool()
    {
        if (_countOfBulletInPool == 0) _countOfBulletInPool = Mathf.CeilToInt(_ammo.GetComponent<Ammo>().LifeTime / _rechargeTime);
        _bulletPool.InitializePool(_ammo.GetComponent<Ammo>(), _ammoTransform, _countOfBulletInPool, _spawnTransform);
        foreach (var item in _bulletPool.PoolObjects)
        {
            item.returnAmmoToPool += _bulletPool.ReturnObjectToPool;
        }
    }

    public virtual void Fire()
    {
        if (fireEffect != null)
        {
            fireEffect.Play();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _isCurrentGun = false;
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
        _isCurrentGun = true;
    }
}
