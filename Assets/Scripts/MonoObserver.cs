using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoObserver : MonoBehaviour, IObserver
{
    public Observer observable;
    private bool subscribed = false;
    public abstract void Notify();
    protected virtual void OnEnable()
    {
        Subscribe();
    }
    protected virtual void OnDisable()
    {
        UnSubscribe();
    }
    protected void Resubscribe(Observer observable)
    {
        UnSubscribe();
        this.observable = observable;
        Subscribe();
    }
    private void Subscribe()
    {
        if (!subscribed)
        {
            observable.Subscribe(this);
            subscribed = true;
        }
    }
    private void UnSubscribe()
    {
        if (subscribed)
        {
            observable.UnSubscribe(this);
            subscribed = false;
        }
    }
}
