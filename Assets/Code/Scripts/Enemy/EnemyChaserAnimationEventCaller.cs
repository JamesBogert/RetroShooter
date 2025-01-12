using UnityEngine;

public class EnemyChaserAnimationEventCaller : MonoBehaviour
{
    [SerializeField] private EnemyChaser chaser;

    public void CallAttack()
    { 
        chaser.Attack();
    }
}
