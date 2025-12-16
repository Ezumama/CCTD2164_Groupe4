using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _goldCountTxt;
    [SerializeField] private TextMeshProUGUI _redBlueprintCountTxt;
    [SerializeField] private TextMeshProUGUI _greenBlueprintCountTxt;
    [SerializeField] private TextMeshProUGUI _yellowBlueprintCountTxt;
    [SerializeField] private TextMeshProUGUI _workerCountTxt;

    [SerializeField] private TextMeshProUGUI _yellowPriceTxt;
    [SerializeField] private TextMeshProUGUI _redPriceTxt;
    [SerializeField] private TextMeshProUGUI _greenPriceTxt;


    void Update()
    {
        _goldCountTxt.text = GameManager.Instance.CurrentMoneyAmount.ToString();
        
        _redBlueprintCountTxt.text = GameManager.Instance.CurrentRedBlueprintAmount.ToString();
        _greenBlueprintCountTxt.text = GameManager.Instance.CurrentGreenBlueprintAmount.ToString();
        _yellowBlueprintCountTxt.text = GameManager.Instance.CurrentYellowBlueprintAmount.ToString();
        
        _redBlueprintCountTxt.text = GameManager.Instance.CurrentRedBlueprintAmount.ToString();
        _greenBlueprintCountTxt.text = GameManager.Instance.CurrentGreenBlueprintAmount.ToString();
        
        _workerCountTxt.text = GameManager.Instance.CurrentWorkerAmount.ToString();

        _redPriceTxt.text = GameManager.Instance.RedBluePrintCost.ToString();
        _greenPriceTxt.text = GameManager.Instance.GreenBluePrintCost.ToString();
        _yellowPriceTxt.text = GameManager.Instance.YellowBluePrintCost.ToString();
        //_energyCountTxt.text = GameManager.Instance.CurrentEnergyAmount.ToString();

    }
}
