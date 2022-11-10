using UnityEngine;

public class AcceleratorCar: MonoBehaviour
{
    private const float _correctionValue = 2;

    [SerializeField] private CarController _carController;
    [SerializeField] private ButtonClickChecker _upArrow;
    [SerializeField] private ButtonClickChecker _downArrow;

    private float _valueForMovement;

   
    private void FixedUpdate()
    {

        Debug.Log(Input.touchCount);
        if (_upArrow.IsDown)
        {
            _valueForMovement = Mathf.Clamp(_valueForMovement + Time.fixedDeltaTime * _correctionValue, 0, 1);
            _carController.CreateVectorForce(_valueForMovement);
        }
        else if (_downArrow.IsDown)
        {
            _valueForMovement = Mathf.Clamp(_valueForMovement - Time.fixedDeltaTime * _correctionValue, -1, 0);
            _carController.CreateVectorForce(_valueForMovement);
        }
        else if(!_upArrow.IsDown && !_downArrow.IsDown)
        {
            if (_valueForMovement > 0)
            {
                _valueForMovement -= Time.fixedDeltaTime * _correctionValue;
            }
            else
            {
                _valueForMovement += Time.fixedDeltaTime * _correctionValue;
            }

            _carController.CreateVectorForce(Input.GetAxis("Vertical"));
        }
    }
}
