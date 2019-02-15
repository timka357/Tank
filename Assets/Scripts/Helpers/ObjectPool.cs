using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T _poolObjectPrefab;
    private List<T> _poolObjects;

    private Transform _transformPositionAndRotation, _parent;

    public int CountActiveObject
    {
        get; private set;
    }

    public List<T> PoolObjects
    {
        get { return _poolObjects; }
    }

    public void InitializePool(T prefab, Transform parent, int count, Transform position = null)
    {
        _poolObjectPrefab = prefab;
        _transformPositionAndRotation = position;
        _parent = parent;

        CountActiveObject = 0;

        _poolObjects = new List<T>();

        for (int i = 0; i < count; i++)
            AddObjectToPool();
    }

    public void ClearPool()
    {
        foreach (var element in _poolObjects)
        {
            if (element != null)
                MonoBehaviour.Destroy(element);
        }

        _poolObjects.Clear();
    }

    public void AddObjectToPool(T prefab = null)
    {
        if (prefab != null) _poolObjectPrefab = prefab;

        T newObject = MonoBehaviour.Instantiate(_poolObjectPrefab).GetComponent<T>();
        newObject.transform.SetParent(_parent);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localRotation = Quaternion.identity;
        newObject.gameObject.SetActive(false);

        _poolObjects.Add(newObject);
    }

    public void AddObjectsToPool(T prefab, int count)
    {
        for (int i = 0; i < count; i++)
            AddObjectToPool(prefab);
    }

    public T GetObjectFromPool()
    {
        T freeObject = _poolObjects.Find(x => !x.isActiveAndEnabled);

        if (freeObject == null)
            AddObjectToPool();
        else
        {
            if (_transformPositionAndRotation != null)
            {
                freeObject.transform.position = _transformPositionAndRotation.position;
                freeObject.transform.rotation = _transformPositionAndRotation.rotation;
            }
            freeObject.gameObject.SetActive(true);
            CountActiveObject++;
            return freeObject;
        }

        return GetObjectFromPool();
    }

    public void ReturnObjectToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        CountActiveObject--;
    }
}
