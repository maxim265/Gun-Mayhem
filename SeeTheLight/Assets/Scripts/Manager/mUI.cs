using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class mUI : MonoBehaviour
{
    static mUI inst;
    public static mUI Inst => inst;

    public RectTransform screenUI;
    public RectTransform worldUI;
    public GameObject interactUI;
    TextMeshProUGUI interactText;
    public RectTransform leftUp;
    RectTransform hp;
    TextMeshProUGUI textPlayerHp;
    Image imgPlayerHp;
    RectTransform def;
    TextMeshProUGUI textPlayerDef;
    Image imgPlayerDef;

    private void Awake()
    {
        inst = this;

        hp = leftUp.Find("HP") as RectTransform;
        textPlayerHp = hp.Find("hpText").GetComponent<TextMeshProUGUI>();
        imgPlayerHp = hp.Find("nowHP").GetComponent<Image>();

        def = leftUp.Find("DEF") as RectTransform;
        textPlayerDef = def.Find("defText").GetComponent<TextMeshProUGUI>();
        imgPlayerDef = def.Find("nowDEF").GetComponent<Image>();

        interactText = interactUI.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void InteractTip(Vector3 pos, string str, Color color = new Color())
    {
        if (color == new Color(0, 0, 0, 0))
            color = Color.white;
        interactText.color = color;
        interactText.text = str;
        Vector3 point = pos + Vector3.up;
        interactUI.GetComponent<RectTransform>().position = point;
        interactUI.SetActive(true);
    }
    public void ColseInteractTip()
    {
        interactUI.SetActive(false);
    }
    public void PlayerUpdate(int maxHp, int nowHp, int maxDef, int nowDef)
    {
        PlayerUpdateHp(maxHp, nowHp);
        PlayerUpdateDef(maxDef, nowDef);
    }
    public void PlayerUpdateHp(int maxHp, int nowHp)
    {
        textPlayerHp.text = nowHp + "/" + maxHp;
        imgPlayerHp.rectTransform.localScale = new Vector3(nowHp / maxHp, 1, 1);
    }
    public void PlayerUpdateDef(int maxDef, int nowDef)
    {
        textPlayerDef.text = maxDef + "/" + nowDef;
        imgPlayerDef.rectTransform.localScale = new Vector3(nowDef / maxDef, 1, 1);
    }
}
