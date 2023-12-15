using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float enemyHealth;
    [SerializeField] private int enemyXP;

    private Vector3 offset;
    public GameObject damageText;
    public GameObject xpText;
    public GameObject player;

    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = new Vector3(0, 1f);
    }

    public void LoseHP(float damage){
        enemyHealth -= damage;
        SpawnDamageText(damage);

        source.PlayOneShot(clip);

        GetComponent<EntityChangeColor>().ChangeColor();

        if(enemyHealth <= 0)
        {
            EnemyDie();
            SpawnXPText(enemyXP);
        } 
    }

    private void EnemyDie(){
        player.GetComponent<XPManager>().GainXP(enemyXP);
        SpawnDeathParticles();
        GameObject.Destroy(this.gameObject);
    }

    private void SpawnDeathParticles(){
        GameObject particles = Instantiate(deathParticles, transform);
        particles.transform.parent = null;
        GameObject.Destroy(particles, 6f);
    }

    private void SpawnXPText(int enemyXP){
        GameObject floatingPoint =  Instantiate(xpText, transform.position + offset, transform.rotation);
        floatingPoint.GetComponent<TextMeshPro>().text = "+ " + enemyXP.ToString() + "XP";

        GameObject.Destroy(floatingPoint, 1f);
    }

    void SpawnDamageText(float damage){
        GameObject floatingPoint =  Instantiate(damageText, transform.position, transform.rotation);
        floatingPoint.GetComponent<TextMeshPro>().text = "- " + damage.ToString();

        GameObject.Destroy(floatingPoint, 0.5f);
    }
}
