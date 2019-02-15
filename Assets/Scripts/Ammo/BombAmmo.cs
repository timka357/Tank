using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAmmo : Ammo
{
    [SerializeField]
    private ParticleSystem _effect;
    [SerializeField]
    private float _radius = 5f;

    private bool _isWasPlaing;

    protected override void Awake()
    {
        base.Awake();
        _effect = Instantiate(_effect, transform);
    }

    override protected void OnEnable()
    {
        base.OnEnable();
        _isWasPlaing = false;
    }

    protected override void Update()
    {
        base.Update();
        if(_lifeTime-_lifeTimer.GetCurrTime<0.5f && !_isWasPlaing)
        {
            _isWasPlaing = true;
            if (_effect != null) _effect.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Deactivate();
        }
    }

    protected override void Deactivate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
        foreach(Collider item in colliders)
        {
            IHaveHealth temp = item.GetComponent<IHaveHealth>();
            if (temp != null) temp.GetDamage(_damage);
        }

        base.Deactivate();
    }
}
