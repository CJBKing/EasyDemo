using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class OnePointColorEffect : MonoBehaviour
{

    public Camera colorListCamera;
    public Sprite bitMapSprite;
    public Material Material;
    public static OnePointColorEffect _instance;
    public Color TargetColor = Color.white;  //屏幕中的颜色
    public Color MyColor = Color.white;  //想要设置的颜色值
    [Range(0, 0.5f)]
    public float Near = 0.1f;   //颜色相似度
    public float AddValue = 2; //调色的值
    public float Alpha=1;   //判断是黑色（0.5），白色（-0.5），其它色（1），还是原色（0）
    public float Speed = 0;
    public Color colorPick;
    private Image pickColorImage;

    private GameObject colorSetGo;
    private Button btnColorPick;
    private Button btnSetColor;
    private Button btnBack;
    private Image btnSetColorImage;

    public  WebCamTexture webTexture;
    public RawImage cameraTexture;
    //private GameObject cameraTexture;

    private Image orgCarColor;
    //private Button[] btnSetColors;
    private GameObject scanGo;
    private Image mouse;
    void Start()
    {
        _instance = this;
        colorSetGo = GameObject.Find("ColorSetList").gameObject;
        btnColorPick = transform.parent.Find("BottomPanel/BtnPick").GetComponent<Button>();
        btnBack = GameObject.Find("TopPanel/BtnBack").GetComponent<Button>();
        pickColorImage = transform.parent.Find("BottomPanel/BtnPick").GetComponent<Image>();
        btnSetColor = transform.parent.Find("BottomPanel/BtnSetColor").GetComponent<Button>();
        btnSetColorImage = transform.parent.Find("BottomPanel/BtnSetColor").GetComponent<Image>();
        cameraTexture = transform.parent.Find("cameraTexture").GetComponent<RawImage>();
        scanGo = transform.parent.Find("BottomPanel/Scan").gameObject;

        orgCarColor = colorSetGo.transform.parent.Find("ColorSetList/OrgColorItem/Color").GetComponent<Image>();
        mouse = transform.parent.Find("BottomPanel/mouse").GetComponent<Image>();
        btnSetColor.onClick.AddListener(OnBtnSetColorClick);
        btnBack.onClick.AddListener(OnBtnBackClicked);
        StartCoroutine(InitCamera());
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        this.Material.SetColor("_TargetColor", this.TargetColor);
        this.Material.SetFloat("_Near", this.Near);
        this.Material.SetColor("_MyColor", this.MyColor);
        this.Material.SetFloat("_AddValue", this.AddValue);
        this.Material.SetFloat("_Alpha", this.Alpha);
        Graphics.Blit(source, destination, this.Material);

    }
    public void SetCamtextureColor(Color setColor)
    {
        MyColor = setColor;
    }
    public IEnumerator InitCamera()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            webTexture = new WebCamTexture(devices[0].name);
            cameraTexture.texture = webTexture;
            webTexture.Play();

        }
    }

    void OnBtnSetColorClick()
    {
        Alpha = 0;
        colorListCamera.gameObject.SetActive(true);
        colorListCamera.depth = 2;
        UIController._instance.isPick = false;
        btnSetColor.transform.gameObject.SetActive(false);
        btnSetColor.transform.parent.gameObject.SetActive(false);
        colorSetGo.SetActive(true);
        mouse.gameObject.SetActive(false);
        TargetColor = orgCarColor.color;
    }
    void OnBtnBackClicked()
    {
        Alpha = 0;
        UIController._instance.isPick = false;
        UIController._instance.BackClicked();
        btnSetColor.transform.gameObject.SetActive(false);
        mouse.gameObject.SetActive(false);
        pickColorImage.gameObject.SetActive(true);
        pickColorImage.sprite = bitMapSprite;
        pickColorImage.transform.parent.gameObject.SetActive(true);
        colorSetGo.SetActive(false);
        btnColorPick.transform.parent.gameObject.SetActive(true);
        scanGo.SetActive(false);

    }



}
