using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameBoss : MonoBehaviour
{
    #region Config
    [SerializeField]
    private int CorrectPoints = 2;

    [SerializeField]
    private int WrongPoints = 1;

    [SerializeField]
    private int ForbiddenPoints = -1;

    [SerializeField]
    private int MissedBaggage = -5;

    [SerializeField]
    private Material[] airportsShort;

    [SerializeField]
    private int stamina = 10;

    [SerializeField]
    private Material road;
    [SerializeField]
    private float multiply = 1;
    private float OffSet = 0;
    [SerializeField]
    private Material road2;
    [SerializeField]
    private Destination[] cars;
    [SerializeField]
    private Transform movingPlatform;
    [SerializeField]
    private GameObject effect;
    [SerializeField]
    private GameObject light;
    public delegate void recalculatePoints();
    [SerializeField]
    private Animation shlakabaka;
    public int wallets = 25;
    public recalculatePoints rec;
    #endregion

    #region session
    public float timeLeft = 0;
    private float bTimer = 0;
    private float timeMulti = 1;
    private float speedMulti = 1;
    public int PointsNeed = 100;
    public float waitCarDelay = 6;
    private DayClass cDay; 
    private int currentPoints = 0;
    [SerializeField]private Image hiddenObject;
    private Transform HiddenObject;
    public int CurrentPoints { get { return currentPoints; } }
    private int totalPoints = 0;
    public int TotalPoints { get { return currentPoints; } }
    private bool playing = false;
    public bool roadFree = false;
    private List<GameObject> baggageList = new List<GameObject>();
    private Vector3 Position = Vector3.zero;
    private Vector3 PositionH = Vector3.zero;
    private Dictionary<int, Baggage> moving = new Dictionary<int, Baggage>();
    private int scoreAtOnce = 0;
    public int restricted = 0;
    public int scoreinRaw = 0;
    public int carsSend = 0;
    #endregion

    #region General
    [SerializeField] GameObject[] baggages;
    [SerializeField] Sprite[] GeneralItems;
    [SerializeField] Sprite[] forbiddenItems;
    #endregion 
    public bool Play(DayClass cD)
    {
        HiddenObject = hiddenObject.transform.parent;
        cDay = cD;
        PointsNeed = cDay.Points;
        timeLeft = cDay.levelTime + MainMenu.Instance().staminaLevel * stamina;
        // turn 0ff unneccassary cars:
        Reset();
        playing = true;
        return true;
    }
    private void Reset() {
        carsSend = 0;
        restricted = 0;
        if (cars == null || movingPlatform == null) return;
        airportsShort = new Material[cDay.cars];
        for (int _92 = 0; _92 < cars.Length; _92++)
        {
            cars[_92].sent = 0;
            cars[_92].Clear();
            if (_92 < cDay.cars)
                airportsShort[_92] = cars[_92].Activate();
            else
                cars[_92].Deactivate();
        }
        foreach (GameObject go in baggageList)
        {
            if (go != null)
                Destroy(go);
        }
        baggageList = new List<GameObject>();
        if (Position == Vector3.zero)
        {
            Position = movingPlatform.position;
            PositionH = HiddenObject.position;
        }
        moving = new Dictionary<int, Baggage>();
        if (rec != null) rec();
        currentPoints = 0;
    
    
    }
    public void CarArrived(Destination dest) {
        roadFree = true;
        dest.state++;
        shlakabaka.CrossFade("Close", 0.1f);
        if (dest.state == carState.wait) {
            dest.time = waitCarDelay/(MainMenu.Instance().trucklevel+1);
        }
    }
    public void Update() {
        if (!playing)
            return;
        MoveObjects();
        AddBaggage();
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            if (gameFinished != null)
                gameFinished();
        }
        for (int i = 0; i < cDay.cars; i++) { 
            Destination Car = cars[i];
            if (Car.state == carState.stand)
            {
                if (roadFree) {
                    if (Car.full)
                    {
                        Car.full = false;
                        roadFree = false;
                        Car.sent++;
                        if (carsSend < Car.sent)
                            carsSend = Car.sent;
                        Car.state++;
                        Car.anim.Play("car_move");
                        shlakabaka.CrossFade("Open", 0.1f);
                        Car.time = waitCarDelay;
                    }
                }
            }
            else if (Car.state == carState.wait)
            {
                Car.time -= Time.deltaTime;
                if (Car.time < 0 && roadFree) {
                    roadFree = false;
                    Car.Clear();
                    Car.state = 0;
                    Car.anim.Play("back");
                    shlakabaka.CrossFade("Open", 0.1f);
                }
            }
        
        }

    
    }
    private void AddBaggage() {
        bTimer -= Time.deltaTime;
        if (bTimer < 0) {
            bTimer = cDay.freequency;
            Transform Bag = Instantiate(baggages[Random.Range(0,baggages.Length)]).transform;
            Baggage bg = Bag.GetComponentInChildren<Baggage>();
            // all setups:
            Bag.parent = movingPlatform;
            Bag.localScale = Vector3.one;
            Bag.position = Position;
            bg.transform.parent = movingPlatform;
            Destroy(Bag.gameObject);
            if (Random.Range(0.0f,1)>cDay.forbChance){
                 bg.forbidden = false;
                 hiddenObject.sprite = GeneralItems[Random.Range(0, GeneralItems.Length)];
            }else{
                bg.forbidden = true;
                hiddenObject.sprite = forbiddenItems[Random.Range(0, forbiddenItems.Length)];
            }
            hiddenObject.SetNativeSize();
            bg.hidden = Instantiate(hiddenObject.transform);
            bg.hidden.SetParent(HiddenObject);
            bg.hidden.position = PositionH;
            bg.hidden.localScale = Vector3.one;
            int dd = Random.Range(0, cDay.cars);
            bg.txt.material = airportsShort[dd];
            bg.dest = cars[dd];
            baggageList.Add(bg.gameObject);
        }
    
    }
    public void BaggageArrived(Baggage _bg) {
        // if player loose baggage
        if (_bg.currentDes == null)
        {
            if (_bg.forbidden)
            {
                currentPoints += CorrectPoints;
                scoreAtOnce += CorrectPoints;
            }
            else
            {
                scoreAtOnce = 0;
                currentPoints += MissedBaggage;
            }
            if (effect != null)
            {
                Renderer _ren = _bg.GetComponent<Renderer>();
                if (_ren != null && _ren.isVisible)
                    Destroy(Instantiate(effect, _bg.transform.position, effect.transform.rotation), 1.1f);
            }
            Destroy(_bg.gameObject);
        }
        else
        {
            if (_bg.currentDes == _bg.dest)
            {
                if (_bg.forbidden)
                {
                    currentPoints += ForbiddenPoints;
                    scoreAtOnce = 0;
                }
                else
                {
                    currentPoints += CorrectPoints;
                    scoreAtOnce += CorrectPoints;
                }
            }
            else if (_bg.forbidden)
            {
                if (_bg.currentDes.receiveForbidden)
                {
                    currentPoints += 2 * CorrectPoints;
                    scoreAtOnce += 2 * CorrectPoints;
                    restricted++;
                    if (effect != null)
                    {
                        Renderer _ren = _bg.GetComponent<Renderer>();
                        if (_ren != null && _ren.isVisible)
                            Destroy(Instantiate(effect, _bg.transform.position, effect.transform.rotation), 1.1f);
                    }
                    Destroy(_bg.gameObject);
                }
                else
                {
                    currentPoints += ForbiddenPoints + WrongPoints;
                    scoreAtOnce = 0;
                }
            }
            else
            {
                scoreAtOnce = 0;
                currentPoints += WrongPoints;
            }
            _bg.transform.parent = _bg.currentDes.transform.GetChild(0);
            _bg.currentDes.baggages.Add(_bg.gameObject);
            if (_bg.currentDes.baggages.Count >= cDay.bagsinCar)
                _bg.currentDes.full = true;
            Destroy(_bg);

        }
        if (scoreAtOnce > scoreinRaw)
            scoreinRaw = scoreAtOnce;
        if (currentPoints >= (cDay.Points + wallets * MainMenu.Instance().walletLevel))
        {
            if (gameFinished != null)
                gameFinished();
        }
        if (rec!= null)
        rec();
    }
    // move baggages along the string and inputs block
    private void MoveObjects() {
        movingPlatform.Translate(cDay.moveSpeed*Vector3.right * Time.deltaTime);
        HiddenObject.Translate(cDay.moveSpeed * Vector3.right * Time.deltaTime);
        OffSet += multiply*cDay.moveSpeed * Time.deltaTime;
        road.mainTextureOffset = new Vector2(OffSet,0);
        road2.mainTextureOffset = new Vector2(Mathf.FloorToInt(OffSet / 0.5f)*0.5f,0);
        //touches
        Touch[] touches = Input.touches;
#if UNITY_EDITOR
        if (Input.GetMouseButton(0)){
                if (Input.GetMouseButtonDown(0)){
                     RaycastHit ray;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out ray, 1000))
                    {
                        Baggage bag = ray.collider.GetComponent<Baggage>();
                        if (bag != null) {
                            moving[012] = bag;
                            bag.grab();
                        }
                    }
                }else{
                    if (moving.ContainsKey(012)){
                        RaycastHit ray;
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out ray, 1000))
                        {
                            if (moving[012] == null)
                            {
                                moving.Remove(012);
                                return;
                            }
                            moving[012].Move(ray.point);
                        }
                    }
                }

        } else if (Input.GetMouseButtonUp(0))
        {
            RaycastHit ray;
            if (moving.ContainsKey(012))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out ray, 1000))
                {
                    moving[012].Move(ray.point);
                }
                moving[012].Drop();
                moving.Remove(012);
            }
        }
#else
        foreach (Touch tt in touches){
            if (tt.phase == TouchPhase.Canceled || tt.phase == TouchPhase.Ended)
            {
                if (moving.ContainsKey(tt.fingerId))
                {
                    if  (moving[tt.fingerId] == null){
                        moving.Remove(tt.fingerId);
                        continue;
                    }
                        
                    RaycastHit ray;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(tt.position), out ray, 1000))
                    {
                        moving[tt.fingerId].Move(ray.point);
                    }
                    moving[tt.fingerId].Drop();
                    moving.Remove(tt.fingerId);
                }

            }
            else if (tt.phase == TouchPhase.Began){
                RaycastHit ray;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(tt.position), out ray, 1000))
                {
                    Baggage bag = ray.collider.GetComponent<Baggage>();
                    if (bag != null) {
                        moving[tt.fingerId] = bag;
                        bag.grab();
                    }
                }
            } else if (tt.phase == TouchPhase.Moved)
            {
                if (moving.ContainsKey(tt.fingerId)){
                    RaycastHit ray;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(tt.position), out ray, 1000))
                    {
                        moving[tt.fingerId].Move(ray.point);
                    }
                }
            }
        
        }

#endif
    }
    public delegate void gameEnd();
    public gameEnd gameFinished;
}