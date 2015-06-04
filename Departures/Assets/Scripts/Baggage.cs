using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Baggage : MonoBehaviour{
    [SerializeField] Color[] Colors = new Color[2]{Color.red,Color.black};
    public Transform hidden;
    public Renderer txt;
    public Renderer mat;
    public Destination dest;
    public bool forbidden = false;
    public Destination currentDes;
    private Rigidbody rigid;
    private float multiply = 3;
    private float maxSpeed = 18;
    private bool calm = false;
    public void Awake() {
        mat.material = new Material(mat.material);
        mat.material.color = Colors[Random.Range(0, Colors.Length)];
        mat.transform.Rotate(0, 0, Random.Range(-15, 15));
        rigid = GetComponent<Rigidbody>();
    }
    private void Update() {
        if (calm && rigid.IsSleeping()) {
            Destroy(rigid);
            MainMenu.Instance().GB.BaggageArrived(this);
        }
    }
    public void grab() {
        transform.localScale = 0.8f*Vector3.one;
        if (hidden != null) Destroy(hidden.gameObject);
        transform.parent = transform.root;
        gameObject.layer = 2;
        rigid.useGravity = false;
    }
    public void OnDestroy() {
        if (hidden != null) Destroy(hidden.gameObject);
    
    }
    public void Move(Vector3 position) {
        position  = position - transform.position+0.4f*Vector3.up*transform.lossyScale.y;
        position *= multiply;
        if (position.magnitude > maxSpeed)
            position = position.normalized * maxSpeed; 
        rigid.velocity = position;   
    }
    public void Drop() {
        if (rigid!= null)
            rigid.useGravity = true;
        calm = true;
    }
 

   
}