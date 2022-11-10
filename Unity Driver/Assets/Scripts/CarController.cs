using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    private const float _maxSpeedForSmokeOnStart = 12f;
    private const float _minSpeedForSmokeOnStart = 3f;
    private const float _correctionSpeed = 2.7f;
    private const float _limitForSteerHelpAssist = 10f;

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

    public ButtonClickChecker UpArrow;


    private void Awake()
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

    private void OnTriggerEnter(Collider other)
    {
        var finish = other.GetComponent<Finish>();

        if (finish)
        {
            GameManager.Instance.AddCurrentLap();
        }
    }
    private void FixedUpdate()
    {
        EmitSmokeFromTires();

        if (GameManager.Instance.GameIsPaused)
        {
            return;
        }

        ApplyForceToMovementCar();
        AddSkidOffset();
    }

    private void Update()
    {
        SmokeFromTiresOnStart();
        NitroEffects();
        Speedometer();
    }

    private void ApplyForceToMovementCar()
    {
        if (_onGround)
        {
            _rb.AddForce(_movementVector * _speed, ForceMode.Impulse);
        }
    }

    private void AddSkidOffset()
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

    private void NitroEffects()
    {
        _nitroEffects.SetActive(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || UpArrow.IsDown);
    }

    private void EmitSmokeFromTires()
    {
        if (_rb.velocity.magnitude > _minSpeedForSmoke && _onGround)
        {
            float angel = Quaternion.Angle(Quaternion.LookRotation(_rb.velocity, Vector3.up),
                Quaternion.LookRotation(transform.forward,
                Vector3.up));

            SwitchSmokeParticles(angel > _minAngelForSmoke);
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
            (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || UpArrow.IsDown) &&
            _onGround)
        {
            SwitchSmokeParticles(true);
        }
    }

    private void SwitchSmokeParticles(bool enable)
    {
        foreach (ParticleSystem particle in _tireSmokeEffects)
        {
            ParticleSystem.EmissionModule ParticleSysEmissionModule = particle.emission;
            ParticleSysEmissionModule.enabled = enable;
        }
    }

    private void Speedometer()
    {
        var speed = Mathf.Round(_rb.velocity.magnitude * _correctionSpeed);
        _speedometer.text = speed.ToString();
    }

    public void CreateVectorForce(float vertical)
    {
        float angelY = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float angelZ = transform.rotation.eulerAngles.x * Mathf.Deg2Rad;
        float cos = Mathf.Cos(angelY);
        float sin = Mathf.Sin(angelY);
        float sinZ = Mathf.Sin(angelZ);

        _movementVector = new Vector3(vertical * sin, -sinZ * vertical, vertical * cos);
    }

    public void Rotation(float horizontal)
    {
        if (_rb.velocity.sqrMagnitude > 1 && _onGround)
        {
            transform.Rotate(0.0f, horizontal * _speedRotation, 0.0f, Space.World);
        }
    }
}
