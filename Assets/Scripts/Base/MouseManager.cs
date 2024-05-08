using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManger : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera; //Camera for detecting mouseposition

    private Vector3 lastposition; // last mouseposition

    [SerializeField] private LayerMask placementLayermask; // Layermask to select what to detect on

    public event Action OnClicked, OnExit; //Used to tell when the mouse is clicked (Used by other scripts)

    private void Update() {

        if(Input.GetMouseButtonDown(0))OnClicked?.Invoke(); //Invokes OnClicked when you press your leftmousebutton
        if(Input.GetKeyDown(KeyCode.Escape))OnExit?.Invoke(); //Temp remove later!!
        
    }

    public bool PointerUI()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetMousePosition() //Used to find lastPosition (Used by other scripts)
    {
        Vector3 mousePos = Input.mousePosition; //Mouseposition detection
        mousePos.z = sceneCamera.nearClipPlane; //Prevents to select non renderd objects
        Ray ray = sceneCamera.ScreenPointToRay(mousePos); //Raycasting
        RaycastHit hit; //Detecting raycasthit
        if (Physics.Raycast(ray, out hit, 100, placementLayermask)) { //If it hits prints position to lastposition
            lastposition = hit.point;
        }
        return lastposition; // Returns lastposition
    }
}