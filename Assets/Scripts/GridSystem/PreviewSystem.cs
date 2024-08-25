using System; //It was my birthday the time i wrote this script (:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewOffset = 0.06f; //Offset to the preview so it doesn't clip through the ground

    [SerializeField] private GameObject cellIndicator; //The indicator of what cell you are in
    private GameObject previewObject; //The object to preview

    [SerializeField] private Material previewMaterialPrefab; //The transparent material
    private Material previewMaterialInstance; //Our material instance so we don't modify the original

    private void Start() //Unity start
    {
        previewMaterialInstance = new Material(previewMaterialPrefab); //Initializes the material
        cellIndicator.SetActive(false); //Sets cellIndicator.active to false
    }

    public void StartPreview(GameObject prefab, Vector2Int size) //Used to start the preview
    {
        previewObject = Instantiate(prefab); //Initializes previewObject
        PrepPreview(previewObject); //Calls prepare preview
        PrepCursor(size); //Calls prepare cursor
    }

    private void PrepCursor(Vector2Int size) //Used to prepare the cellindicator
    {
        if(size.x > 0 || size.y > 0) //If the size is more than zero
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 0, size.y); //Scales the cellindicator to encapsulate the object
        }
    }

    private void PrepPreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            
        }
    }
}
