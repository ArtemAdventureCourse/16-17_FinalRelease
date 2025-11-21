using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 本クラスは、時間の経過を管理するタイマー機能を提供します。
/// </summary>
/// <remarks>
/// 主な機能:
/// - タイマーの開始
/// - タイマーの進行状況の更新
/// - タイマー終了時のリソース解放
/// 
/// 使用方法:
/// - ゲーム内やアプリ内で一定時間の処理やイベントを管理する際に使用します。
/// - タイマーは自動的に進行し、不要になった場合はリソースが解放されます。
/// </remarks>
public class TimerManager : MonoBehaviour
{
    [SerializeField] GameObject _gameobjectPlayer;
   private Timer _timer;
    [SerializeField] CoinCollector _collector;

    private void Awake()
    {
        _timer = new Timer();
    }

    private void Start()
    {
      _timer.Start();   
    }

    private void Update()
    {
        _timer.Tick();

        if (_timer.IsFinished() && _collector.HasFullCoin==false)
        {
            Destroy(_gameobjectPlayer);
        }
    }

    private void OnDestroy()
    {
        _timer?.Dispose();
        _timer=null;
    }

}
