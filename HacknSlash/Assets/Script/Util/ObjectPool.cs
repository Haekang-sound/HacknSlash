using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 간단한 제네릭 오브젝트 풀입니다.
/// 런타임 가비지 생성을 방지합니다.
/// 관리대상을 Commponent로 한정합니다.
/// 
/// </summary>
public class ObjectPool<T> where T : Component
{
    private T _prefab;
    private Transform _parent;
    private Queue<T> _pool = new Queue<T>();

    public Queue<T> GetPool() { return _pool; }

    /// <summary>
    /// 오브젝트풀 생성자 
    /// </summary>
    /// <param name="prefab">오브젝트풀이 관리할 자료형</param>
    /// <param name="initialSize">초기 사이즈</param>
    /// <param name="parent">오브젝트로 생성된 객체들을 담아둘 entity</param>
    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNew();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public void AddPool(T prefab, int _size)
    {
        _prefab = prefab;

        for (int i = 0; i < _size; i++)
        {
            T obj = CreateNew();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// 오브젝트를 생성하고 
    /// pool에 집어넣습니다.
    /// 
    /// </summary>
    /// <returns>T</returns>
    private T CreateNew()
    {
        T obj = Object.Instantiate(_prefab, _parent);
        return obj;
    }

    /// <summary>
    /// 오브젝트를 꺼내옵니다.
    /// 꺼낼 오브젝트가 없는경우 생성합니다.
    /// 
    /// </summary>
    /// <returns>T</returns>
    public T Get()
    {
        T obj;
        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else
        {
            obj = CreateNew();
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    /// <summary>
    /// 오브젝트를 비활성화하고
    /// 풀로 반환합니다.
    /// 
    /// </summary>
    /// <param name="obj"></param>
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
