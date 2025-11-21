using System;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class AggrZone : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Player _player;
    [SerializeField] private Transform _target;
    [SerializeField]private CharacterHealth _character;
     private float _enterDistance = 5.5f;   
     private float _exitDistance = 12.0f;
    private bool HasEnemy => _enemy != null;

    public event Action<GameObject> Entered;
    private bool playerInTrigger = false;
    private bool isActive = false;
    private float _dist;
    public GameObject GetGameObject() => _player.gameObject;

    public float GetDistance() => _dist = Vector3.Distance(_player.transform.position, _enemy.transform.position);
    private void Update()
    {

        GetDistance();

        if (_dist <= _enterDistance)
        {
            _enemy.SetTarget(_player.transform);
            _enemy.SetReactBehavior();
            _character.TakeDamage(1);
        }


        if (_dist > _exitDistance)
        {
            _enemy.SetIdleBehavior();
            _enemy.ClearTarget();
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Player>() is Player player && HasEnemy)
        {
            _player = player;
            Entered?.Invoke(player.gameObject);
            
        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<Player>() is Player player && HasEnemy)
        {
            Entered?.Invoke(player.gameObject);
            _player = player;


        }

    }

}
