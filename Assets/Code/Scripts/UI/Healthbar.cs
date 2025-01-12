using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private PlayerInfoSO playerInfo;
    [SerializeField] private float updateRate;
    private float target;

    private void Start()
    {
        healthBar.value = playerInfo.CurrentHealth/playerInfo.MaxHealth;
        target = healthBar.value;
        healthText.text = playerInfo.CurrentHealth + "/" + playerInfo.MaxHealth;
    }

    public void UpdateHealth()
    {
        Debug.Log("updating healthbar");
        target = playerInfo.CurrentHealth/playerInfo.MaxHealth;
        healthText.text = playerInfo.CurrentHealth + "/" + playerInfo.MaxHealth;
    }

    private void Update()
    {
        float current = playerInfo.CurrentHealth;
        float max = playerInfo.MaxHealth;
        target = current / max;
        if (healthBar.value != target)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, target, updateRate * Time.deltaTime);
            healthText.text = playerInfo.CurrentHealth + "/" + playerInfo.MaxHealth;
        }
    }
}
