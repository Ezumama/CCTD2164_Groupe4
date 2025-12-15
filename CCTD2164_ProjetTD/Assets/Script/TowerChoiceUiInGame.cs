using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerChoiceUiInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _priceTowerGroundTxt;
    [SerializeField] private TextMeshProUGUI _priceTowerTeslaTxt;
    [SerializeField] private TextMeshProUGUI _priceTowerGatlingTxt;


    private void Update()
    {
        _priceTowerGroundTxt.text = GameManager.Instance.GroundCost.ToString();
        _priceTowerTeslaTxt.text = GameManager.Instance.TeslaCost.ToString();
        _priceTowerGatlingTxt.text = GameManager.Instance.GatlingCost.ToString();
    }
}
