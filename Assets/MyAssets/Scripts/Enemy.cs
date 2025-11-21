using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour, IAgroObserver
{
    private GameObject _gameObject;
    [SerializeField] private Transform _targetPlayer;
    [SerializeField] Spawner _spawner;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private List<Transform> _points;
    [SerializeField] private Transform _homePosition;
    [SerializeField] private CoinCollector _collector;
    private IBehavior _currentBehavior;
    private IBehavior _idleBehavior;
    private IBehavior _reactBehavior;
    private IBehavior _forCollectCoinBehavior;
    private IBehavior _dieBehavior;
    [SerializeField] private Effect _effect;

    [Header("Текущее под-состояние Idle")]
    [SerializeField] private EnemyIdleBehaviorType _currentIdleType;
    private EnemyIdleBehaviorType _previousIdleType;

    [Header("Текущее под-состояние React")]
    [SerializeField] private EnemyReactBehaviorType _currentReactType;
    private EnemyReactBehaviorType _previousReactType;

    private event Action behaviorChanged;
    [SerializeField] private AggrZone _aggrZone;

    private float _dist = 0;
    public bool IsChangedIdleStateInInspector => _currentIdleType != _previousIdleType;
    public bool IsChangedReactStateInInspector => _currentReactType != _previousReactType;

    private bool IsEnteredPlayer;



    public void Awake()
    {


    }
    public void Initialize()
    {
        _currentIdleType = EnemyIdleBehaviorType.Stand;
    }
    private void Start()
    {


        var groups = _points.OrderByDescending(group => group.name);

        foreach (Transform group in groups)
        {
            DevLog.Log($"group:{group.position}");
        }

    }

    private bool IsSafetyDistance()
    {

        if (_dist >= 2)
        {
            return false;
        }
        return true;

    }

    public void Dead()
    {
        gameObject.SetActive(false);
    }
    private bool NotStartCollectCoin() => _forCollectCoinBehavior == null;
    public void Update()
    {
        _dist = _aggrZone.GetDistance();

        if (IsSafetyDistance() == false)
        {

            if (NotStartCollectCoin())
            {
                _currentBehavior?.Update();
            }
            _forCollectCoinBehavior?.Update();
        }
        else
        {
            DamagePlayerForExplosion();
            _dieBehavior?.Update();

        }

        ChangeStateAllEnemies();

        ChangeStateInInspector();

    }

    private void ChangeStateAllEnemies()
    {

        if (Input.GetKey(KeyCode.Y))
        {
            _forCollectCoinBehavior = null;
            ChangeIdleSubState(EnemyIdleBehaviorType.RandomWalk);

        }

        if (Input.GetKey(KeyCode.I))
        {
            _forCollectCoinBehavior = null;
            ChangeReactSubState(EnemyReactBehaviorType.Chase);

        }
    }
    private void ChangeStateInInspector()
    {

        if (IsChangedIdleStateInInspector)
        {
            ChangeIdleSubState(_currentIdleType);
            _previousIdleType = _currentIdleType;
            return;
        }

        if (IsChangedReactStateInInspector)
        {
            ChangeReactSubState(_currentReactType);
            _previousReactType = _currentReactType;
            return;
        }
    }
    public void Set(IBehavior behavior)
    {
        _currentBehavior = behavior;
    }
    public void SetTarget(Transform target) => _targetPlayer = target;


    public void ChangeChaseState()
    {
        _forCollectCoinBehavior = _spawner.SpawnReactBehavior(EnemyReactBehaviorType.Chase, this, _effect, this.transform);

    }

    private void DamagePlayerForExplosion()
    {
        _dieBehavior = _spawner.SpawnReactBehavior(EnemyReactBehaviorType.Die, this, _effect, this.transform);
    }
    public void Init(EnemyIdleBehaviorType idleType, EnemyReactBehaviorType reactType)
    {

        _idleBehavior = _spawner.SpawnIdleBehavior(idleType, this, _points);
        _reactBehavior = _spawner.SpawnReactBehavior(reactType, this, _effect, this.transform);

        _currentIdleType = idleType;
        _previousIdleType = _currentIdleType;

        _currentReactType = reactType;
        _previousReactType = _currentReactType;


        SetIdleBehavior();

    }

    public void ChangeIdleSubState(EnemyIdleBehaviorType newType)
    {


        _idleBehavior = _spawner.SpawnIdleBehavior(newType, this, _points);
        _currentBehavior = _idleBehavior;
        _currentBehavior.Enter();
        Debug.Log($"{name}:  {newType}に変更");

    }

    public void ChangeReactSubState(EnemyReactBehaviorType newType)
    {


        _reactBehavior = _spawner.SpawnReactBehavior(newType, this, _effect, this.transform);
        _currentBehavior = _reactBehavior;
        _currentBehavior.Enter();
        Debug.Log($"{name}: {newType}に変更");
    }


    public void SetIdleBehavior()
    {

        if (_idleBehavior == null)
            throw new ArgumentException("idleBehavior 指定されてません!");
        else
            _currentBehavior = _idleBehavior;
        _currentBehavior?.Enter();
    }

    public void SetReactBehavior()
    {
        if (_reactBehavior == null)
            throw new ArgumentException("ReactBehavior 指定されてません!");
        else
            _currentBehavior?.Exit();
        _currentBehavior = _reactBehavior;
        _currentBehavior?.Enter();
    }

    public void OnExit(GameObject player)
    {
        _gameObject = player;

    }
    public void OnEntered(GameObject player)
    {
        _gameObject = player;
    }

    public void ClearTarget() => _targetPlayer = null;



}



