using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerInfo")]
public class PlayerInfoSO : ScriptableObject
{
    public Vector3 playerPosition;
    public Vector3 playerVelocity;

    public int CurrentHealth;
    public int MaxHealth;
    public bool IsPlayerDead;
}
