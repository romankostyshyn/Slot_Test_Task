using System;
using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;

public class GameFieldContainer : MonoBehaviour
{
    [SerializeField] private GameObject backgroundFortuneWheel;
    [SerializeField] private Button spinButton;
    [SerializeField] private Popup popup;
    
    [SerializeField] public PickerWheel pickerWheel;

    public static GameFieldContainer Instance;

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
        Instantiate(popup, transform);
        backgroundFortuneWheel.SetActive(false);
        spinButton.onClick.AddListener(WheelSpin);
    }

    public void WheelState(bool value)
    {
        backgroundFortuneWheel.SetActive(value);
    }

    private void WheelSpin()
    {
        pickerWheel.Spin();
        pickerWheel.OnSpinEnd(wheelPiece =>
        {
            GameField.Instance.UpdateScore(wheelPiece.Amount);
            Popup.Instance.PopupState(true, wheelPiece.Amount);
        });
    }
}