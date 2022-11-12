using UnityEngine;

public class AcceleratorCar: MonoBehaviour
{
    private const float _correctionValue = 2;

    [SerializeField] private CarController _carController;
    [SerializeField] private ButtonClickChecker _upArrow;
    [SerializeField] private ButtonClickChecker _downArrow;

    public float ValueForMovement
    {
        get; private set;
    }

   
    private void FixedUpdate()
    {
        if (_upArrow.IsDown)
        {
            ValueForMovement = Mathf.Clamp(ValueForMovement + Time.fixedDeltaTime * _correctionValue, 0, 1);
        }
        else if (_downArrow.IsDown)
        {
            ValueForMovement = Mathf.Clamp(ValueForMovement - Time.fixedDeltaTime * _correctionValue, -1, 0);
        }
        else if (!_upArrow.IsDown && !_downArrow.IsDown)
        {
            if (ValueForMovement > 0)
            {
                ValueForMovement -= Time.fixedDeltaTime * _correctionValue;
            }
            else
            {
                ValueForMovement += Time.fixedDeltaTime * _correctionValue;
            }

            ValueForMovement = Input.GetAxis("Vertical");
        }
    }
}
