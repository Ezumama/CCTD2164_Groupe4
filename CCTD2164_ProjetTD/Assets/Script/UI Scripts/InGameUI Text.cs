using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _goldCountTxt;
    [SerializeField] private TextMeshProUGUI _redBlueprintCountTxt;
    [SerializeField] private TextMeshProUGUI _greenBlueprintCountTxt;
    [SerializeField] private TextMeshProUGUI _yellowBlueprintCountTxt;
    [SerializeField] private TextMeshProUGUI _energyCountTxt;
    [SerializeField] private TextMeshProUGUI _WorkerCountTxt;

    [SerializeField] private TextMeshProUGUI _yellowWorkerPriceTxt;
    [SerializeField] private TextMeshProUGUI _redWorkerPriceTxt;
    [SerializeField] private TextMeshProUGUI _greenWorkerPriceTxt;
    void Update()
    {
        _goldCountTxt.text = GameManager.Instance.CurrentMoneyAmount.ToString();
        _redBlueprintCountTxt.text = GameManager.Instance.CurrentRedBlueprintAmount.ToString();
        _greenBlueprintCountTxt.text = GameManager.Instance.CurrentGreenBlueprintAmount.ToString();
        _yellowBlueprintCountTxt.text = GameManager.Instance.CurrentYellowBlueprintAmount.ToString();
        _redBlueprintCountTxt.text = GameManager.Instance.CurrentRedBlueprintAmount.ToString();
        _greenBlueprintCountTxt.text = GameManager.Instance.CurrentGreenBlueprintAmount.ToString();
        _WorkerCountTxt.text = GameManager.Instance.CurrentWorkerAmount.ToString();

        _redWorkerPriceTxt.text = GameManager.Instance.RedBluePrintCost.ToString();
        _greenWorkerPriceTxt.text = GameManager.Instance.GreenBluePrintCost.ToString();
        _yellowWorkerPriceTxt.text = GameManager.Instance.YellowBluePrintCost.ToString();
        //_energyCountTxt.text = GameManager.Instance.CurrentEnergyAmount.ToString();
    }
}
