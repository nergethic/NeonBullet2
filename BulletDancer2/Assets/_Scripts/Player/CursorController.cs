using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour {
    [SerializeField] Transform cursor;
    [SerializeField] Texture2D cursorTexture;

    void Awake() {
        SetCursor(cursorTexture);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void SetCursor(Texture2D cursorTexture) {
        Vector2 spawnPoint = new Vector2(cursorTexture.width / 2.0f, cursorTexture.height / 2.0f);
        Cursor.SetCursor(cursorTexture, spawnPoint, CursorMode.Auto);
    }
    
    void Update() {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        
        var newPos = cursor.position;
        newPos.x = mousePos.x;
        newPos.z = mousePos.y;
        //cursor.position = newPos;
        //Debug.Log(mousePos.x);
    }
}
