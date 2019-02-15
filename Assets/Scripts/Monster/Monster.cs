using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour, IHaveHealth
{
    [SerializeField]
    protected Transform _goal;

    [SerializeField]
    protected float _speed;
    [SerializeField]
    protected int _damage;
    [SerializeField]
    protected int Health;
    [SerializeField]
    protected float _protect;
    [SerializeField]
    protected float _distanceForAttack;

    protected int _currentHealth;
    protected NavMeshAgent _agent;
    protected bool _isAlive;

    public Action<Monster> ReturnMonsterToPool { get; set; }
    protected Timer _timerBeforeReturningIntoPool;
    protected float _timeAfterDeath = 4f;

    protected virtual void Awake()
    {
        if (_goal == null)
        {
            _goal = FindObjectOfType<TankController>().transform;
        }
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;

        _timerBeforeReturningIntoPool = new Timer();
        _timerBeforeReturningIntoPool.InitTimer(_timeAfterDeath);
    }

    protected virtual void Update()
    {
        if (transform.position.y < -2f) Dead(); //if monster falling down

        if (_isAlive && isActiveAndEnabled)
        {
            _agent.destination = _goal.position;
            if (Vector3.Distance(transform.position, _goal.position) < _distanceForAttack)
            {
                Attack();
            }
            else
            {
                RunToGoal();
            }
        }

        if(_timerBeforeReturningIntoPool.IsActive)
        {
            _timerBeforeReturningIntoPool.Update();
            if (_timerBeforeReturningIntoPool.TimeOver)
            {
                _timerBeforeReturningIntoPool.Off();
                if (ReturnMonsterToPool != null)
                {
                    ReturnMonsterToPool(this);
                }
                else
                {
                    Destroy(gameObject);
                }
            }    
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (_isAlive)
        {
            if (collision.collider.CompareTag(_goal.tag))
            {
                collision.transform.GetComponent<IHaveHealth>().GetDamage(_damage);
            }
        }
    }

    public virtual void GetDamage(int damage)
    {
        _currentHealth -= Mathf.CeilToInt(damage * (1 - _protect));
        if (_currentHealth <= 0 && _isAlive)
        {
            Dead();
        }
    }

    protected virtual void Dead()
    {
        _isAlive = false;
        _timerBeforeReturningIntoPool.On();
    }

    protected virtual void OnEnable()
    {
        _isAlive = true;
        _currentHealth = Health;
    }

    protected virtual void RunToGoal(){}

    protected virtual void Attack() {}

    protected virtual void OnDisable() { }

    public virtual void AddHealth(int health) { }
}
