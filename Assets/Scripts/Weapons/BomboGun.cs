using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomboGun : Weapon
{
    [SerializeField]
    private float yAngle = 35;
    protected override void Start()
    {
        base.Start();
    }

    public override void Fire()
    {
        if (IsCurrentGun)
        {
            Vector3 forceVector = Quaternion.Euler(-yAngle, 0,0)*SpawnPosition.forward;
            _bulletPool.GetObjectFromPool().GetRigidbody.AddForce(forceVector * Force, ForceMode.Impulse);
        }
    }
}
