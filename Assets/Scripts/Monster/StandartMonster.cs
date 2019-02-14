using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartMonster : Monster
{
    [SerializeField]
    protected float _afterHitTime = 1.5f;
    [SerializeField]
    protected float _cooldownTime = 1f;

    private Timer _getHitTimer;
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

            if (Vector3.Distance(transform.position, _goal.position) < _distanceForAttack)
            {
                Attack();
            }
            else
            {
                RunToGoal();
            }
        }
    }

    protected void Attack()
    {
        _cooldownTimer.Update();
        if (_cooldownTimer.TimeOver && _cooldownTimer.IsActive)
        {
            _animator.SetTrigger("Attack");
            _cooldownTimer.ResetTimer(); ;
            _goal.GetComponent<IHaveHealth>().GetDamage(_damage);
        }
    }

    protected void RunToGoal()
    {
        if(_isAlive)
            _animator.SetTrigger("Run");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void ResetMonstr()
    {
        base.ResetMonstr();
        _cooldownTimer.On();
        _currentHealth = Health;
        _agent.isStopped = false;
        _animator.Play("Run");
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _cooldownTimer.Off();
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

    protected override void Dead()
    {
        _isAlive = false;
        _cooldownTimer.Off();
        _agent.isStopped = true;
        _getHitTimer.Off();
        _animator.Play("Dead");
        // get object to spawnManager or Pool
    }

    /*public bool IsAnimationPlaying(string animationName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            return true;
        return false;
    }*/
}
