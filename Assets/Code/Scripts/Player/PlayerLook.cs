using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    public float sensitivity;
    public Transform Player;
    float xRotation = 0f;
    private Controls controls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new Controls();
        controls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float x = controls.Player.Look.ReadValue<Vector2>().x * sensitivity * Time.deltaTime;
        float y = controls.Player.Look.ReadValue<Vector2>().y * sensitivity * Time.deltaTime;
        xRotation -= y;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        Player.Rotate(Vector3.up * x);
    }
}
