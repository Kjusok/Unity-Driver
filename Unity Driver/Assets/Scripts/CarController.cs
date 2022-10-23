using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;
    [SerializeField] [Range(0, 1)] private float _helpValue;
    [SerializeField] private float _minSpeedForSmoke;
    [SerializeField] private float _minAngelForSmoke;
    [SerializeField] private ParticleSystem[] _tireSmokeEffects;
    [SerializeField] private GameObject _nitroEffects;
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Text _speedometer;

    private Rigidbody _rb;
    private float _lastRotationY;
    private Vector3 _movementVector;
    private bool _onGround;

    private const float _maxSpeedForSmokeOnStart = 12f;
    private const float _minSpeedForSmokeOnStart = 3f;
    private const float _correctionSpeed = 2.7f;
    private const float _limitForSteerHelpAssist = 10f;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerOfMass.localPosition;
    }

    private void OnCollisionExit(Collision other)
    {
        _onGround = false;
    }

    private void OnCollisionStay(Collision other)
    {
        _onGround = true;
    }

    private void CreateVectorForce()
    {
        var vertical = Input.GetAxis("Vertical");

        float angelY = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float angelZ = transform.rotation.eulerAngles.x * Mathf.Deg2Rad;
        float cos = Mathf.Cos(angelY);
        float sin = Mathf.Sin(angelY);
        float sinZ = Mathf.Sin(angelZ);

        _movementVector = new Vector3(vertical * sin, -sinZ * vertical, vertical * cos);
    }

    private void ApplyForceToMovementCar()
    {
        if (_onGround)
        {
            _rb.AddForce(_movementVector * _speed, ForceMode.Impulse);
        }
    }

    private void SteerHelpAssist()
    {
        if (!_onGround)
        {
            return;
        }

        if (Mathf.Abs(transform.rotation.eulerAngles.y - _lastRotationY) < _limitForSteerHelpAssist)
        {
            float turnAdjust = (transform.rotation.eulerAngles.y - _lastRotationY) * _helpValue;
            Quaternion rotateHelp = Quaternion.AngleAxis(turnAdjust, Vector3.up);
            _rb.velocity = rotateHelp * _rb.velocity;
        }

        _lastRotationY = transform.rotation.eulerAngles.y;
    }
   
    private void Rotation()
    {
        var horizontal = Input.GetAxis("Horizontal");

        if (_rb.velocity.sqrMagnitude > 1 && _onGround)
        {
            transform.Rotate(0.0f, horizontal * _speedRotation, 0.0f, Space.World);
        }
    }

    private void NitroEffects()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _nitroEffects.SetActive(true);
        }
        else
        {
            _nitroEffects.SetActive(false);
        }
    }

    private void EmitSmokeFromTires()
    {
        if (_rb.velocity.magnitude > _minSpeedForSmoke && _onGround)
        {
            float angel = Quaternion.Angle(Quaternion.LookRotation(_rb.velocity, Vector3.up),
                Quaternion.LookRotation(transform.forward,
                Vector3.up));

            if (angel > _minAngelForSmoke)
            {
                SwitchSmokeParticles(true);
            }
            else
            {
                SwitchSmokeParticles(false);
            }
        }
        else
        {
            SwitchSmokeParticles(false);
        }
    }

    private void SmokeFromTiresOnStart()
    {
        if (_rb.velocity.magnitude < _maxSpeedForSmokeOnStart &&
            _rb.velocity.magnitude > _minSpeedForSmokeOnStart &&
            Input.GetKey(KeyCode.UpArrow) &&
            _onGround)
        {
            SwitchSmokeParticles(true);
        }
    }

    private void SwitchSmokeParticles(bool _enable)
    {
        foreach (ParticleSystem ps in _tireSmokeEffects)
        {
            ParticleSystem.EmissionModule psEm = ps.emission;
            psEm.enabled = _enable;
        }
    }

    private void Speedometer()
    {
        var speed = Mathf.Round(_rb.velocity.magnitude * _correctionSpeed);
        _speedometer.text = speed.ToString();
    }

    private void FixedUpdate()
    {
        CreateVectorForce();
        EmitSmokeFromTires();
        SmokeFromTiresOnStart();
        Rotation();
        ApplyForceToMovementCar();
        SteerHelpAssist();
        NitroEffects();
        Speedometer();
    }
}
