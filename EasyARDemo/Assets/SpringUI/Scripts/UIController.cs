using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEditor;

public class UIController : MonoBehaviour,IDragHandler,IPointerDownHandler
{
    public static UIController _instance;
    public float waitTime=0.2f;
    public Camera mainCamera;
    public Sprite whiteCircle;
    private Image mouse;
    private Texture2D m_texture = null;
    public int xCount = 5, yCount = 5;
    private Image pickColorImage;
    private Image orgCarColor;
    public  bool isPick = false;
    private Image btnSetColorImage;
    private Button btnColorPick;
    private Button btnSetColor;
    private Button btnTakePicture;
    private GameObject scanGo;
    private GameObject pictureGo;
    private GameObject topPanelGo;
    private RawImage pictureImage;
    void Start()
    {
        _instance = this;
        scanGo = transform.Find("BottomPanel/Scan").gameObject;
        mouse = transform.Find("BottomPanel/mouse").GetComponent<Image>();
        orgCarColor = GameObject.Find("ColorSetList/OrgColorItem/Color").GetComponent<Image>();
        btnTakePicture = GameObject.Find("BtnTakePicture").GetComponent<Button>();
        pictureGo = GameObject.Find("Picture").gameObject;
        topPanelGo = GameObject.Find("TopPanel").gameObject;
        pictureGo.SetActive(false);
        pictureImage = pictureGo.GetComponent<RawImage>();
        pickColorImage = transform.Find("BottomPanel/BtnPick").GetComponent<Image>();
        btnSetColorImage = transform.Find("BottomPanel/BtnSetColor").GetComponent<Image>();
        btnColorPick = transform.Find("BottomPanel/BtnPick").GetComponent<Button>();
        btnSetColor = transform.Find("BottomPanel/BtnSetColor").GetComponent<Button>();
        btnColorPick.onClick.AddListener(OnBtnColorPickClicked);
        btnTakePicture.onClick.AddListener(OnBtnPictureClicked);
    }


    void OnBtnPictureClicked()
    {
        pictureGo.SetActive(true);
        StartCoroutine(TakePicture());
       
    }
    public void BackClicked()
    {
        pictureGo.SetActive(false);
        OnePointColorEffect._instance.webTexture.Play();
    }
    IEnumerator TakePicture()
    {
        btnColorPick.gameObject.SetActive(false);
        topPanelGo.SetActive(false);
        Application.CaptureScreenshot(Application.dataPath+ "/Resources/Pic.jpg");
        yield return new WaitForSeconds(waitTime);
        AssetDatabase.Refresh();
        GameObject picture= Resources.Load("/Pic") as GameObject;
        Debug.Log(picture.name);
        pictureImage.texture = OnePointColorEffect._instance.cameraTexture.texture;
        for (int i = 0; i <= 7; i++)
        {
            pictureImage.GetComponent<RectTransform>().localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(waitTime);
        }
        yield return new WaitForEndOfFrame();
        OnePointColorEffect._instance.webTexture.Stop();
    }
    void OnBtnColorPickClicked()
    {
        isPick = true;
        btnColorPick.gameObject.SetActive(false);
        btnSetColor.transform.gameObject.SetActive(true);
        scanGo.gameObject.SetActive(true);
        mouse.gameObject.SetActive(true);
    }

    IEnumerator ScreenShot()
    {
        m_texture = new Texture2D(xCount, yCount, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();
        m_texture.ReadPixels(new Rect((int)Input.mousePosition.x - (int)(xCount / 2),
            (int)Input.mousePosition.y - (int)(yCount / 2), xCount, yCount), 0, 0);
        m_texture.Apply();
        pickColorImage.sprite = Sprite.Create(m_texture, new Rect(0, 0, xCount, yCount), Vector2.zero);
        orgCarColor.color = pickColorImage.sprite.texture.GetPixel(1,1);
        btnSetColorImage.sprite = whiteCircle;
        btnSetColorImage.color = pickColorImage.sprite.texture.GetPixel(1,1);
    }
    public void OnDrag(PointerEventData eventData) //鼠标点击拖动图片
    {
        Vector3 gloalMousePos;
        if (isPick && RectTransformUtility.ScreenPointToWorldPointInRectangle(this.transform as RectTransform, 
            eventData.position, eventData.pressEventCamera, out gloalMousePos))
        {
            mouse.transform.position = gloalMousePos;
            StartCoroutine(ScreenShot());
        }
    }

    public void OnPointerDown(PointerEventData eventData) //鼠标点击处设置图片的位置
    {
        Vector3 gloalMousePos;
        if (isPick && RectTransformUtility.ScreenPointToWorldPointInRectangle(this.transform as RectTransform, 
            eventData.position, eventData.pressEventCamera, out gloalMousePos))
        {
            mouse.transform.position = gloalMousePos;
            StartCoroutine(ScreenShot());
        }
    }
}
