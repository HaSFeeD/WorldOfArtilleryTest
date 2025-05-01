using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class ArtilleryController : MonoBehaviour
{
    [Inject] private IGameplayInput _input;

    [Header ("UI")]
    [SerializeField] private GameObject _zoomSliderGameobject;
    [SerializeField] private Button _zoomButton;
    private Slider _zoomSlider;

    [Header("Rotation and Zoom")]
    [SerializeField] private Transform turretPivot;
    [SerializeField] private Camera artilleryCamera;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float minFOV = 15f, maxFOV = 60f;

    [Header("Fire")]
    [Inject] private Projectile.Pool _projectilePool;
    [SerializeField] private float reloadTime = 3f;
    private float _prevZoomValue;
    private bool _canFire = true;
    private bool _inZoom = false;
        private void Start()
    {
        _zoomSlider = _zoomSliderGameobject.GetComponentInChildren<Slider>();
        _zoomButton.onClick.AddListener(Zoom);

        _zoomSlider.onValueChanged.AddListener(val =>
        {
            artilleryCamera.fieldOfView = Mathf.Lerp(
                maxFOV,
                minFOV,
                val
            );
        });
    }


    void Update()
    {
        HandleAim();
        if(_inZoom){
            HandleZoom();
        }
        HandleFire();
    }

    private void HandleAim()
    {
        var aim = _input.Aim;
        transform.Rotate(0, aim.x * rotationSpeed * Time.deltaTime, 0);
        turretPivot.Rotate(-aim.y * rotationSpeed * Time.deltaTime, 0, 0);
    }

    private void HandleZoom()
    {
        float current = _zoomSlider.value;
        float delta   = (current - _prevZoomValue) * zoomSpeed * Time.deltaTime;

        artilleryCamera.fieldOfView = Mathf.Clamp(
            artilleryCamera.fieldOfView - delta,
            minFOV, maxFOV
        );

        _prevZoomValue = current;
    }


    private void HandleFire()
    {
        if (_input.Fire && _canFire)
        {
            Fire();
        }
    }

    private void Fire()
    {
        _canFire = false;
        var proj = _projectilePool.Spawn();
        proj.Initialize(turretPivot.position, turretPivot.forward, OnProjectileReturned);

        Invoke(nameof(ResetReload), reloadTime);
    }
    private void Zoom(){
        if (_inZoom)
        {
            ExitZoom();
        }
        else
        {
            EnterZoom();
        }
    }
    private void ExitZoom(){
        _zoomSliderGameobject.SetActive(false);
        _inZoom = false;
    }
    private void EnterZoom()
    {
        _zoomSliderGameobject.SetActive(true);
        _inZoom = true;

        _prevZoomValue = _zoomSlider.value;
    }


    private void ResetReload()
    {
        _canFire = true;
    }

    private void OnProjectileReturned(Projectile p)
    {
        _projectilePool.Despawn(p);
    }
    public class Factory : PlaceholderFactory<ArtilleryController> {}
}
