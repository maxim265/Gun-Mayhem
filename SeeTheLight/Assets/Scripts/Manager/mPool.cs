using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class mPool : MonoBehaviour
{
    static mPool inst;
    public static mPool Inst => inst;
    Transform mainPool;
    Dictionary<string, List<GameObject>> poolDic = new Dictionary<string, List<GameObject>>();
    private void Awake()
    {
        inst = this;
        mainPool = new GameObject("Pool").transform;
    }

    public void AddToPool(string name, GameObject obj)
    {
        //如果没有对应池位置
        if (!poolDic.ContainsKey(name))
        {
            poolDic[name] = new List<GameObject>();
        }
        obj.SetActive(false);
        obj.transform.SetParent(mainPool);
        poolDic[name].Add(obj);
    }
    public GameObject GetFromPool(string name)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].Count > 0)
        {
            GameObject obj = poolDic[name][0];
            poolDic[name].RemoveAt(0);
            obj.SetActive(true);
            obj.transform.SetParent(null);
            return obj;
        }
        else 
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("GameObject/" + name));
            obj.name = name;
            return obj;
        }
    }
}
