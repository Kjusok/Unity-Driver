using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameManager _gameManager;

    private Quaternion _startRotation;
    private Vector3 _offset;


    private void Start()
    {
        _offset = _playerTarget.InverseTransformPoint(transform.position);
        _startRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (_gameManager.GameIsPaused)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position,
            _playerTarget.position + transform.rotation * _offset,
            _moveSpeed * Time.fixedDeltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation,
            _playerTarget.rotation * _startRotation,
            _rotationSpeed * Time.fixedDeltaTime);
    }
}
