using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : BaseController
{
    private int _currentWeapon = 0;
    [SerializeField]
    private Weapon[] _weaponPrefabs;

    private List<Weapon> _weaponList;
    protected Timer _rechargeTimer;
    private bool _isCanFire = false;

    public bool IsCanFire
    {
        get { return _isCanFire; }
    }

    public Weapon CurrentWeapon { get { return _weaponList[_currentWeapon]; } }

    void Start()
    {
        _weaponList = new List<Weapon>();
        _rechargeTimer = new Timer();

        if (_weaponPrefabs != null && _weaponPrefabs.Length > 0)
        {
            foreach (var weapon in _weaponPrefabs)
            {
                AddNewWeapon(weapon);
            }

            SetCurrentWeapon(_currentWeapon);
            Enable = true;
        }
        else Enable = false;
    }

    private void Update()
    {
        if (Enable)
        {
            _rechargeTimer.Update();
            if (_rechargeTimer.TimeOver && !IsCanFire)
            {
                _isCanFire = true;
            }
        }
    }

    public void AddNewWeapon(Weapon weapon)
    {
        _weaponList.Add(Instantiate(weapon, transform));
        _weaponList[_weaponList.Count - 1].Hide();

        if (_weaponList.Count == 1)
        {
            SetCurrentWeapon(0);
            Enable = true;
        }
    }

    public void RemoveWeapon(Weapon weapon)
    {
        if (_weaponList.Count > 1 && _weaponList.Contains(weapon))
        {
            if (!weapon.Equals(_weaponList[_currentWeapon]))
            {
                if (_weaponList.IndexOf(weapon) < _currentWeapon) _currentWeapon--;
                _weaponList.Remove(weapon);
            }
            else
            {
                ChangeWeapon();
                RemoveWeapon(weapon);
            }
        }
    }

    public void SetCurrentWeapon(int index)
    {
        _weaponList[index].SetActive();
        _rechargeTimer.InitTimer(_weaponList[index].RechargeTime);
        _rechargeTimer.On();
        _isCanFire = false;
    }

    public void ChangeWeapon()
    {
        _weaponList[_currentWeapon].Hide();
        if (_currentWeapon + 1 >= _weaponList.Count) _currentWeapon = 0;
        else _currentWeapon++;

        SetCurrentWeapon(_currentWeapon);
    }

    public void Fire()
    {
        if (_isCanFire)
        {
            _isCanFire = false;
            _rechargeTimer.ResetTimer();
            _weaponList[_currentWeapon].Fire();
        }
    }
}
