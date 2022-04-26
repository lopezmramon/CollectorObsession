using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GenericEnumCollection<TEnum, T> : IEnumerable<KeyValuePair<TEnum,T>> where TEnum : struct, IConvertible, IComparable, IFormattable
{
    [SerializeField]
    private T[] Collection;

    public GenericEnumCollection()
    {
        var attributes = (TEnum[])Enum.GetValues(typeof(TEnum));
        Collection = new T[attributes.Length];
        for (var i = 0; i < Collection.Length; i++)
            Collection[i] = default;
    }

    public T this[TEnum e]
    {
        get { return Collection[Convert.ToInt32(e)]; }
        set { Collection[Convert.ToInt32(e)] = value; }
    }

    protected T this[int i]
    {
        get { return Collection[i]; }
        set { Collection[i] = value; }
    }

    public TEnum[] Keys { get { return (TEnum[])Enum.GetValues(typeof(TEnum)); } }

    public T[] Values { get { return Collection.ToArray(); } }
    
    public TEnum IndexOf(T t) => (TEnum)Enum.ToObject(typeof(TEnum), Collection.ToList().IndexOf(t));

    public IEnumerator<KeyValuePair<TEnum,T>> GetEnumerator() => 
        Collection.Select(c=>new KeyValuePair<TEnum, T>(IndexOf(c),c)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
