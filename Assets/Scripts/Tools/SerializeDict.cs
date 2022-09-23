using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializeDict<TKey, TVal> : Dictionary<TKey, TVal>
{
    [SerializeField]
    private List<MyKeyValuePair> contents = new List<MyKeyValuePair>();

    public void Construct()
    {
        foreach (var item in contents)
        {
            this[item.Key] = item.Value;
        }
    }

    [System.Serializable]
    private struct MyKeyValuePair
    {
        public TKey Key;
        public TVal Value;
    }
}
