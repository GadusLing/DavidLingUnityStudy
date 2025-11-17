using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    public static GamePanel panel;
    public Button fireButton;
    public PlayerObject player;
    public Toggle toggleOn;
    public Toggle toggleOff;

    public TMP_Text txtName;
    public Button btnChangeName;
    public Slider SliderSound;
    public Button btnBag;
    public TMP_Dropdown ddChange; // 白天黑夜切换
    public Light light1;
    public LongPress longPress;
    public GameObject imgRoot; // 进度条根对象，控制显隐
    public Image imgBK; // 进度条对象，控制进度

    private bool isDown = false;
    private float holdTime = 0f;
    private int HP = 10;

    public RectTransform imgJoy;
    public EventTrigger et;

    void Awake()
    {
        panel = this;
    }
    void Start()
    {
        fireButton.onClick.AddListener(() => {
            player.Fire();
        });

        btnChangeName.onClick.AddListener(() =>
        {
            ChangeNamePanel.panel.gameObject.SetActive(true);
        });

        toggleOn.onValueChanged.AddListener(TogChanged);
        toggleOff.onValueChanged.AddListener(TogChanged);

        SliderSound.value = MusicData.SoundVolume;
        SliderSound.onValueChanged.AddListener((v) =>
        {
            MusicData.SoundVolume = v;
        });

        btnBag.onClick.AddListener(() =>
        {
            BagPanel.panel.gameObject.SetActive(true);
        });

        ddChange.onValueChanged.AddListener((v) =>
        {
            if (v == 0)
            {
                light1.color = Color.white;
            }
            else if (v == 1)
            {
                light1.color = Color.gray;
            }
        });

        longPress.downEvent += BtnDown;
        longPress.upEvent += BtnUp;

        imgRoot.SetActive(false);

        // 为摇杆添加拖拽事件
        EventTrigger.Entry entryDrag = new EventTrigger.Entry();
        entryDrag.eventID = EventTriggerType.Drag;
        entryDrag.callback.AddListener(JoyDrag);
        et.triggers.Add(entryDrag);

        // 为摇杆添加结束拖拽事件
        EventTrigger.Entry entryEndDrag = new EventTrigger.Entry();
        entryEndDrag.eventID = EventTriggerType.EndDrag;
        entryEndDrag.callback.AddListener(JoyEndDrag);
        et.triggers.Add(entryEndDrag);
    }

    private void BtnDown()
    {
        isDown = true;
        holdTime = 0f;

        imgBK.fillAmount = 0f;
    }

    private void BtnUp()
    {
        isDown = false;
        imgRoot.SetActive(false);
    }

    void Update()
    {
        if (isDown)
        {
            if (holdTime >= 0.2f)
            {
                imgRoot.SetActive(true);
                imgBK.fillAmount += Time.deltaTime / 2f; // 两秒充完
                if (imgBK.fillAmount >= 1f)
                {
                    HP += 10;
                    print("回血10点，当前血量：" + HP);
                    imgBK.fillAmount = 0f;
                    holdTime = 0f; // 重置 holdTime
                }
            }
            else
            {
                holdTime += Time.deltaTime; // 仍然保持 holdTime 的累加
            }
        }
    }

    private void TogChanged(bool b)
    {
        foreach(Toggle item in toggleOn.group.ActiveToggles())
        {
            if(item == toggleOn)
            {
                MusicData.SoundIsOpen = true;
            }
            else if(item == toggleOff)
            {
                MusicData.SoundIsOpen = false;
            }
        }
    }

    private void JoyDrag(BaseEventData data)
    {
        // PointerEventData ped = (PointerEventData)data;
        // imgJoy.position += new Vector3(ped.delta.x, ped.delta.y, 0);
        // imgJoy.anchoredPosition = Vector2.ClampMagnitude(imgJoy.anchoredPosition, 120f);// 用API的写法限制最大距离
        // //imgJoy.anchoredPosition=imgJoy.anchoredPosition.normalized * 120; // 等同于上面一行，用单位向量的写法
        // player.Move(imgJoy.anchoredPosition);

        PointerEventData ped = (PointerEventData)data;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgJoy.parent as RectTransform,
            ped.position,
            ped.enterEventCamera,
            out Vector2 curPos);
            imgJoy.localPosition = curPos;
            if(imgJoy.anchoredPosition.magnitude > 120f) // 用长度的写法限制最大距离
            {
                imgJoy.anchoredPosition = imgJoy.anchoredPosition.normalized * 120f;
            }
            player.Move(imgJoy.anchoredPosition);

        // 两种写法的区别 上面一种用向量的写法 比如鼠标按住向上拖拽 再拉回来 鼠标有这个拉回来的动作 人物也会跟着你鼠标拉回来的动作往后移 
        // 而第二种定点的写法 就会有不同的操作手感 就比如说我鼠标往上拉 拉出界很远再往回拉之后 人物不会跟着这个鼠标 因为它是定点 相当于在摇杆上面定点 而并非一个向量 
        // 所以说它在拉回来的时候 你的定点判断摇杆还是在操作区域的上方 这个时候还是往上走 除非拉回摇杆操作区域里 然后再从摇杆区域往下拉 人物才会往回走
        
    }

    private void JoyEndDrag(BaseEventData data)
    {
        imgJoy.anchoredPosition = Vector2.zero;
        player.Move(Vector2.zero);
    }
}
