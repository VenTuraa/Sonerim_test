using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotObjectHighlighter : MonoBehaviour
{
    public ModelsContainer modelsData; // Заполнить из другого скрипта
    public Material highlightMaterial; // Материал подсветки
    public Canvas popupCanvas;         // UI канвас
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    private Material originalMaterial;
    private GameObject lastSelected;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelection();
        }
    }

    public void SetModels(ModelsContainer container)
    {
        modelsData = container;
    }
    
    void HandleSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;

            if (lastSelected != null && originalMaterial != null)
            {
                // Убрать подсветку с предыдущего объекта
                lastSelected.GetComponent<Renderer>().material = originalMaterial;
            }

            // Сохраняем оригинальный материал
            Renderer renderer = clickedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterial = renderer.material;
                renderer.material = highlightMaterial;
                lastSelected = clickedObject;
            }

            string objectName = clickedObject.name;

            // Ищем HotObject по Id
            foreach (var model in modelsData.Models)
            {
                foreach (var hotObj in model.HotObjects)
                {
                    if (hotObj.Id == objectName)
                    {
                        ShowPopup(hotObj.Title, hotObj.Description);
                        return;
                    }
                }
            }

            HidePopup();
        }
    }

    void ShowPopup(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
        popupCanvas.gameObject.SetActive(true);
    }

    void HidePopup()
    {
        popupCanvas.gameObject.SetActive(false);
    }
}
