using System;
using System.Collections.Generic;

public class ObjectPool<T> where T : new()
{
    private readonly Stack<T> m_pool = new Stack<T>();
    private readonly Action<T> m_ActionOnGet;
    private readonly Action<T> m_ActionOnRelease;

    public int countAll { get; private set; }
    public int countActive { get { return countAll - countInactive; } }
    public int countInactive { get { return m_pool.Count; } }

    private object m_lock = new object();

    public ObjectPool(Action<T> actionOnGet = null, Action<T> actionOnRelease = null)
    {
        m_ActionOnGet = actionOnGet;
        m_ActionOnRelease = actionOnRelease;
    }

    public void Clear()
    {
        lock (m_lock)
        {
            m_pool.Clear();
            countAll = 0;
        }
    }

    public T Get()
    {
        T element;

        lock (m_lock)
        {
            if (m_pool.Count == 0)
            {
                element = new T();
                countAll++;
            }
            else
            {
                element = m_pool.Pop();
            }
        }
        if (m_ActionOnGet != null)
            m_ActionOnGet(element);
        return element;
    }

    public void Release(T element)
    {
        if (m_ActionOnRelease != null)
            m_ActionOnRelease(element);
        lock (m_lock)
        {
            m_pool.Push(element);
        }
    }
}
