using UnityEngine;
using Zenject;
using System;

public class Projectile : MonoBehaviour
{
    [Inject] private readonly ProjectileSettings _settings;

    public class Pool : MonoMemoryPool<Projectile>
    {
        protected override void Reinitialize(Projectile item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(Projectile item)
        {
            item.gameObject.SetActive(false);
        }
    }

    [Serializable]
    public struct ProjectileSettings
    {
        public float Speed;
        public float Lifetime;
    }

    private Vector3 _direction;
    private Action<Projectile> _onReturn;
    private float _timer;

    public void Initialize(Vector3 pos, Vector3 dir, Action<Projectile> onReturn)
    {
        transform.position = pos;
        transform.forward  = dir;
        _direction         = dir;
        _onReturn          = onReturn;
        _timer             = 0f;
        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.position += _direction * _settings.Speed * Time.deltaTime;

        _timer += Time.deltaTime;
        if (_timer >= _settings.Lifetime)
        {
            ReturnToPool();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TankController>(out var tank))
        {
            tank.TakeHit();
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        _onReturn?.Invoke(this);
    }
}
