using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Threading;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float enemyHealth;
    [SerializeField] private Slider slider;
    [SerializeField] private RectTransform sliderRect;
    [SerializeField] private GameObject enemyHealthBar;

    public GameObject damageText;
    public GameObject player;

    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    [SerializeField] private int chanceOfDropingItems;
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject arrow;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        slider.maxValue = enemyHealth;
        UpdateSlider();
        SetSliderLength();
    }

    public void LoseHP(float damage){
        
        enemyHealth -= damage;
        SpawnDamageText(damage);

        UpdateSlider();

        source.PlayOneShot(clip);

        GetComponent<EntityChangeColor>().ChangeColor();

        if(enemyHealth <= 0)
        {
            EnemyDie();
        } 
    }

    private void EnemyDie(){
        SpawnDeathParticles();
        SpawnItems();
        GameObject.Destroy(this.gameObject);
    }

    private void SpawnDeathParticles(){
        GameObject particles = Instantiate(deathParticles, transform);
        particles.transform.parent = null;
        GameObject.Destroy(particles, 6f);
    }

    void SpawnDamageText(float damage){
        GameObject floatingPoint =  Instantiate(damageText, transform.position, transform.rotation);
        floatingPoint.GetComponent<TextMeshPro>().text = "- " + damage.ToString();

        GameObject.Destroy(floatingPoint, 0.5f);
    }

    private void UpdateSlider(){
        slider.value = enemyHealth;
        enemyHealthBar.SetActive(true);
    }

    private void SetSliderLength(){
        sliderRect.sizeDelta = new Vector2(enemyHealth * 3, 12.4f);
    }

    private void SpawnItems(){
        for(int i = 0; i < chanceOfDropingItems / 2; i++){
            if(Random.Range(0, 100) == 1){
                GameObject item = Instantiate(heart, transform.position, transform.rotation);
                item.transform.parent = null;
                item.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f,1f), 3f), ForceMode2D.Impulse);
            }
        }

        for(int i = 0; i < chanceOfDropingItems; i++){
            if(Random.Range(0, 100) == 1){
                GameObject item = Instantiate(arrow, transform.position, transform.rotation);
                item.transform.parent = null;
                item.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f,1f), 3f), ForceMode2D.Impulse);
            }
        }
    }
}
