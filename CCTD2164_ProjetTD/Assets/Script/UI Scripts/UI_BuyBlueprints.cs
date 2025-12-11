using UnityEngine;

public class UI_BuyBlueprints : MonoBehaviour
{
    [SerializeField] private GameObject _blueprintBuyPanel;

    private void Start()
    {
        _blueprintBuyPanel.SetActive(false);
    }

    public void PhoneClicked()
    {
        if(_blueprintBuyPanel.activeSelf == true)
        {
            _blueprintBuyPanel.SetActive(false);
        }
        else
        {
            _blueprintBuyPanel.SetActive(true);
        }
    }

   public void RedBlueprintClicked()
   {
        if (GameManager.Instance.CurrentMoneyAmount >= GameManager.Instance.RedBluePrintCost)
        {
            GameManager.Instance.GainRedBlueprint(1);
            GameManager.Instance.LoseMoney(GameManager.Instance.RedBluePrintCost);
        }
   }

    public void GreenBlueprintClicked()
    {
        if (GameManager.Instance.CurrentMoneyAmount >= GameManager.Instance.GreenBluePrintCost)
        {
            GameManager.Instance.GainGreenBlueprint(1);
            GameManager.Instance.LoseMoney(GameManager.Instance.GreenBluePrintCost);
        }
    }

    public void YellowBlueprintClicked()
    {
        if(GameManager.Instance.CurrentMoneyAmount >= GameManager.Instance.YellowBluePrintCost)
        {
            GameManager.Instance.GainYellowBlueprint(1);
            GameManager.Instance.LoseMoney(GameManager.Instance.YellowBluePrintCost);
        }
    }
}
