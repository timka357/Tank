using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartGun : Weapon
{
    protected override void Start()
    {
        base.Start();
    }

    public override void Fire()
    {
        if (IsCurrentGun)
        {
            _bulletPool.GetObjectFromPool().GetRigidbody.AddForce(SpawnPosition.forward * Force, ForceMode.Impulse);
            //Add Efect
        }
    }


}
