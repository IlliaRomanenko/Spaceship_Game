using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using Oculus.Interaction;
using UnityEditor;

public class DoughnutPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 5;
    [SerializeField] private int expandNumber = 1;
    [SerializeField] private int maxPoolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private List<GameObject> allPoolObjects = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ExpandPool(initialSize);
    }

    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            if (allPoolObjects.Count < maxPoolSize)
            {
                ExpandPool(expandNumber);
            }
            else
            {
                return null;
            }
        }

        GameObject obj = pool.Dequeue();
        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.SetParent(null, true);
        }
        return obj;
    }


    public void Return(GameObject obj)
    {
        if (obj == null) return;
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
        pool.Enqueue(obj);
    }

    private void ExpandPool(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
            allPoolObjects.Add(obj);
        }
    }

    public void ClearPool()
    {
        foreach (GameObject obj in allPoolObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        pool.Clear();
        allPoolObjects.Clear();
    }
    
}
