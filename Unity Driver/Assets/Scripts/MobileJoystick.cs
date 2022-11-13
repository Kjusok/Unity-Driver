using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform _joystickTransform;
    [SerializeField] private CarController _carController;

    [SerializeField] private float _dragThreshold = 0.6f;
    [SerializeField] private int _dragMovementDistance = 30;
    [SerializeField] private float _dragOffsetDistance = 100;

    private Vector2 _inputVector;


    private Vector2 CalculateMovementInput(Vector2 offset)
    {
        float x = Mathf.Abs(offset.x) > _dragThreshold ? offset.x : 0;
        float y = Mathf.Abs(offset.y) > _dragThreshold ? offset.y : 0; 
        return new Vector2(x, y);
    }

    public float Value
    {
        get
        {
            if (_inputVector.x != 0 || _inputVector.y != 0)
            {
                return _inputVector.x;
            }
            else
            {
                return Input.GetAxis("Horizontal");
            }
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _joystickTransform.anchoredPosition = Vector2.zero;
        _inputVector.x = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickTransform,  eventData.position, null, out offset);
        offset = Vector2.ClampMagnitude(offset, _dragOffsetDistance) / _dragOffsetDistance;
        _joystickTransform.anchoredPosition = new Vector2(offset.x * _dragMovementDistance, 0);

        _inputVector = CalculateMovementInput(offset);
    }
}

