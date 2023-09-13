using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{

    private float enemyHealth = 100;
    private int enemyXP = 30;
    public GameObject damageText;
    public GameObject xpText;
    public GameObject player;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private Material spriteMaterial;
    [SerializeField] private SpriteRenderer slimeGFX;
    public float flashTimer = 0;

    public void LoseHP(float damage){

        slimeGFX.material = flashMaterial;
        slimeGFX.material = spriteMaterial;

        enemyHealth -= damage;
        FlyingDamage(damage);
        source.PlayOneShot(clip);
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

    void FlyingDamage(float damage){
        GameObject floatingPoint =  Instantiate(damageText, transform.position, transform.rotation);
        floatingPoint.GetComponent<TextMeshPro>().text = "- " + damage.ToString();

        GameObject.Destroy(floatingPoint, 0.5f);
    }
}
