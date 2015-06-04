using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {
#region static block
    public static MainMenu instance;
    public static MainMenu Instance()
    {
        if (instance == null) {
            instance = FindObjectOfType<MainMenu>();
            if (instance == null) {
                instance = new GameObject("miniME").AddComponent<MainMenu>();
            }
        }
        return instance;
    }
    [SerializeField]
    GameObject levelSelection;
    [SerializeField]
    GameObject Loading;
    [SerializeField]
    Transform LoadingBar;
    private bool levelSelect = false;
    public Camera cam;
    public delegate void dataChanged();
    public dataChanged rec;
#endregion

    # region session
    public GameBoss GB;
    public SkillsManager skiller;
#endregion

#region session
    public int currentDay = -1;
    public int currentLevel = -1;
    // total amount of points obtained till this moment
    public int currentScore = 0;
    // total amount of days played
    public int DaysPlayed = 0;
    // total amount of points obtained: 
    public int ScoreRiched = 0;

    public int HighScore = 0;
    /// skill block----------------------------
    public int staminaLevel = 0;
    // 
    public int walletLevel = 0;
    // fire truck
    public int trucklevel = 0;
    // ice
    public int icelevel = 0;
    // police level
    public int policeLevel = 0;
    // stirke level
    public int strike = 0;
    // debt
    public int debt = 0;
    // debt
    public int thickDay = 0;
    // Quality---------------------------
    public int QualityLev = 0;
    public bool volume = false;
    // 
#endregion
#region player settings



#endregion
    private GameObject PlayMenu;
    private GameObject ChooseMenu;
    public bool loading = false;

	// Use this for initialization
	private void Awake () {
        DontDestroyOnLoad(gameObject);
        instance = this;
        GetCamera();
        LoadData();
    }
    private void Start(){
        // load Selection Scene------------------
        if (levelSelection != null){
            levelSelection.SetActive(false);
        }
        // set level ----------------------
         StartCoroutine(LoadLeavel());

	}
    private  void GetCamera(){
        cam = Camera.main;
        if (cam == null)
            cam = FindObjectOfType<Camera>();
        if (cam == null)
        {
            cam = new GameObject("Camera").AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.Color;
            cam.tag = "MainCamera";
            cam.transform.eulerAngles = new Vector3(10, 190, 1.8f);
            cam.orthographic = true;
            cam.orthographicSize = 5;
        }

    }
    public IEnumerator LoadLeavel() {
       float time = 0.5f;
       loading = true;
       if (Loading != null)
       {
           Loading.SetActive(true);
       }
       if (levelSelection != null && levelSelection.activeSelf)
           levelSelection.SetActive(false);
      //  yield return null;
      AsyncOperation async =  Application.LoadLevelAsync(Mathf.Max(1,currentLevel + 1));
      async.allowSceneActivation = true;
      while (async.progress < 0.95f)
      {
          if (LoadingBar != null) {
              LoadingBar.localScale = new Vector3(async.progress*0.9f, 1, 1);
          }
          time -= Time.deltaTime;
          yield return null;
      }
      AsyncOperation async2 = Resources.UnloadUnusedAssets();
      while (!async2.isDone)
      {
          if (LoadingBar != null)
          {
              LoadingBar.localScale = new Vector3(0.9f+async2.progress * 0.1f, 1, 1);
          }
          time -= Time.deltaTime;
          yield return null;
      }
      if (Loading != null)
      {
          if (LoadingBar != null)
          {
              LoadingBar.localScale = Vector3.one;
          }
          while (time > 0) {
              time -= Time.deltaTime;
              yield return null;
          }
          Loading.SetActive(false);
      }
      loading = false;
    }
    public void SetLevel(int id) {
        if (loading)
            return;
        Reset();
        currentLevel = id;
        SaveData();
        StartCoroutine(LoadLeavel());
    }
    public void Reload()
    {
        if (loading)
            return;
        SaveData();
        StartCoroutine(LoadLeavel());
    }
    public void Reset() {
        currentDay = 0;
        currentScore = 0;     
    }
    public void ChooseLevel(bool state)
    {
        levelSelection.SetActive(state);
    }
	// Update is called once per frame
	private void Update () {
	
	}
    private void LoadData() {
        // level setups
        currentDay = PlayerPrefs.GetInt("currentDay", 0);
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        currentScore = PlayerPrefs.GetInt("currentScore", 0);
        DaysPlayed = PlayerPrefs.GetInt("daysPlayed", 0);
        ScoreRiched = PlayerPrefs.GetInt("ScoreRiched", 0);
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        // skills setup
        staminaLevel = PlayerPrefs.GetInt("sLevel", 0);
        walletLevel = PlayerPrefs.GetInt("wLevel", 0);
        trucklevel = PlayerPrefs.GetInt("tLevel", 0);
        icelevel = PlayerPrefs.GetInt("iLevel", 0);
        policeLevel = PlayerPrefs.GetInt("pLevel", 0);
        QualityLev = PlayerPrefs.GetInt("QualityLev", 0);
        volume = (PlayerPrefs.GetInt("volume", 1) == 1)?true:false;
        //
        strike = PlayerPrefs.GetInt("strike", 0);
        debt = PlayerPrefs.GetInt("debt", 0);
        thickDay = PlayerPrefs.GetInt("thickDay", 0);
        QualitySettings.SetQualityLevel(QualityLev);
        if (rec!= null)rec();
    }
    public void SaveData()
    {
        //---------
        PlayerPrefs.SetInt("currentDay", currentDay);
        PlayerPrefs.SetInt("currentLevel", currentLevel);
        PlayerPrefs.SetInt("currentScore", currentScore);
        PlayerPrefs.SetInt("daysPlayed", DaysPlayed);
        PlayerPrefs.SetInt("ScoreRiched", ScoreRiched);
        PlayerPrefs.SetInt("HighScore", HighScore);
        //--------
        PlayerPrefs.SetInt("sLevel", staminaLevel);
        PlayerPrefs.SetInt("wLevel", walletLevel);
        PlayerPrefs.SetInt("tLevel", trucklevel);
        PlayerPrefs.SetInt("iLevel", icelevel);
        PlayerPrefs.SetInt("pLevel", policeLevel);
        PlayerPrefs.SetInt("strike", strike);
        PlayerPrefs.SetInt("debt", debt);
        PlayerPrefs.SetInt("thickDay", thickDay);
        PlayerPrefs.SetInt("QualityLev", QualityLev);
        PlayerPrefs.SetInt("volume", volume?1:0);
        PlayerPrefs.Save();
        if (rec != null) rec();
    }
    private void OnGUI() {
        if (GUILayout.Button("reset")) {
            PlayerPrefs.SetInt("currentDay", 0);
            PlayerPrefs.SetInt("currentLevel", 0);
            PlayerPrefs.SetInt("currentScore", 0);
            PlayerPrefs.SetInt("daysPlayed", 0);
            PlayerPrefs.SetInt("ScoreRiched", 0);
            PlayerPrefs.SetInt("HighScore", 0);
            //--------
            PlayerPrefs.SetInt("sLevel", 0);
            PlayerPrefs.SetInt("wLevel", 0);
            PlayerPrefs.SetInt("tLevel", 0);
            PlayerPrefs.SetInt("iLevel", 0);
            PlayerPrefs.SetInt("pLevel", 0);
            //------------------------------
            PlayerPrefs.SetInt("thickDay", 1);
            PlayerPrefs.SetInt("strike", 1);
            PlayerPrefs.SetInt("debt", 1);
            PlayerPrefs.Save();
            LoadData();
            StartCoroutine(LoadLeavel());
        }
    }
}
