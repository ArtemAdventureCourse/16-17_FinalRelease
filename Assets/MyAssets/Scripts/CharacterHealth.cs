using System;
using System.Collections;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    private ReactiveVariable<int> _health = new ReactiveVariable<int>(5000);

    public ReactiveVariable<int> Health { get { return _health; } private set { _health = value; } }

    [SerializeField] private UI _uI;

    private Coroutine _damage;
    private void Start()
    {
        _health.Dead += OnDead;
    }

    public void OnDead() { gameObject.SetActive(false); }
    public void TakeDamage(int dmg)
    {

        if (_damage != null)
        {
            StopCoroutine(_damage);
        }
        _damage = StartCoroutine(TakeDamageCoroutine(dmg));
    }

    public IEnumerator TakeDamageCoroutine(int dmg)
    {
        _health.Value -= dmg;
        yield return new WaitForSeconds(1);
        yield return _health.CoroutineDamage;

    }

    private void OnDestroy()
    {
        _health = null;
    }
}
