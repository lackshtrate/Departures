using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextWithBorder : MonoBehaviour {
    [SerializeField]
    private Text mainText;
    private Text border;
    [SerializeField]
    private bool withBorder;
    [SerializeField]
    private Color bColor = Color.black;
	// Use this for initialization
	private void Start () {
        Setup();
	}
    private void Setup() {
        if (mainText == null)
        {
            mainText = GetComponent<Text>();
            if (mainText == null)
                return;
        }
        if (withBorder)
        {
            if (border == null)
            {
                border = Instantiate(mainText);
                Destroy(border.GetComponent<TextWithBorder>());
                border.transform.SetParent(mainText.transform);
                border.transform.localPosition = Vector3.zero;
                border.transform.localRotation = Quaternion.identity;
                border.transform.localScale = Vector3.one;
                if (border.font != null)
                    border.font = Resources.Load<Font>("Fonts/b_" + border.font.name);
                border.color = bColor;
            }
        }
    
    }
	public string text{
        set{
            Setup();
            if (mainText != null){
                mainText.text = value;
                if (withBorder && border != null)
                    border.text = value;
            }
        }
    }
}
