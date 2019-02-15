using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartMonster : Monster
{
    [SerializeField]
    protected float _afterHitTime = 1.5f;
    [SerializeField]
    protected float _cooldownTime = 1f;

    protected Timer _getHitTimer;
    protected Animator _animator;
    protected Timer _cooldownTimer;

    protected override void Awake()
    {
        base.Awake();
        _getHitTimer = new Timer(); //Переделать!
        _getHitTimer.InitTimer(_afterHitTime);

        _cooldownTimer = new Timer();
        _cooldownTimer.InitTimer(_cooldownTime);

        _animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if (isActiveAndEnabled && _isAlive)
        {
            _getHitTimer.Update();
            if (_getHitTimer.TimeOver && _getHitTimer.IsActive)
            {
                _getHitTimer.Off();
                _agent.isStopped = false;
            }
        }
    }

    protected override void Attack()
    {
        if (!_agent.isStopped)
        {
            _cooldownTimer.Update();
            if (_cooldownTimer.TimeOver && _cooldownTimer.IsActive)
            {
                transform.LookAt(_goal);
                _animator.SetTrigger("Attack");
                _cooldownTimer.ResetTimer(); ;
                _goal.GetComponent<IHaveHealth>().GetDamage(_damage);
            }
        }
    }

    protected override void RunToGoal()
    {
        if (_isAlive)
            _animator.SetTrigger("Run");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _cooldownTimer.On();
        _currentHealth = Health;
        _agent.isStopped = false;
        _animator.Play("Run");
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Dead()
    {
        base.Dead();
        _isAlive = false;
        _cooldownTimer.Off();
        _agent.isStopped = true;
        _getHitTimer.Off();
        _animator.Play("Dead");
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        if (_isAlive)
        {
            _animator.SetTrigger("GetHit");
            _agent.isStopped = true;
            _getHitTimer.On();
        }
    }

    /*public bool IsAnimationPlaying(string animationName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            return true;
        return false;
    }*/
}
