using System.Collections.Generic;
using UnityEngine;

public class PoolMono<T> where T : MonoBehaviour
{
    public T prefab { get; }

    public bool autoExpand { get; set; }

    public Transform container { get; }

    private List<T> pool;

    public PoolMono(T prefab, int size)
    {
        this.prefab = prefab;
        this.container = null;

        CreatePool(size);
    }

    public PoolMono(T prefab, int size, Transform container)
    {
        this.prefab = prefab;
        this.container = container;

        CreatePool(size);
    }

    private void CreatePool(int size)
    {
        this.pool = new List<T>();

        for (int i = 0; i < size; i++)
        {
            this.CreateObject();
        }
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        T createdObject = Object.Instantiate(this.prefab, this.container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        this.pool.Add(createdObject);
        return createdObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (T mono in pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (this.HasFreeElement(out T element))
        {
            return element;
        }

        if (this.autoExpand)
        {
            return this.CreateObject(true);
        }

        throw new System.Exception("There is no free elements");
    }
}
