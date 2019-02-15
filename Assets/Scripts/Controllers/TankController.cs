using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : BaseController, IHaveHealth
{
    [SerializeField]
    private int StartHealth;

    [SerializeField]
    private float _protect;

    [SerializeField]
    private float _speed = 25f, _speedRotate = 30f, _speedTurret = 25f;

    private float _verticalAxis, _horizontalAxis, _turretRotateAxis;

    private WeaponController _weaponController;

    private Transform _turret;
    private Rigidbody _rigidbody;
    private int _currHealth;
    private UIManager _uiManager;

    public Action<int> HealthChanged;
    public Action<float> TimerForRechargeChanged, SetCurrTime;
    public Action PlayerDead;

    public int Health
    {
        get { return _currHealth; }
        set
        {
            _currHealth = value;
            if (_currHealth < 0) _currHealth = 0;
            if (HealthChanged != null)
            {
                HealthChanged(_currHealth);
            }
        }
    }

    private void Awake()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
        _weaponController = transform.GetComponent<WeaponController>();
        Enable = true;
        _currHealth = StartHealth;
    }

    void Start()
    {
        _turret = transform.Find("Turret");
    }

    public void ChangeWeapon()
    {
        if (Enable)
            _weaponController.ChangeWeapon();
        if (TimerForRechargeChanged != null) TimerForRechargeChanged(_weaponController.CurrentWeapon.RechargeTime);
    }

    public void Fire()
    {
        if (Enable)
            _weaponController.Fire();
    }

    public override void Init()
    {
        _uiManager = Main.Instance.GetPage<UIManager>();
    }

    void Update()
    {
        if (Enable)
        {
            _verticalAxis = _uiManager.GetVerticalAxis();
            _horizontalAxis = _uiManager.GetHorizontalAxis();
            if (_verticalAxis < -0.4) _horizontalAxis = -_horizontalAxis;

            _turretRotateAxis = _uiManager.GetTurretRotateAxis();
            _turret.Rotate(0f, _turretRotateAxis * _speedTurret * Time.deltaTime, 0f);

            if(SetCurrTime!=null) SetCurrTime(_weaponController.CurrRechargeTime);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(transform.forward * _verticalAxis * _speed, ForceMode.Acceleration);

        Quaternion turnRotation = Quaternion.Euler(0f, _horizontalAxis * _speedRotate * Time.deltaTime, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
    }

    public void AddHealth(int health)
    {
        Health = Mathf.Clamp(Health + health, 0, 100);
    }

    public void GetDamage(int damage)
    {
        Health -= Mathf.CeilToInt(damage * (1 - _protect));
        if (Health <= 0)
        {
            Enable = false;
            _verticalAxis = 0;
            _horizontalAxis = 0;
            _turretRotateAxis = 0;
            PlayerDead();
        }
    }

    public void ResetPlayer()
    {
        Enable = true;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        Health = StartHealth;
    }

    public override void Dispose()
    {
        foreach (var funk in HealthChanged.GetInvocationList())
        {
            HealthChanged -= (Action<int>)funk;
        }

        foreach (var funk in SetCurrTime.GetInvocationList())
        {
            SetCurrTime -= (Action<float>)funk;
        }
    }
}
