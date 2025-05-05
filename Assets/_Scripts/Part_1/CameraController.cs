using UnityEngine;

namespace Part_1
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _moveSpeed = 0.5f;
        [SerializeField] private float _zoomSpeed;

        private Vector3 _lastMousePosition;
        private float _distance = 20f;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            _distance = Vector3.Distance(transform.position, Vector3.zero);
            _camera.transform.position = transform.position - transform.forward * _distance;
        }

        private void Update()
        {
            HandleRotation();
            HandlePan();
            HandleZoom();
        }

        private void HandleRotation()
        {
            if (Input.GetMouseButton(0))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                transform.Rotate(Vector3.up, mouseX * _rotationSpeed, Space.World);
                transform.Rotate(Vector3.right, -mouseY * _rotationSpeed, Space.Self);
            }
        }

        private void HandlePan()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(1))
            {
                Vector3 delta = Input.mousePosition - _lastMousePosition;
                Vector3 move = new Vector3(-delta.x, -delta.y, 0) * _moveSpeed * Time.deltaTime;
                transform.Translate(move, Space.Self);
                _lastMousePosition = Input.mousePosition;
            }
        }

        private void HandleZoom()
        {
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE_WIN
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                if (scroll < 0)
                {
                    _camera.transform.position = transform.position - transform.forward * _zoomSpeed;
                }
                else
                {
                    _camera.transform.position = transform.position + transform.forward * _zoomSpeed;
                }
            }
#elif UNITY_ANDROID || UNITY_IOS
        // Zoom (pinch)
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (prevTouch0 - prevTouch1).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = prevMagnitude - currentMagnitude;

            _distance += difference * _zoomSpeed * Time.deltaTime;
            _camera.transform.position = transform.position - transform.forward * _distance;
        }
#endif
        }
    }
}