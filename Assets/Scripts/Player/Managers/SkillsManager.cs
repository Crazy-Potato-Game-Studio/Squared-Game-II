using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    [SerializeField] int movementSpeedBoost;
    [SerializeField] int maxHealthBoost;
    [SerializeField] int maxManaBoost;
    [SerializeField] int jumpHeightBoost;
    [SerializeField] int attackDamageBoost;

    public int skillPoints;

    public void GainSkillPoints(){
        skillPoints += 5;
    }

    
}
