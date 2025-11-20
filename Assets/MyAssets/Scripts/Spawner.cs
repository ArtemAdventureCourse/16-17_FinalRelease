using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 本クラスは、敵キャラクターの生成と行動パターンの設定を管理します。
/// </summary>
/// <remarks>
/// 主な機能:
/// - 新しい敵キャラクターを生成し、管理リストに追加
/// - 生成した敵に対して待機行動や反応行動を割り当て
/// - 生成位置や移動ポイントをランダムに調整
/// 
/// 使用方法:
/// - ゲーム内で敵キャラクターを出現させ、行動パターンを設定する際に使用します。
/// - 管理者や他の開発者は、このクラスを通じて敵の生成状況や行動設定を把握できます。
/// </remarks>
public class Spawner : MonoBehaviour
{
    [SerializeField] private EnemyIdleBehaviorType _idleType;
    [SerializeField] private EnemyReactBehaviorType _reactType;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Transform _spawnPoint;
    private Enemy _lastEnemy;
    public List<Enemy> enemies = new();
    private Vector3 _spawnOffset;
    [SerializeField] private Transform _homePosition;
    [SerializeField] private Effect _effect;
    [SerializeField] private Transform _player;
    private CharactersFactory _charactersFactory = new CharactersFactory();
    private Vector3 random;
    public void Initialize()
    {
        _spawnOffset = new Vector3(0, 1, 0);
    }

    private void LateUpdate()
    {
        _enemyPrefab.gameObject.SetActive(false);
    }


    private void Start()
    {
        random = new Vector3(
          Random.Range(2, 10),
          transform.position.y,
          Random.Range(2, 14));
        _spawnPoint.position += random;

        SpawnEnemy(_spawnPoint);
        SelectBehavior();

    }

    public void SelectBehavior() => _lastEnemy?.Init(_idleType, _reactType);

    public void SpawnEnemy(Transform spawnPoint)
    {
        _lastEnemy = _charactersFactory.CreateEnemy(_enemyPrefab, spawnPoint.position + random, _spawnOffset);
        enemies?.Add(_lastEnemy);

    }

    public IBehavior SpawnIdleBehavior(
        EnemyIdleBehaviorType type,
        Enemy enemy,
        List<Transform> points)
    {
        return _charactersFactory.CreateIdleBehavior(type, enemy, _homePosition, points);

    }

    public IBehavior SpawnReactBehavior(
        EnemyReactBehaviorType type,
        Enemy enemy,
        Effect effect,
        Transform enemyPosition,
        NavMeshAgent navMesh = null)
    {

        return _charactersFactory.CreateReactBehavior(type, enemy, _player, effect, enemyPosition, navMesh);
    }

}

