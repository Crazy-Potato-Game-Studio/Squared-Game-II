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

    [SerializeField] private int minAmountOfDroppedItems;
    [SerializeField] private int maxAmountOfDroppedItems;
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
        for(int i = 0; i < Random.Range(minAmountOfDroppedItems, maxAmountOfDroppedItems); i++){
            int itemNumber;
            itemNumber = Random.Range(1,2);
            if(itemNumber == 1){
                GameObject currentObj = Instantiate(heart, transform);
                currentObj.transform.parent = null;
            }else{
                GameObject currentObj = Instantiate(arrow, transform);
                currentObj.transform.parent = null;
            }
            
        }
    }
}
