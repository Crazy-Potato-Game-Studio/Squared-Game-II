using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float enemyHealth;
    [SerializeField] private int enemyXP;
    public GameObject damageText;
    public GameObject xpText;
    public GameObject player;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void LoseHP(float damage){
        enemyHealth -= damage;
        FlyingDamage(damage);

        source.PlayOneShot(clip);

        GetComponent<EntityChangeColor>().ChangeColor();

        if(enemyHealth <= 0)
        {
            EnemyDie();
            FlyingXP(enemyXP);
        } 
    }

    void EnemyDie(){
        player.GetComponent<XPManager>().GainXP(enemyXP);
        GameObject particles = Instantiate(deathParticles, transform);
        particles.transform.parent = null;
        GameObject.Destroy(particles, 6f);
        GameObject.Destroy(this.gameObject);
    }

    void FlyingXP(int enemyXP)
    {
        Vector3 offset;
        offset = new Vector3(0, 1f);
        GameObject floatingPoint =  Instantiate(xpText, transform.position + offset, transform.rotation);
        floatingPoint.GetComponent<TextMeshPro>().text = "+ " + enemyXP.ToString() + "XP";

        GameObject.Destroy(floatingPoint, 1f);
    }

    void FlyingDamage(float damage){
        GameObject floatingPoint =  Instantiate(damageText, transform.position, transform.rotation);
        floatingPoint.GetComponent<TextMeshPro>().text = "- " + damage.ToString();

        GameObject.Destroy(floatingPoint, 0.5f);
    }
}