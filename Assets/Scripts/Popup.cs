using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Button okButton;
    [SerializeField] private TMP_Text descriptionText;

    public static Popup Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
        okButton.onClick.AddListener(() => PopupState(false, 0));
    }

    public void PopupState(bool value, int reward)
    {
        descriptionText.SetText(reward > 0 ? $"You win {reward} coins!" : "Maybe you win another time");
        this.gameObject.SetActive(value);
        GameFieldContainer.Instance.WheelState(value);
    }
}