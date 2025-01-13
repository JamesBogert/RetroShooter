using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool paused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paused = false;
        //pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.IsPause)
        {
            if (paused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
                paused = false;
            } 
            else
            {
                Cursor.lockState= CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
                paused = true;
            }
        
        }
    }
}
