using UnityEngine;
using System.Collections;

public class finished : MonoBehaviour {
    [SerializeField]
    private Destination dest;
    public void Arrive() {
        MainMenu.Instance().GB.CarArrived(dest);
    }

}
