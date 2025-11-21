using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private List<Coin> coinList;
    private List<Coin> coinCollected = new List<Coin>();
    public event Action StartedCollect;
    [SerializeField] private Enemy _enemy;
    //  private List<Enemy> _enemies=new List<Enemy>();
    [SerializeField] private List<Spawner> _spawners;
     public bool HasFullCoin=false;
    public void Initialize()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Coin coin = other.GetComponentInChildren<Coin>();
        if (coin != null)
        {
            foreach (Spawner spawner in _spawners)
            {
                foreach (Enemy enemy in spawner.enemies)
                {
                    enemy.ChangeChaseState();
                    enemy.SetReactBehavior();
                }
            }

            coinCollected.Add(coin);
            DevLog.Log("монета:" + coin);
            coin.gameObject.SetActive(false);

            bool equal = coinList.Select(x => x.GetInstanceID()).OrderBy(id => id)
                  .SequenceEqual(
                        coinCollected.Select(x => x.GetInstanceID()).OrderBy(id => id));
            if (equal)
            {
                DevLog.Log("все монеты собраны");
                HasFullCoin = true;
                _enemy.Dead();
            }

        }
    }

}
