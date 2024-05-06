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
    [HideInInspector] public GameObject player;

    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    [SerializeField] private int minAmountOfDroppedItems;
    [SerializeField] private int maxAmountOfDroppedItems;
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject arrow;

    [SerializeField] private bool isBoss;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        slider.maxValue = enemyHealth;
        UpdateSlider();
        if(!isBoss){
            enemyHealthBar.SetActive(false);
        }
        SetSliderLength();
    }

    public void LoseHP(float damage){
        
        enemyHealth -= damage;
        SpawnDamageText(damage);

        UpdateSlider();

        source.PlayOneShot(clip);

        GetComponent<EntityChangeColor>().ChangeColor(1f);

        if(enemyHealth <= 0)
        {
            EnemyDie();
        } 
    }

    private void EnemyDie(){
        if(GetComponent<SlimeKing>()){
            GetComponent<SlimeKing>().OpenTheDoors();
        }
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
        if(!isBoss){
            StopAllCoroutines();
            StartCoroutine(HideHealthBar());
        }
    }

    IEnumerator HideHealthBar(){
        yield return new WaitForSeconds(3f);
        enemyHealthBar.SetActive(false);
    }

    private void SetSliderLength(){
        if(!isBoss){
            sliderRect.sizeDelta = new Vector2(enemyHealth * 3, 12.4f);
        }
    }

    private void SpawnItems(){
        for(int i = 0; i < Random.Range(minAmountOfDroppedItems, maxAmountOfDroppedItems+1); i++){
            int itemNumber;
            itemNumber = Random.Range(1,3);
            Debug.Log(itemNumber);
            if(itemNumber == 1){
                GameObject currentObj = Instantiate(heart, transform);
                currentObj.transform.parent = null;
            }else if(itemNumber == 2){
                GameObject currentObj = Instantiate(arrow, transform);
                currentObj.transform.parent = null;
            }
            
        }
    }
}
