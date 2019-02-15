using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    public Monster[] MonsperPrefabs;
    public int activeMonster;
    public float minSpawnDistance = 2;

    private ObjectPool<Monster> _monsterPool;
    private Transform _playerTransform;
    private float _sizeOfHalfGround;

    private void Awake()
    {
        int countOfEachTypes = 0;
        _monsterPool = new ObjectPool<Monster>();
        if (MonsperPrefabs.Length > 0)
        {
            countOfEachTypes = Mathf.CeilToInt(activeMonster / MonsperPrefabs.Length);
            _monsterPool.InitializePool(MonsperPrefabs[0], transform, countOfEachTypes);
            if (MonsperPrefabs.Length > 1)
            {
                for (int i = 1; i < MonsperPrefabs.Length; i++)
                {
                    _monsterPool.AddObjectsToPool(MonsperPrefabs[i], countOfEachTypes);
                }
            }
            Enable = true;

            foreach (var item in _monsterPool.PoolObjects)
            {
                item.ReturnMonsterToPool += _monsterPool.ReturnObjectToPool;
            }
        }
    }

    void Start()
    {
        Vector3 size = GameObject.Find("Ground").GetComponent<Renderer>().bounds.size;
        _sizeOfHalfGround = (size.x < size.z ? size.x : size.z) / 2;
        _playerTransform = GameObject.Find("Player/Turret").transform;
    }

    public void Reset()
    {
        foreach (var item in _monsterPool.PoolObjects)
        {
            _monsterPool.ReturnObjectToPool(item);
        }
    }

    void Update()
    {
        if (Enable)
        {
            if (_monsterPool.CountActiveObject < activeMonster)
            {
                Quaternion rotationAroundSpawnPos = Quaternion.Euler(0, UnityEngine.Random.Range(-90, 90), 0);
                float distPlayerAndCenterGround = Vector3.Distance(_playerTransform.position, Vector3.zero);
                float spawnDistance = (_sizeOfHalfGround - UnityEngine.Random.Range(distPlayerAndCenterGround, _sizeOfHalfGround)) + minSpawnDistance;
                Vector3 offSet = (-_playerTransform.forward) * spawnDistance;
                _monsterPool.GetObjectFromPool().transform.position = rotationAroundSpawnPos * (_playerTransform.position + offSet);
            }
        }
    }
}
