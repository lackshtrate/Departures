using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class skillShower : MonoBehaviour {
    [SerializeField]
    private Sprite[] trucks;
    [SerializeField]
    private Sprite[] ice;
    [SerializeField]
    private Sprite[] wallet;
    [SerializeField]
    private Sprite[] stamina;
    [SerializeField]
    private Sprite[] police;
    [SerializeField]
    private Sprite[] Circle;
    [SerializeField]
    private Image truckIm;
    [SerializeField]
    private Image iceIm;
    [SerializeField]
    private Image walletIm;
    [SerializeField]
    private Image staminaIm;
    [SerializeField]
    private Image policeIm;
    [SerializeField]
    private Image[] highlited;
    [SerializeField]
    private Image center;
    [SerializeField]
    private Color highL;
    [SerializeField]
    private Color normal;
	// Use this for initialization
	private void OnEnable () {
        if (truckIm != null) {
            truckIm.sprite = trucks[Mathf.Min(MainMenu.Instance().trucklevel, trucks.Length - 1)];
            truckIm.SetNativeSize();
        }
        if (walletIm != null)
        {
            walletIm.sprite = wallet[Mathf.Min(MainMenu.Instance().walletLevel, wallet.Length - 1)];
            walletIm.SetNativeSize();
        }
        if (staminaIm != null)
        {
            staminaIm.sprite = stamina[Mathf.Min(MainMenu.Instance().staminaLevel, stamina.Length - 1)];
            staminaIm.SetNativeSize();
        }
        if (policeIm != null)
        {
            policeIm.sprite = police[Mathf.Min(MainMenu.Instance().policeLevel, police.Length - 1)];
            policeIm.SetNativeSize();
        }
        if (iceIm != null)
        {
            iceIm.sprite = ice[Mathf.Min(MainMenu.Instance().icelevel, ice.Length - 1)];
            iceIm.SetNativeSize();
        }
        if (center != null)
        {
            center.sprite = Circle[Mathf.Min(MainMenu.Instance().policeLevel, Circle.Length - 1)];
            center.SetNativeSize();
        }
        if (MainMenu.Instance().walletLevel > MainMenu.Instance().staminaLevel)
        {
            Activate(0);
        }
        else if (MainMenu.Instance().staminaLevel > MainMenu.Instance().icelevel)
        {
            Activate(1);
        }
        else if (MainMenu.Instance().icelevel > MainMenu.Instance().trucklevel)
        {
            Activate(2);
        }
        else if (MainMenu.Instance().trucklevel > MainMenu.Instance().policeLevel)
        {
            Activate(3);
        }
        else {
            Activate(4);
        }
        
	}
    public void Activate(int id) {
        for (int i = 0; i < highlited.Length; i++) {
            if (i == id)
                highlited[i].color = highL;
            else
                highlited[i].color = normal;

        }
    
    }
}
