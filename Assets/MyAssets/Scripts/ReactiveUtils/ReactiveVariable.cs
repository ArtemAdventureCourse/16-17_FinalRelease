using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ReactiveVariable<T> : IDisposable where T : IEquatable<T>
{
    public event Action<T, T> ChangedDetailed;
    public event Action<T> Changed;
    public event Action Dead;
    private Coroutine _coroutineDamage;
    private T _value;

    public Coroutine CoroutineDamage { get { return _coroutineDamage; } set { value = _coroutineDamage; } }
    public ReactiveVariable() => _value = default(T);

    public ReactiveVariable(T value) => _value = value;


    public T Value
    {

        get => _value;
        set
        {
            T oldValue = _value;
            _value = value;
            if (_value.Equals(oldValue) == false)
            {
                ChangedDetailed?.Invoke(oldValue, value);
                Changed?.Invoke(value);

                if (Convert.ToInt16(value) == 0)
                {
                 
                    Dead.Invoke();
                }
            }
        }
    }

    public void Dispose()
    {




    }
}
