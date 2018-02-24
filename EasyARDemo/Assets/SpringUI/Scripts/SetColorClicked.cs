using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetColorClicked : MonoBehaviour {

    private Button btnSetColorClicked;
    private RawImage SetColorImage;
    private Image colorImage;
	void Start () {
        btnSetColorClicked = this.GetComponent<Button>();
        SetColorImage = this.GetComponent<RawImage>();
        colorImage = transform.Find("Color").GetComponent<Image>();
        btnSetColorClicked.onClick.AddListener(OnSetColorClicked);
    }
	
	void OnSetColorClicked()
    {
        OnePointColorEffect._instance.SetCamtextureColor(colorImage.color);
        if(this.gameObject.transform.name.Equals("YellowColorItem"))
        {
            OnePointColorEffect._instance.Alpha = 1;
            OnePointColorEffect._instance.Near = 0.045f;
            OnePointColorEffect._instance.AddValue = 0.03f;
            OnePointColorEffect._instance.Speed = 0;
        }
        else if(this.gameObject.transform.name.Equals("BlueColorItem"))
        {
            OnePointColorEffect._instance.Alpha = 1;
            OnePointColorEffect._instance.Near = 0.045f;
            OnePointColorEffect._instance.AddValue = 0.6f;
            OnePointColorEffect._instance.Speed = 0;
            OnePointColorEffect._instance.MyColor = Color.green;
        }
        else if (this.gameObject.transform.name.Equals("OrgColorItem"))
        {
            OnePointColorEffect._instance.Alpha = 0;

        }
        else if (this.gameObject.transform.name.Equals("RedColorItem"))
        {
            OnePointColorEffect._instance.Alpha = 1;
            OnePointColorEffect._instance.AddValue = -0.16f;
            OnePointColorEffect._instance.Near = 0.045f;
            OnePointColorEffect._instance.Speed = 0;
            OnePointColorEffect._instance.MyColor = Color.yellow;
        }
        else if(this.gameObject.transform.name.Equals("BlackColorItem"))
        {
            OnePointColorEffect._instance.Alpha = 0.5f;
            OnePointColorEffect._instance.Near = 0.021f;

            OnePointColorEffect._instance.AddValue = 0.0008f;
        }
        else if (this.gameObject.transform.name.Equals("WhiteColorItem"))
        {
            OnePointColorEffect._instance.Alpha = -0.5f;
            OnePointColorEffect._instance.Near = 0.021f;
            OnePointColorEffect._instance.AddValue = 0.021f;
        }
        else
        {
            OnePointColorEffect._instance.Alpha = 1;
            OnePointColorEffect._instance.AddValue = 0.31f;
        }
    }
}
