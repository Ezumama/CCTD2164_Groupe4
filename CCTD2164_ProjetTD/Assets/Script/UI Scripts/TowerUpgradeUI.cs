using UnityEngine;

public class TowerUpgradeUI : MonoBehaviour
{
    private TowerSpawner _towerUpgradeScript;
    [SerializeField] private GameObject _downgradeButton;
    [SerializeField] private GameObject _upgradeButton;

    public void SetUpgrade(TowerSpawner _upgradeScript)
    {
        Debug.Log("SetUpgrade CALLED on: " + gameObject.name);
        _towerUpgradeScript = _upgradeScript;

        InitializeUI();
    }
    private void InitializeUI()
    {
        if (_towerUpgradeScript._levelUpgrade == 1)
        {
            _downgradeButton.SetActive(false);
        }
        else
        {
            _downgradeButton.SetActive(true);
        }

        if (_towerUpgradeScript._levelUpgrade == 3)
        {
            _upgradeButton.SetActive(false);
        }
        else
        {
            _upgradeButton.SetActive(true);
        }
    }

    public void ReInitialize()
    {
        if (_towerUpgradeScript != null)
        {
            InitializeUI();
        }
    }

    public void UpgradeLvl2Clicked()
    {
        _towerUpgradeScript.UpgradeTowerLevel2();
        gameObject.SetActive(false);
    }

    public void UpgradeLvl3Clicked()
    {
        _towerUpgradeScript.UpgradeTowerLevel3();
        gameObject.SetActive(false);
    }


    // Downgrade level 3 to level 2
    public void DowngradeFromLvl3()
    {
        _towerUpgradeScript.DowngradeToLevel2();
    }

    // Downgrade level 2 to level 1
    public void DowngradeFromLvl2()
    {
        _towerUpgradeScript.DowngradeToLevel1();
    }

}
