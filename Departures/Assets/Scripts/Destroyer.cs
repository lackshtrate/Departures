using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	// Use this for initialization
    public void OnTriggerExit(Collider col) {
        Baggage bag = col.GetComponent<Baggage>();
        if (bag != null)
        {
            MainMenu.Instance().GB.BaggageArrived(bag);
        }
        else
            Destroy(col.gameObject);
    }
}
