using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store<K,V>
{
    public List<ValuePairData<K, V>> datas;

    public Store()
    {
        datas = new List<ValuePairData<K, V>>();
    }

    public void AddData(K key,V value)
    {
        datas.Add(new ValuePairData<K, V>() { key = key, value = value });
    }

    void SetValue(K key,V value)
    {
        for(int i=0;i < datas.Count; i++)
        {
            if (datas[i].key.Equals(key))
            {
                datas[i].value = value;
            }
        }
    }

    V GetValue(K key)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].key.Equals(key))
            {
                return datas[i].value;
            }
        }
        return default(V);
    }

    public V this[K key]
    {
        set
        {
            SetValue(key, value);
        }

        get
        {
            return GetValue(key);
        }
    }

    public bool ContainsKey(K key)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].key.Equals(key))
            {
                return true;
            }
        }
        return false;
    }

    public void DeleteKey(K key)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].key.Equals(key))
            {
                datas.RemoveAt(i);
            }
        }
    }
}
