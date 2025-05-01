using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MobileInput : MonoBehaviour, IGameplayInput
{
    [Header("UI Elements")]
    [SerializeField] private FixedJoystick aimJoystick;
    [SerializeField] private Button fireButton;
    [SerializeField] private float pinchSpeed = 0.2f;

    private bool firePressed;
    private float zoomValue;
    private float prevTouchDist;

    void Start()
    {
        fireButton.onClick.AddListener(() => firePressed = true);
    }

    void Update()
    {
        zoomValue = 0f;

        if (Input.touchCount == 2)
        {
            var t0 = Input.GetTouch(0);
            var t1 = Input.GetTouch(1);
            float curDist = Vector2.Distance(t0.position, t1.position);

            if (prevTouchDist > 0f)
            {
                float delta = curDist - prevTouchDist;
                zoomValue = delta * pinchSpeed * Time.deltaTime;
            }

            prevTouchDist = curDist;
        }
        else
        {
            prevTouchDist = 0f;
        }
    }

    public Vector2 Aim => new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);

    public bool Fire
    {
        get
        {
            if (firePressed)
            {
                firePressed = false;
                return true;
            }
            return false;
        }
    }

    public float Zoom => zoomValue;
}
