using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementForceMaker : MonoBehaviour
{
    [SerializeField] private CarController _carController;

    private float _valueForMovement;

    public ButtonClickChecker UpArrow;
    public ButtonClickChecker DownArrow;


    private void FixedUpdate()
    {
        if (UpArrow.IsDown)
        {
            _valueForMovement += Time.fixedDeltaTime * 2;

            if (_valueForMovement >= 1)
            {
                _valueForMovement = 1;
            }
            _carController.CreateVectorForce(_valueForMovement);

        }
        else if (DownArrow.IsDown)
        {
            _valueForMovement -= Time.fixedDeltaTime * 2;

            if (_valueForMovement <= -1)
            {
                _valueForMovement = -1;
            }
            _carController.CreateVectorForce(_valueForMovement);

        }
        else if(!UpArrow.IsDown && !DownArrow.IsDown)
        {
            _valueForMovement = 0;
            _carController.CreateVectorForce(Input.GetAxis("Vertical"));
        }
    }
}
