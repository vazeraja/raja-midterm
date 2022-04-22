using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public static class Helper {
    public static GameObject FindInChildren(this GameObject go, string name) {
        return (from x in go.GetComponentsInChildren<Transform>()
                where x.gameObject.name == name
                select x.gameObject).First();
    }

    public static void SetActiveAllChildren<T>(this GameObject go, bool state) where T : UnityEngine.Component {
        go.GetComponentsInChildren<T>().ToList().ForEach(x => x.gameObject.SetActive(state));
        go.SetActive(true);
    }
}
public class UIManager : Singleton<UIManager>, IPointerEnterHandler, IPointerExitHandler {

    private bool menuInput;

    // Main Game HUD
    private TextMeshProUGUI currentHPText;
    private TextMeshProUGUI currentOxygenText;
    private TextMeshProUGUI currentGemsText;
    private Image healthFill;
    private Image oxygenFill;
    [SerializeField] private GameObject tooltip;
    [SerializeField] private GameObject CreeditMenu_GO;
    private SpriteLetterSystem SPL;

    private int upgradeCost = 1;

    #region 
    private void Awake() {
        currentHPText = gameObject.FindInChildren("HUD_CURRENT_HP_TEXT").GetComponent<TextMeshProUGUI>();
        currentOxygenText = gameObject.FindInChildren("HUD_CURRENT_O2_TEXT").GetComponent<TextMeshProUGUI>();
        healthFill = gameObject.FindInChildren("HUD_CURRENT_HEALTH_FILL").GetComponent<Image>();
        oxygenFill = gameObject.FindInChildren("HUD_CURRENT_O2_FILL").GetComponent<Image>();
        currentGemsText = gameObject.FindInChildren("HUD_CURRENT_GEMS_TEXT").GetComponent<TextMeshProUGUI>();
        SPL = tooltip.GetComponentInChildren<SpriteLetterSystem>();
    }
    private void Update() {
        menuInput = PlayerInputHandler.MenuInput;
        EnableMenu();
    }

    public void SetHPHUD() {
        float ratio = Player.Instance.CurrentHP / Player.Instance.MaxHP;
        healthFill.fillAmount = ratio;
        currentHPText.text = Player.Instance.CurrentHP.ToString() + "%";
    }

    public void SetOxgyenHUD() {
        float ratio = Player.Instance.CurrentOxygen / Player.Instance.MaxOxygen;
        oxygenFill.fillAmount = ratio;
        currentOxygenText.text = Player.Instance.CurrentOxygen.ToString() + "%";
    }

    public void SetGemsHUD() {
        currentGemsText.text = Player.Instance.CurrentGems.ToString();
    }

    #endregion

    private void TryPurchase() {
        string text;
        if (Player.Instance.CurrentGems > 0) {
            text = "Purchase sucessful ! However, your debt continues to grow.";
            Player.Instance.CurrentGems -= upgradeCost;
        } else {
            text = "Sorry, <c=(84, 161, 32)>Deeto</c>. I can't give <c=(235,122,52)>credit</c>. Come back when you're a little... <c=(235, 52, 208)><w>mmmmmmmmmmm</w></c> richer !";
        }
        SPL.GenerateSmallText(text);
    }


    #region Onclick Button Functions

    public void OnPayOffDebtClick() {
        TryPurchase();
        Debug.Log("Paying off debt");
    }

    public void OnUpgradeWeaponClick() {
        TryPurchase();
        Debug.Log("weapon");
        LocalSave.Instance.saveData.gunLevel++;
    }

    public void OnBuyOxygenClick() {
        TryPurchase();

        Player.Instance.CurrentOxygen += 30;

        Debug.Log("oxygen");
    }

    public void OnPayOffDebtHover() {
        tooltip.SetActive(true);
        var text = "pay off debt";
        SPL.GenerateSmallText(text);
    }

    public void OnUpgradeWeaponHover() {
        tooltip.SetActive(true);
        var text = "upgrade weapon";
        SPL.GenerateSmallText(text);

    }

    public void OnBuyOxygenHover() {
        tooltip.SetActive(true);
        var text = "buy oxygen";
        SPL.GenerateSmallText(text);
    }
    public void OnPointerEnter(PointerEventData eventData) {

        // tooltip.GetComponent<RectTransform>().position = new Vector3(transform.position.x + ((tooltip.GetComponent<RectTransform>().sizeDelta.x / 2) + 100),
        //     transform.position.y - ((tooltip.GetComponent<RectTransform>().sizeDelta.y / 2) + 100), 0);
        // tooltip.GetComponent<RectTransform>().position = eventData.position;
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltip.SetActive(false);
    }


    #endregion

    public void EnableMenu() {
        if (menuInput) {
            CreeditMenu_GO.SetActive(true);
        } else if (!menuInput) {
            CreeditMenu_GO.SetActive(false);
        }
    }

}
