using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAmmo : Ammo
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<IHaveHealth>().GetDamage(_damage);
            Deactivate();
        }
    }

    protected override void Deactivate()
    {
        //Effect of bomb
        //Add force to other rigidbody
        base.Deactivate();
    }
}
