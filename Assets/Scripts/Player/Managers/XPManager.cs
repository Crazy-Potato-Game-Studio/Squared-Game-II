using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPManager : MonoBehaviour
{

    public GameObject XPSlider;
    public GameObject player;
    public float PlayerXP;
    public float XPToNextLvl = 10;
    public int playerLvl = 1;
    public AudioClip newLvl;

    [SerializeField] private TextMeshProUGUI XPText;

    [SerializeField] GameObject newLevelText;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerXP = 0;
        XPSlider.GetComponent<Slider>().value = PlayerXP;
        XPSlider.GetComponent<Slider>().maxValue = XPToNextLvl;
    }

    public void GainXP(int enemyXP){
        PlayerXP += enemyXP;
        XPSlider.GetComponent<Slider>().value = PlayerXP;
        Debug.Log(PlayerXP);
        if(PlayerXP >= XPToNextLvl){
            NextLvl();
        }

        XPText.text = PlayerXP.ToString() + "/" + XPToNextLvl.ToString();    
    }

    void NextLvl(){
        PlayerXP -= XPToNextLvl;
        XPToNextLvl *= 1.5f;
        XPToNextLvl = Mathf.RoundToInt((float)XPToNextLvl);
        playerLvl ++;

        XPSlider.GetComponent<Slider>().maxValue = XPToNextLvl;
        XPSlider.GetComponent<Slider>().value = PlayerXP;

        GetComponent<AudioSource>().PlayOneShot(newLvl);
        FloatingXP();
    }

    void FloatingXP(){
        Vector3 offset;
        offset = new Vector3(0, 3f);

        GameObject floatingPoint =  Instantiate(newLevelText, player.transform.position + offset, transform.rotation);
        floatingPoint.GetComponent<TextMeshPro>().text = "New Level!";

        GameObject.Destroy(floatingPoint, 1.5f);
    }
}
