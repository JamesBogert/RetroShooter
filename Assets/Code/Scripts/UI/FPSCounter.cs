using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI fpsText;
    [SerializeField] float updateRate = 0.05f;
    private float time;
    private int numFrames;

    private void Awake()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        numFrames = 0;
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > updateRate)
        {
            fpsText.text = Mathf.Round(numFrames / time) + "fps";

            numFrames = 0;
            time = 0f;
        }

        numFrames += 1;
        time += Time.deltaTime;
    }
}
