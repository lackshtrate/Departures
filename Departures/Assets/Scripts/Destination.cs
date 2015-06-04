using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Destination : MonoBehaviour {
    [SerializeField] Material txt;
    public bool receiveForbidden = false;
    public List<GameObject> baggages = new List<GameObject>();
    public bool full = false;
    public int sent = 0;
    [SerializeField]
    GameObject mainGO;
    [SerializeField]public Animation anim;
    public float time = -1;
    public carState state = carState.wait;
    public float animState = 0;
	// Use this for initialization
	public Material Activate () {
        if (mainGO!= null)mainGO.SetActive(true);
        return txt;
	}
    public void Deactivate()
    {
        if (mainGO != null) mainGO.SetActive(false);
    }
    public void OnEnable()
    {
        if (anim != null) {
            if (state == carState.back)
            {
                anim.CrossFade("car_move");
                anim["car_move"].time = animState;
            }else if (state == carState.going)
                {
                    anim.CrossFade("back");
                    anim["back"].time = animState;
                }
        } 
    }
     public void OnDisable()
    {
        if (anim != null) {
            if (state == carState.back)
                animState  = anim["car_move"].time;
            if (state == carState.going)
                animState = anim["back"].time;
        } 
    }
	// Update is called once per frame
    public void OnTriggerEnter(Collider col){
        Baggage bag = col.GetComponent<Baggage>();
        if (bag != null)
            bag.currentDes = this;   
    }
    public void OnTriggerExit(Collider col)
    {
        Baggage bag = col.GetComponent<Baggage>();
        if (bag != null && bag.currentDes == this)
            bag.currentDes = null;
    }
    public void Clear() { 
        foreach (GameObject inThe in baggages)
           if (inThe != null)
                Destroy(inThe);
        baggages = new List<GameObject>();
        full = false;
    }
}
public enum carState
{
    going = 0,
    stand = 1,
    back = 2,
    wait = 3
}