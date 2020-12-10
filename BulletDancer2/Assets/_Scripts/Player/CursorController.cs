using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour {
    [SerializeField] Transform cursor;
    [SerializeField] Texture2D cursorTexture;
    // Start is called before the first frame update
    void Awake() {
        SetCursor(cursorTexture);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void SetCursor(Texture2D cursorTexture) {
        Debug.LogError(cursorTexture.format);
        Vector2 spawnPoint = new Vector2(cursorTexture.width / 2.0f, cursorTexture.height / 2.0f);
        Cursor.SetCursor(cursorTexture, spawnPoint, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update() {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        
        var newPos = cursor.position;
        newPos.x = mousePos.x;
        newPos.z = mousePos.y;
        //cursor.position = newPos;
        //Debug.Log(mousePos.x);
    }
}
