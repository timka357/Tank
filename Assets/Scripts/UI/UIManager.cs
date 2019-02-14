using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : BasePage
{
    private FixedJoystick _bodyJoystick, _turretJoystick;

    private Button _fireButton, _changeWeaponButton;
    private Action fireAction, changeWeaponAction;

    private void Awake()
    {
        _bodyJoystick = transform.Find("BodyJoystick").GetComponent<FixedJoystick>();
        _turretJoystick = transform.Find("TurretJoystick").GetComponent<FixedJoystick>();

        _fireButton = transform.Find("FireButton").GetComponent<Button>();
        _changeWeaponButton = transform.Find("ChangeWeaponButton").GetComponent<Button>();
    }

    public override void Init()
    {
        base.Init();
        _fireButton.onClick.AddListener(FireClick);
        _changeWeaponButton.onClick.AddListener(ChangeWeaponClick);

        TankController Tank = Main.Instance.GetController<TankController>();
        changeWeaponAction += Tank.ChangeWeapon;
        fireAction += Tank.Fire;
    }

    public float GetHorizontalAxis()
    {
        return _bodyJoystick.Horizontal;
    }

    public float GetVerticalAxis()
    {
        return _bodyJoystick.Vertical;
    }

    public float GetTurretRotateAxis()
    {
        return _turretJoystick.Horizontal;
    }

    public override void Dispose()
    {
        base.Dispose();

        foreach (var funk in changeWeaponAction.GetInvocationList())
        {
            changeWeaponAction -= (Action)funk;
        }

        foreach (var funk in fireAction.GetInvocationList())
        {
            fireAction -= (Action)funk;
        }
    }

    private void ChangeWeaponClick()
    {
        if (changeWeaponAction != null)
            changeWeaponAction();
    }

    private void FireClick()
    {
        if (fireAction != null)
            fireAction();
    }

}
