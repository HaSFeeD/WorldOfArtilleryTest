using UnityEngine;
using Zenject;

public class TankController : MonoBehaviour
{
    [Inject(Id = "TankSpeed")] private float _speed;
    [Inject] private ArtilleryController _artillery;
    [Inject] private Projectile.Pool _projectilePool;
    [Inject] private GameStateMachine _gsm;
    [SerializeField] private Transform turretPivot;

    private Transform _targetTransform;
    private float _attackDistance = 60f;
    private float _elapsedTime = 0f;
    private float _timeToAttack = 3f;
    private bool _isReached = false;
    private bool _hasAttacked = false;
    private bool _alive = true;

    public class Factory : PlaceholderFactory<TankController> {}

    void Start()
    {
        _targetTransform = _artillery.transform;
    }

    void Update()
    {
        if (!_alive) return;
        CheckAttackDistance();

        Vector3 dir = (_targetTransform.position - transform.position).normalized;
        if (!_isReached)
            transform.position += dir * _speed * Time.deltaTime;
        else if (!_hasAttacked)
            Attack();

        turretPivot.rotation = Quaternion.LookRotation(dir);
        dir.y = 0;
        transform.rotation  = Quaternion.LookRotation(dir);
    }

    void CheckAttackDistance()
    {
        if (!_isReached &&
            Vector3.Distance(_targetTransform.position, transform.position) < _attackDistance)
        {
            _isReached = true;
            _elapsedTime = 0f;
        }
    }

    void Attack()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _timeToAttack)
        {
            _hasAttacked = true;
            _gsm.EnterGameOver();
        }
    }

    public void TakeHit()
    {
        _alive = false;
        Destroy(gameObject, 0.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Projectile>(out var p))
            TakeHit();
    }
}
