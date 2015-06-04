using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class IScore : MonoBehaviour {
    [SerializeField] Text txt;
    [SerializeField] Transform bar;
    [SerializeField] Text Days;
    [SerializeField] Text Score;
    [SerializeField] private string refString;
    [SerializeField] private Wallet[] wallets;
    [SerializeField] private GameObject looseSet;
    [SerializeField] private GameObject Savers;
    [SerializeField] private GameObject Strike;
    [SerializeField] private GameObject WinSet;
    [SerializeField] private GameObject HighScore;
    [SerializeField] private GameObject CongScreen;
    [SerializeField] private GameObject Skills;
    [SerializeField] private GameObject Scorer;
    [SerializeField] private bool dayoffs = false;
    [SerializeField] private bool strike = false;
    [SerializeField] private bool debt = false;

	// Use this for initialization
    private void OnEnable() {
        if (Days != null)
            refString = Days.text;
        if (MainMenu.Instance().GB != null)
        {
            MainMenu.Instance().GB.rec += ScoreUpdate;
            MainMenu.Instance().GB.gameFinished += backScreen;
        }
        else
            return;
        int ww = MainMenu.Instance().walletLevel;
        for (int i = 0; i < wallets.Length; i++)
        {
            Wallet w = wallets[i];
            if (i < ww)
            {
                if (w.GO != null)
                    w.GO.SetActive(true);
                if (w.image != null)
                    w.image.localScale = Vector3.zero;
            }
            else {
                if (w.GO != null)
                    w.GO.SetActive(false);
            }
        }
        if (looseSet!= null)looseSet.SetActive(false);
        if (Savers != null) Savers.SetActive(false);
        if (WinSet != null) WinSet.SetActive(false);
        if (HighScore != null) HighScore.SetActive(false);
        if (CongScreen != null) CongScreen.SetActive(false);
        if (Skills != null) Skills.SetActive(false);
        if (Scorer != null) Scorer.SetActive(false);
        if (MainMenu.Instance().strike > 0 && strike)
            Strike.SetActive(true);
        else 
            Strike.SetActive(false);
        ScoreUpdate();
    }
    public void Credit() {
        Savers.SetActive(false);
        if (MainMenu.Instance().debt > 0 && debt)
        {
            MainMenu.Instance().debt--;
            MainMenu.Instance().currentScore += MainMenu.Instance().GB.CurrentPoints;
            MainMenu.Instance().Reload();
        }
    }
    public void Continue() {
        MainMenu.Instance().Reload();
    
    }
    private void backScreen(){
        if (Scorer != null)
        {
            Scorer.SetActive(true);
            TextWithBorder text = Scorer.GetComponentInChildren<TextWithBorder>();
            if (text != null)
                text.text = (MainMenu.Instance().GB.CurrentPoints + MainMenu.Instance().currentScore).ToString();

        }     
        if (MainMenu.Instance().GB.CurrentPoints + MainMenu.Instance().currentScore > MainMenu.Instance().HighScore) {
            if (HighScore != null)
                HighScore.SetActive(true);
            MainMenu.Instance().HighScore = MainMenu.Instance().GB.CurrentPoints + MainMenu.Instance().currentScore;
        }
        if (MainMenu.Instance().GB.CurrentPoints < MainMenu.Instance().GB.PointsNeed)
        {
            if (looseSet != null)
                looseSet.SetActive(true);
            if (MainMenu.Instance().debt > 0 && debt)
                Savers.SetActive(true);
        }
        else {
            if (WinSet != null)
                WinSet.SetActive(true);
            MainMenu.Instance().currentDay++;
            MainMenu.Instance().DaysPlayed++;
            MainMenu.Instance().currentScore += MainMenu.Instance().GB.CurrentPoints;
            if (dayoffs) {
                if (MainMenu.Instance().currentDay % 7 == 5)
                {
                    MainMenu.Instance().currentDay += 2;
                }
            }
        }
        Skills.SetActive(MainMenu.Instance().skiller.check());
        MainMenu.Instance().GB.gameObject.SetActive(false);
    }
    private void Update() { 
        if (txt != null) {
            if (MainMenu.Instance().GB.timeLeft>60){
                txt.text  = string.Format("{0}:{1}",Mathf.FloorToInt(MainMenu.Instance().GB.timeLeft/60),Mathf.FloorToInt(MainMenu.Instance().GB.timeLeft%60));
            }else{
                txt.text  = Mathf.FloorToInt(MainMenu.Instance().GB.timeLeft).ToString();
            }
        }
    }
    public virtual void ScoreUpdate() {

        if (Days != null)
        {
            Days.text = ""+MainMenu.Instance().currentDay;     
        }
        if (bar != null) {
            float points = MainMenu.Instance().GB.CurrentPoints;
            bar.localScale = new Vector3(Mathf.Clamp01(points * 1.0f / MainMenu.Instance().GB.PointsNeed), 1, 1);
            if (points >= MainMenu.Instance().GB.PointsNeed)
            {
                bar.localScale = Vector3.one;
                points -= MainMenu.Instance().GB.PointsNeed;
                int ww = MainMenu.Instance().walletLevel;
                for (int i = 0; i < ww; i++)
                {
                    if (wallets[i].image!= null)
                        wallets[i].image.localScale = new Vector3(Mathf.Clamp01(points/(MainMenu.Instance().GB.wallets)), 1, 1);
                    points -= MainMenu.Instance().GB.wallets;
                }
            }
            else {
                bar.localScale = new Vector3(Mathf.Clamp01(points * 1.0f / MainMenu.Instance().GB.PointsNeed), 1, 1);
            }

        
        }
        if (Score != null)
            Score.text =""+ (MainMenu.Instance().GB.CurrentPoints + MainMenu.Instance().currentScore);
    }
    [System.Serializable]
    public class Wallet{
       public Transform image;
       public GameObject GO; 
    }
}
