using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;
    [SerializeField] [Range(0, 1)] private float _helpValue;

    private Rigidbody _rb;
    private float _lastRotationY;
    private Vector3 _movementVector;
    public bool _onGround;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnCollisionExit(Collision other)
    {
        _onGround = false;
    }

    private void OnCollisionStay(Collision other)
    {
        _onGround = true;
    }

    private void SteerHelpAssist()
    {
        if (!_onGround)
        {
            return;
        }

        if (Mathf.Abs(transform.rotation.eulerAngles.y - _lastRotationY) < 10f)
        {
            float turnAdjust = (transform.rotation.eulerAngles.y - _lastRotationY) * _helpValue;
            Quaternion rotateHelp = Quaternion.AngleAxis(turnAdjust, Vector3.up);
            _rb.velocity = rotateHelp * _rb.velocity;
        }

        _lastRotationY = transform.rotation.eulerAngles.y;
    }

    private void CreateVectorForce()
    {
        var vertical = Input.GetAxis("Vertical");

        float angelY = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float cos = Mathf.Cos(angelY);
        float sin = Mathf.Sin(angelY);

        _movementVector = new Vector3(vertical * sin, 0.0f, vertical * cos);
    }

    private void Rotation()
    {
        var horizontal = Input.GetAxis("Horizontal");

        if (_rb.velocity.sqrMagnitude > 1 && _onGround)
        {
            transform.Rotate(0.0f, horizontal * _speedRotation, 0.0f, Space.World);
        }
    }

    private void AplyForceToMovementCar()
    {
        if (_onGround)
        {
            _rb.AddForce(_movementVector * _speed, ForceMode.Impulse);
        }
    }
    
    private void FixedUpdate()
    {
        CreateVectorForce();
        Rotation();
        AplyForceToMovementCar();
        SteerHelpAssist();
    }
}
