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

    protected virtual void Awake()
    {
        if (_goal == null)
        {
            _goal = FindObjectOfType<TankController>().transform;
        }
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
    }

    protected virtual void Update()
    {
        _agent.destination = _goal.position;
    }


    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (_isAlive)
        {
            if (collision.collider.CompareTag("Player"))
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
            _isAlive = false;
            Dead();
        }
    }

    public virtual void ResetMonstr()
    {
        _isAlive = true;
    }

    protected virtual void OnEnable()
    {
        ResetMonstr();
    }

    protected virtual void OnDisable() { }

    public virtual void AddHealth(int health) { }

    protected abstract void Dead();
}
