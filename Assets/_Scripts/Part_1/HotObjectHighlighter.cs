using UnityEngine;

namespace Part_1
{
    public class HotObjectHighlighter : MonoBehaviour
    {
        [SerializeField] private Material _highlightMaterial;
        [SerializeField] private InfoPopupView _infoPopup;

        private Material _originalMaterial;
        private GameObject _lastSelected;
        private ModelsContainer _modelsData;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleSelection();
            }
        }

        public void SetModels(ModelsContainer container)
        {
            _modelsData = container;
        }

        private void HandleSelection()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (_lastSelected != null && _originalMaterial != null)
                {
                    _lastSelected.GetComponent<Renderer>().material = _originalMaterial;
                }

                Renderer renderer = clickedObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    _originalMaterial = renderer.material;
                    renderer.material = _highlightMaterial;
                    _lastSelected = clickedObject;
                }

                string objectName = clickedObject.name;

                foreach (var model in _modelsData.Models)
                {
                    foreach (var hotObj in model.HotObjects)
                    {
                        if (hotObj.Id == objectName)
                        {
                            _infoPopup.ShowPopup(hotObj.Id, hotObj.Title, hotObj.Description);
                            return;
                        }
                    }
                }

                _infoPopup.HidePopup();
            }
        }
    }
}