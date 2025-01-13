using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InputCaller : MonoBehaviour
{
    void OnEnable()
    {
        InputManager.instance.EnableInput();
    }

    private void Update()
    {
        InputManager.instance.UpdateInput();
    }

    private void OnDisable()
    {
        InputManager.instance.DisableInput();
    }

    
}
