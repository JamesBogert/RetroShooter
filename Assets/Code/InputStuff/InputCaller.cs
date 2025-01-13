using UnityEngine;

public class InputCaller : MonoBehaviour
{
    private void Update()
    {
        InputManager.instance.UpdateInput();
    }
}
