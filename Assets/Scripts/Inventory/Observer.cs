using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer
{
    private List<IObserver> subscribers = new List<IObserver>();

    public void Subscribe(IObserver observer)
    {
        subscribers.Add(observer);
    }
    public void UnSubscribe(IObserver observer)
    {
        subscribers.Remove(observer);
    }
    public void Notify()
    {
        foreach (var observer in subscribers)
        {
            observer.Notify();
        }
    }
}
