using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : BaseController, IHaveHealth
{
    [SerializeField]
    private int _health;

    [SerializeField]
    private float _protect;

    [SerializeField]
    private float _speed = 25f, _speedRotate = 30f, _speedTurret = 25f;

    private float _verticalAxis, _horizontalAxis, _turretRotateAxis;

    private WeaponController _weaponController;

    private Transform _turret;
    private Rigidbody _rigidbody;

    private UIManager _uiManager;

    // private ParticleSystem shootEffect
    private void Awake()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
        Enable = true;
    }

    void Start()
    {
        _turret = transform.Find("Turret");
        _weaponController = transform.Find("Turret/Weapons").GetComponent<WeaponController>();
    }

    public void ChangeWeapon()
    {
        _weaponController.ChangeWeapon();
    }

    public void Fire()
    {
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
        }
    }

    private void FixedUpdate()
    { 
        _rigidbody.AddForce(transform.forward * _verticalAxis * _speed, ForceMode.Acceleration);

        Quaternion turnRotation = Quaternion.Euler(0f, _horizontalAxis * _speedRotate*Time.deltaTime, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
    }

    public void AddHealth(int health)
    {
        _health = Mathf.Clamp(_health + health, 0, 100);
    }

    public void GetDamage(int damage)
    {
        _health -= Mathf.CeilToInt(damage * (1 - _protect));
        if (_health <= 0)
        {
            Debug.Log("Player is Dead");
            //Invoke some menu
            Enable = false;
        }
    }
}
