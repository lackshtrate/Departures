using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    #region serialized
    [SerializeField]
    private TextWithBorder days;
    [SerializeField]
    private TextWithBorder Score;
    [SerializeField]
    private TextWithBorder HighScore;
    [SerializeField]
    private TextWithBorder scoreReq;
    [SerializeField]
    private TextWithBorder timeNext;
    [SerializeField]
    private GameBoss GB;
    [SerializeField]
    private GameObject play;
    [SerializeField]
    private GameObject IC;
    #endregion
    [SerializeField]
    private DayClass[] daysConf;
    [SerializeField]
    private Image[] qualityB;
    [SerializeField]
    private GameObject[] volumes;
    // Use this for initialization
    public void Start() {
        MainMenu.Instance().GB = GB;
        GB.gameFinished += levelCheck;
        SetOptions(MainMenu.Instance().QualityLev);
        Activate();
        Volume(MainMenu.Instance().volume);
    
    }
    public void Selectlevel() {
        MainMenu.Instance().ChooseLevel(true);
    }
    public void levelCheck() {
       // check();
    }
    public void SetOptions(int level)
    {
        QualitySettings.SetQualityLevel(level);
        MainMenu.Instance().QualityLev = level;
        for (int i = 0; i < qualityB.Length; i++)
        {
            if (qualityB[i] != null)
                if (i != level)
                    qualityB[i].enabled = false;
                else
                    qualityB[i].enabled = true;
        }
    }
    public void Volume(bool Volume)
    {
        MainMenu.Instance().volume = Volume;
        volumes[0].SetActive(Volume);
        volumes[1].SetActive(!Volume);
    }
    public void Activate () {
        if (play != null)
            play.SetActive(true);
        if (IC != null)
            IC.SetActive(false);
        MainMenu.Instance().GB = GB;
        GB.gameObject.SetActive(false);
        Changed();
	}
    public void Left() {
        MainMenu.Instance().SetLevel(MainMenu.Instance().currentLevel);  
    }
    /*
    public void OnDisable() {
        MainMenu.Instance().rec -= Changed;
    }*/
    public void Play() {
        if (GB != null)
        {
            GB.gameObject.SetActive(true);
            GB.Play(daysConf[MainMenu.Instance().currentDay]);
        }
        if (play != null)
            play.SetActive(false);
        if (IC != null)
            IC.SetActive(true);
    }
	// Update is called once per frame
	void Update () {
	
	}
    private void Changed() {
        MainMenu ins = MainMenu.Instance();
        if (days != null)
            days.text = ins.currentDay.ToString();
        if (Score != null)
            Score.text = ins.currentScore.ToString();
        if (HighScore != null)
            HighScore.text = ins.HighScore.ToString();
        if (timeNext != null)
            timeNext.text = daysConf[ins.currentDay].levelTime.ToString();
        if (scoreReq != null)
            scoreReq.text = daysConf[ins.currentDay].Points.ToString();
    }
}