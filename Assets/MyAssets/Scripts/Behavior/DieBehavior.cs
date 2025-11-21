using UnityEngine;

public class DieBehavior : IBehavior
{

    private readonly GameObject _gameObject;
    private readonly Effect _effect;

    public string Name => "name die";
    public DieBehavior(GameObject gameObject, Effect effect)
    {
        _gameObject = gameObject;
        _effect = effect;
    }

    public void Enter() {  }

    private void SetEffect()
    {
        _effect.SetPosition(_gameObject, _effect.transform);
        _effect.Play(_gameObject);
    }

    private void Destroy()
    {
        _gameObject.SetActive(false);
    }

    public void Update()
    {
        SetEffect();
       // Destroy();
    }

    public void Exit()
    {

    }
}
