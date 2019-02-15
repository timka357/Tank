using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : BasePage
{
    private FixedJoystick _bodyJoystick, _turretJoystick;
    private Transform GamePanel, ExitPanel;

    private Button _fireButton, _changeWeaponButton, _exitButton, _resetButton;
    private Slider _healthSlider, _rechargeSlider;
    private Action fireAction, changeWeaponAction, resetGame;

    private TankController Tank;

    private void Awake()
    { 
        GamePanel = transform.Find("GamePanel");
        ExitPanel = transform.Find("ExitPanel");
        _exitButton = ExitPanel.Find("Exit").GetComponent<Button>();
        _resetButton = ExitPanel.Find("Reset").GetComponent<Button>();
        ExitPanel.gameObject.SetActive(false);
        _bodyJoystick = GamePanel.Find("BodyJoystick").GetComponent<FixedJoystick>();
        _turretJoystick = GamePanel.Find("TurretJoystick").GetComponent<FixedJoystick>();
        _healthSlider = GamePanel.Find("HealthBar").GetComponent<Slider>();
        _rechargeSlider = GamePanel.Find("RechargeBar").GetComponent<Slider>();

        _fireButton = GamePanel.Find("FireButton").GetComponent<Button>();
        _changeWeaponButton = GamePanel.Find("ChangeWeaponButton").GetComponent<Button>();
    }

    public override void Init()
    {
        base.Init();
        Tank = Main.Instance.GetController<TankController>();
        changeWeaponAction += Tank.ChangeWeapon;
        fireAction += Tank.Fire;
        resetGame += Tank.ResetPlayer;
        resetGame += Main.Instance.GetController<MonsterController>().Reset;

        Tank.HealthChanged += SetHealthBar;
        Tank.SetCurrTime += SetRechargeTime;
        Tank.PlayerDead += PlayerDead;
        Tank.TimerForRechargeChanged += SetNewRechargeTime;

        _fireButton.onClick.AddListener(FireClick);
        _changeWeaponButton.onClick.AddListener(ChangeWeaponClick);
        _exitButton.onClick.AddListener(Application.Quit);
        _resetButton.onClick.AddListener(ResetGame);
    }

    private void ResetGame()
    {
        ExitPanel.gameObject.SetActive(false);
        GamePanel.gameObject.SetActive(true);
        if (resetGame != null) resetGame();
    }

    private void PlayerDead()
    {
        GamePanel.gameObject.SetActive(false);
        ExitPanel.gameObject.SetActive(true);
    }

    private void SetHealthBar(int health)
    {
        _healthSlider.value = health;
    }

    private void SetNewRechargeTime(float valeu)
    {
        _rechargeSlider.maxValue = valeu;
    }

    private void SetRechargeTime(float valeu)
    {
        _rechargeSlider.value = valeu;
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
