using UnityEngine;

public class Effect:MonoBehaviour
{
    
   [SerializeField ]private  ParticleSystem _particleSystem;

    private void PlayEffect(GameObject gameObject)=> _particleSystem.Play(gameObject);

    public void Play(GameObject gameObject)  => PlayEffect(gameObject); 

    private void SetPositionForFx(GameObject gameObject,Transform transform) => transform.position = gameObject.transform.position;

    public void SetPosition( GameObject gameObject,Transform transform) => SetPositionForFx(gameObject,transform);
}
