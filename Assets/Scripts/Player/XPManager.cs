using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPManager : MonoBehaviour
{

    public GameObject XPSlider;
    public float PlayerXP;
    public float XPToNextLvl = 10;
    public int playerLvl = 1;
    public AudioClip newLvl;

    [SerializeField] GameObject newLevelText;
    private void Start() {
        PlayerXP = 0;
        XPSlider.GetComponent<Slider>().value = PlayerXP;
        XPSlider.GetComponent<Slider>().maxValue = XPToNextLvl;
    }

    public void GainXP(int enemyXP){
        PlayerXP += enemyXP;
        if(PlayerXP >= XPToNextLvl){
            NextLvl();
            XPSlider.GetComponent<Slider>().maxValue = XPToNextLvl;
        }
        XPSlider.GetComponent<Slider>().value = PlayerXP;
    }

    void NextLvl(){
        while(PlayerXP >= XPToNextLvl)
        {
            PlayerXP -= XPToNextLvl;
            XPToNextLvl *= 1.3f;
            XPToNextLvl = Mathf.RoundToInt((float)XPToNextLvl);
            playerLvl ++;
            Debug.Log(playerLvl);
            GetComponent<AudioSource>().PlayOneShot(newLvl);

            Vector3 offset;
            offset = new Vector3(0, 3f);

            GameObject floatingPoint =  Instantiate(newLevelText, transform.position + offset, transform.rotation);
            floatingPoint.GetComponent<TextMeshPro>().text = "New Level!";

            GameObject.Destroy(floatingPoint, 0.7f);

            //GetComponent<SkillsManager>().GainSkillPoints();
        }
    }
}
