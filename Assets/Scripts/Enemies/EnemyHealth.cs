using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{

    private int enemyHealth = 100;
    private int enemyXP = 30;
    public GameObject damageText;
    public GameObject xpText;
    public GameObject player;

    public void LoseHP(int damage){
        enemyHealth -= damage;
        FlyingDamage(damage);
        if(enemyHealth <= 0)
        {
            EnemyDie();
            FlyingXP(enemyXP);
        } 
    }

    void EnemyDie(){
        GameObject.Destroy(gameObject);
        player.GetComponent<XPManager>().GainXP(enemyXP);
    }

    void FlyingXP(int enemyXP)
    {
        Vector3 offset;
        offset = new Vector3(0, 1f);
        GameObject floatingPoint =  Instantiate(xpText, transform.position + offset, transform.rotation);
        floatingPoint.GetComponent<TextMeshPro>().text = "+ " + enemyXP.ToString() + "XP";

        GameObject.Destroy(floatingPoint, 0.5f);
    }

    void FlyingDamage(int damage){
        GameObject floatingPoint =  Instantiate(damageText, transform.position, transform.rotation);
        floatingPoint.GetComponent<TextMeshPro>().text = "- " + damage.ToString();

        GameObject.Destroy(floatingPoint, 0.5f);
    }
}
