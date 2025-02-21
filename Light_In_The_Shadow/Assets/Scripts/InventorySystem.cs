using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    
    public List<GameObject> itemsInInventory = new List<GameObject>();
    public List<String> idsInInventory = new List<string>();
    public GameObject descriptionPanel, rotatableObject, itemsHolder;
    [SerializeField] private GameObject rotatePivot;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;
    [SerializeField] private Camera _uiCamera;

    private Vector3 _mousePreviousPosition = Vector3.zero, _mouseDeltaPostion = Vector3.zero;

    private void Start()
    {
        var dummyGameObject = new GameObject("Dummy");
        dummyGameObject.transform.SetParent(gameObject.transform);
        var item = dummyGameObject.AddComponent<item>();
        item.itemName = item.description= item.id =  "Dummy";
        itemsInInventory.Add(dummyGameObject);
        idsInInventory.Add(item.id);
        item.inInventory = true;
    }

    private void Update()
    {

        
        
        if(!rotatableObject || !descriptionPanel.activeSelf) return;
        if (Input.GetMouseButton(0))
        {
            var cameraRightVector = _uiCamera.transform.right;
            _mouseDeltaPostion = Input.mousePosition - _mousePreviousPosition;
            if (Vector3.Dot(rotatePivot.transform.up, Vector3.up) >= 0)
            {
                rotatableObject.transform.Rotate(rotatePivot.transform.up, Vector3.Dot(_mouseDeltaPostion, cameraRightVector), Space.World);
            }

            else
            {
                rotatableObject.transform.Rotate(rotatePivot.transform.up, -Vector3.Dot(_mouseDeltaPostion, cameraRightVector), Space.World);
            }
        
            rotatableObject.transform.Rotate(cameraRightVector, Vector3.Dot(_mouseDeltaPostion, _uiCamera.transform.up), Space.World);
        }

        _mousePreviousPosition = Input.mousePosition;
        
    }
    
    public void ShowHighlightedItem(GameObject item)
    {
        if (item == rotatableObject) return;
        if(rotatableObject) Destroy(rotatableObject);
        descriptionPanel.transform.GetChild(0).GetComponent<Text>().text = item.GetComponent<item>().itemName;
        descriptionPanel.transform.GetChild(1).GetComponent<Text>().text = item.GetComponent<item>().description;
        descriptionPanel.SetActive(true);
        GameObject meme = Instantiate(item,rotatePivot.transform);
        meme.transform.localPosition = Vector3.zero;
        meme.transform.localScale *= 10;
        meme.layer = 5;
        foreach (var go in meme.GetComponentsInChildren<Transform>()) go.gameObject.layer = 5;
        rotatableObject = meme;
    }
    
    
    public void RemoveItem(string id)
    {
        var indexToRemove = 0;
        var hasItem = false;
        for (int i = 0; i < idsInInventory.Count; i++)
        {
            if (idsInInventory[i] == id)
            {
                indexToRemove = i;
                hasItem = true;
                break;
            }
        }
        if(!hasItem) return;
        idsInInventory.Remove(idsInInventory[indexToRemove]);
        itemsInInventory.Remove(itemsInInventory[indexToRemove]);
        foreach (var item in itemsHolder.GetComponentsInChildren<item>())
        {
            if(item.name.Contains(id)) Destroy(item.gameObject);
        }
   
    }
    
 
     
}

