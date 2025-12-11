using System;
using System.Collections;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _towers;
    [SerializeField] private GameObject towerChoicePanelPrefab;

    [SerializeField] private GraphicRaycaster _uiRaycaster;
    [SerializeField] private EventSystem _eventSystem;

    #region int costs
    private int _gatlingCost;
    private int _teslaCost;
    private int _groundCost;

    private int _gatlingEnergyCost;
    private int _teslaEnergyCost;
    private int _groundEnergyCost;
    #endregion

    #region tower cost UI texts
    [Header("Tower Cost UI Text")]
    [SerializeField] private TextMeshProUGUI _gatlingCostText;
    [SerializeField] private TextMeshProUGUI _teslaCostText;
    [SerializeField] private TextMeshProUGUI _groundCostText;
    [SerializeField] private TextMeshProUGUI _gatlingEnergyCostText;
    [SerializeField] private TextMeshProUGUI _teslaEnergyCostText;
    [SerializeField] private TextMeshProUGUI _groundEnergyCostText;
    #endregion

    #region tower animation
    [Header("Tower Animation")]
    [SerializeField] private Animator _towerAnimator;
    [SerializeField] private float _openAnimationDuration = 1.0f;
    [SerializeField] private Transform _spawnStartPosition;
    [SerializeField] private Transform _spawnEndPosition;
    [SerializeField] private float _spawnPopDuration = 0.5f;
    #endregion

    #region prefabs
    private GameObject _towerLevel1;
    private GameObject _towerLevel2;
    private GameObject _towerLevel3;
    [SerializeField] private GameObject _towerUpgradeDowngradePanelLvl1_Prefab;
    [SerializeField] private GameObject _towerUpgradeDowngradePanelLvl2_Prefab;
    [SerializeField] private GameObject _towerUpgradeDowngradePanelLvl3_Prefab;
    #endregion

    #region boolean to know which tower it is and it's upgrade level
    [Header("Boolean to know which tower it is")]
    private bool _isTripleMelTower;
    private bool _isBigBettyTower;
    private bool _isSimpleLizaTower;
    public int _levelUpgrade;
    #endregion

    #region upgrade cost
    [Header("Upgrade Cost")]
    [SerializeField] private int _blueprintCostLvl2;
    [SerializeField] private int _blueprintCostLvl3;
    #endregion

    #region private variables
    [Header("Private Variables")]
    private GameObject _towerPanelLvl1;
    private GameObject _towerPanelLvl2;
    private GameObject _towerPanelLvl3;
    private TowerUpgradeUI _towerUIScriptLvl1;
    private TowerUpgradeUI _towerUIScriptLvl2;
    private TowerUpgradeUI _towerUIScriptLvl3;
    private Camera _camera;
    [SerializeField] private GameObject _currentTower;
    private TowerChoiceUI _choiceUIScript;
    private GameObject _towerChoicePanel;
    [SerializeField] private bool _isBuilding = false;
    #endregion

    private void Start()
    {
        // Get TowerChoiceUI script from UI Panel and assign Spawner to this tower spawner
        _towerChoicePanel = Instantiate(towerChoicePanelPrefab, transform);
        _towerChoicePanel.SetActive(false);

        _choiceUIScript = _towerChoicePanel.GetComponentInChildren<TowerChoiceUI>();
        _choiceUIScript.SetSpawner(this);
        _camera = Camera.main;

        // Set cost
        _gatlingCost = GameManager.Instance.GatlingCost;
        _teslaCost = GameManager.Instance.TeslaCost;
        _groundCost = GameManager.Instance.GroundCost;
        _gatlingEnergyCost = GameManager.Instance.GatlingEnergyCost;
        _teslaEnergyCost = GameManager.Instance.TeslaEnergyCost;
        _groundEnergyCost = GameManager.Instance.GroundEnergyCost;

        // Initialize Level 1 panel (downgrade from level 2)
        _towerPanelLvl1 = Instantiate(_towerUpgradeDowngradePanelLvl1_Prefab, transform);
        _towerPanelLvl1.SetActive(false);
        _towerUIScriptLvl1 = _towerPanelLvl1.GetComponentInChildren<TowerUpgradeUI>();
        _towerUIScriptLvl1.SetUpgrade(this);

        // Initialize Level 2 panel (upgrade/downgrade)
        _towerPanelLvl2 = Instantiate(_towerUpgradeDowngradePanelLvl2_Prefab, transform);
        _towerPanelLvl2.SetActive(false);
        _towerUIScriptLvl2 = _towerPanelLvl2.GetComponentInChildren<TowerUpgradeUI>();
        _towerUIScriptLvl2.SetUpgrade(this);

        // Initialize Level 3 panel (downgrade only)
        _towerPanelLvl3 = Instantiate(_towerUpgradeDowngradePanelLvl3_Prefab, transform);
        _towerPanelLvl3.SetActive(false);
        _towerUIScriptLvl3 = _towerPanelLvl3.GetComponentInChildren<TowerUpgradeUI>();
        _towerUIScriptLvl3.SetUpgrade(this);

        _levelUpgrade = 0;
    }

    public void SpawnTower(int index)
    {
        // Spawn specific tower and destroy spawner
        if (!_isBuilding)
        {
            StartCoroutine(BuildSequence(index));
        }
    }

    #region Tower Build Sequence
    private IEnumerator BuildSequence(int index)
    {
        _isBuilding = true;

        // Hide the UI
        _towerChoicePanel.SetActive(false);

        // Play the Opening Animation
        _towerAnimator.SetTrigger("Open");

        // Set The status to open
        _towerAnimator.SetBool("IsOpen", true);

        // Wait for the animation to finish
        yield return new WaitForSeconds(_openAnimationDuration);

        // Spawn specific tower at start position
        Vector3 startPos = _spawnStartPosition != null ? _spawnStartPosition.position : transform.position;
        Vector3 endPos = _spawnEndPosition != null ? _spawnEndPosition.position : transform.position;

        _currentTower = Instantiate(_towers[index], startPos, Quaternion.identity, transform);
        _levelUpgrade = 1;

        // Pop animation: lerp from start to end position
        float elapsedTime = 0f;
        while (elapsedTime < _spawnPopDuration && _currentTower != null)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _spawnPopDuration;
            _currentTower.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        // Ensure final position is exact
        if (_currentTower != null)
        {
            _currentTower.transform.position = endPos;
        }

        _isBuilding = false;

    }
    #endregion

    #region Upgrade Sequence Logic
    private IEnumerator UpgradeSequence(GameObject newTowerPrefab, int newLevelIndex)
    {
        _isBuilding = true;
        CloseAllPanels();

        // Calculate positions
        Vector3 downPos = _spawnStartPosition != null ? _spawnStartPosition.position : transform.position;
        Vector3 upPos = _spawnEndPosition != null ? _spawnEndPosition.position : transform.position;

        //Move old tower down
        float elapsedTime = 0f;
        while (elapsedTime < _spawnPopDuration && _currentTower != null)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _spawnPopDuration;
            _currentTower.transform.position = Vector3.Lerp(upPos, downPos, t);
            yield return null;
        }

        // Destroy old tower
        if (_currentTower != null) Destroy(_currentTower);

        _towerAnimator.Play("Closing");
        yield return new WaitForSeconds(_openAnimationDuration);
        _towerAnimator.Play("Opening");
        yield return new WaitForSeconds(_openAnimationDuration);

        //Move new tower up
        _currentTower = Instantiate(newTowerPrefab, downPos, Quaternion.identity, transform);
        _levelUpgrade = newLevelIndex;

        elapsedTime = 0f;
        while (elapsedTime < _spawnPopDuration && _currentTower != null)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _spawnPopDuration;
            _currentTower.transform.position = Vector3.Lerp(downPos, upPos, t);
            yield return null;
        }

        if (_currentTower != null) _currentTower.transform.position = upPos;

        _isBuilding = false;
    }
    #endregion

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"Mouse clicked! IsBuilding: {_isBuilding}");

            // mouse hovering over a UI element?
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Pointer is over UI - ignoring click");
                return;
            }

            if (_isBuilding == false)
            {
                Debug.Log("Calling OnClick()");
                OnClick();
            }
            else
            {
                Debug.Log("Cannot click - currently building");
            }
        }
    }

    #region tower choice
    public void GatlingChoice()
    {
        if (GameManager.Instance.CurrentMoneyAmount >= _gatlingCost && GameManager.Instance.CurrentEnergyAmount >= _gatlingEnergyCost)
        {
            GameManager.Instance.LoseMoney(_gatlingCost);
            GameManager.Instance.LoseEnergy(_gatlingEnergyCost);
            _isTripleMelTower = true;
            _towerLevel1 = _towers[0];
            _towerLevel2 = _towers[3];
            _towerLevel3 = _towers[4];
            SpawnTower(0);
        }
        else if (GameManager.Instance.CurrentMoneyAmount < _gatlingCost)
        {
            Debug.Log("Not enough money to build Gatling Tower!");
            _gatlingCostText.color = Color.red;
        }
        else if (GameManager.Instance.CurrentEnergyAmount < _gatlingEnergyCost)
        {
            Debug.Log("Not enough energy to build Gatling Tower!");
            _gatlingEnergyCostText.color = Color.red;
        }

    }
    public void TeslaChoice()
    {
        if (GameManager.Instance.CurrentMoneyAmount >= _teslaCost && GameManager.Instance.CurrentEnergyAmount >= _teslaEnergyCost)
        {
            GameManager.Instance.LoseMoney(_teslaCost);
            GameManager.Instance.LoseEnergy(_teslaEnergyCost);
            _isBigBettyTower = true;
            _towerLevel1 = _towers[1];
            _towerLevel2 = _towers[5];
            _towerLevel3 = _towers[6];
            SpawnTower(1);
        }
        else if (GameManager.Instance.CurrentMoneyAmount < _teslaCost)
        {
            Debug.Log("Not enough money to build Tesla Tower!");
            _teslaCostText.color = Color.red;
        }
        else if (GameManager.Instance.CurrentEnergyAmount < _teslaEnergyCost)
        {
            Debug.Log("Not enough energy to build Tesla Tower!");
            _teslaEnergyCostText.color = Color.red;
        }
    }

    public void GroundChoice()
    {
        if (GameManager.Instance.CurrentMoneyAmount >= _groundCost && GameManager.Instance.CurrentEnergyAmount >= _groundEnergyCost)
        {
            GameManager.Instance.LoseMoney(_groundCost);
            GameManager.Instance.LoseEnergy(_groundEnergyCost);
            _isSimpleLizaTower = true;
            _towerLevel1 = _towers[2];
            _towerLevel2 = _towers[7];
            _towerLevel3 = _towers[8];
            SpawnTower(2);
        }
        else if (GameManager.Instance.CurrentMoneyAmount < _groundCost)
        {
            Debug.Log("Not enough money to build Ground Tower!");
            _groundCostText.color = Color.red;
        }
        else if (GameManager.Instance.CurrentEnergyAmount < _groundEnergyCost)
        {
            Debug.Log("Not enough energy to build Ground Tower!");
            _groundEnergyCostText.color = Color.red;
        }
    }
    #endregion

    public void OnClick()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        float maxDistance = 100f;

        // What did we click?   
        if (!Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.Log("Raycast hit nothing - closing panels");
            CloseAllPanels();
            return;
        }

        // If this line executes, the Raycast worked. What did it hit?
        Debug.Log($"Ray Hit! Object: {hit.collider.gameObject.name}, Parent: {(hit.collider.transform.parent != null ? hit.collider.transform.parent.name : "No Parent")}");
        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 5f);

        // Did we click this spawner or its children?
        TowerSpawner spawnerHit = hit.collider.GetComponentInParent<TowerSpawner>();

        if (spawnerHit == null)
        {
            Debug.Log("No TowerSpawner found in hierarchy - closing panels");
            CloseAllPanels();
            return;
        }

        // Clicked something that is NOT this spawner
        if (spawnerHit != this)
        {
            Debug.Log($"Clicked different spawner: {spawnerHit.gameObject.name}");
            CloseAllPanels();
            return;
        }

        // Did we click THIS spawner?
        Debug.Log($"Clicked THIS spawner! Current level: {_levelUpgrade}");
        CloseAllPanels();

        // Show appropriate panel based on tower level
        if (_levelUpgrade == 0)
        {
            Debug.Log("Showing tower choice panel");
            _towerChoicePanel.SetActive(true);
        }
        else if (_levelUpgrade == 1)
        {
            Debug.Log("Showing level 1 panel (upgrade to level 2 only)");
            _towerUIScriptLvl1.ReInitialize();
            _towerPanelLvl1.SetActive(true);
        }
        else if (_levelUpgrade == 2)
        {
            Debug.Log("Showing level 2 panel (upgrade to 3 or downgrade to 1)");
            _towerUIScriptLvl2.ReInitialize();
            _towerPanelLvl2.SetActive(true);
        }
        else if (_levelUpgrade == 3)
        {
            Debug.Log("Showing level 3 panel (downgrade to level 2 only)");
            _towerUIScriptLvl3.ReInitialize();
            _towerPanelLvl3.SetActive(true);
        }
    }

    private void CloseAllPanels()
    {
        _towerChoicePanel.SetActive(false);
        _towerPanelLvl1.SetActive(false);
        _towerPanelLvl2.SetActive(false);
        _towerPanelLvl3.SetActive(false);
    }

    #region Level 2 Upgrade (from Level 1)
    public void UpgradeTowerLevel2()
    {
        // Don't upgrade if already building
        if (_isBuilding) return;

        // Pay the cost
        if (_isTripleMelTower)
            GameManager.Instance.LoseRedBlueprint(_blueprintCostLvl2);
        else if (_isBigBettyTower)
            GameManager.Instance.LoseGreenBlueprint(_blueprintCostLvl2);
        else if (_isSimpleLizaTower)
            GameManager.Instance.LoseYellowBlueprint(_blueprintCostLvl2);

        // Start the animation
        StartCoroutine(UpgradeSequence(_towerLevel2, 2));
    }
    #endregion

    #region Level 3 Upgrade (from Level 2)
    public void UpgradeTowerLevel3()
    {
        // Don't upgrade if already building
        if (_isBuilding) return;

        // Pay the cost
        if (_isTripleMelTower)
            GameManager.Instance.LoseRedBlueprint(_blueprintCostLvl3);
        else if (_isBigBettyTower)
            GameManager.Instance.LoseGreenBlueprint(_blueprintCostLvl3);
        else if (_isSimpleLizaTower)
            GameManager.Instance.LoseYellowBlueprint(_blueprintCostLvl3);

        // Start the animation
        StartCoroutine(UpgradeSequence(_towerLevel3, 3));
    }
    #endregion

    #region Downgrade Level 3 to Level 2
    public void DowngradeToLevel2()
    {
        if (_isBuilding)
        {
            return;
        }

        // Refund the Level 3 upgrade cost
        if (_isTripleMelTower)
        {
            GameManager.Instance.GainRedBlueprint(_blueprintCostLvl3);
        }
        else if (_isBigBettyTower)
        {
            GameManager.Instance.GainGreenBlueprint(_blueprintCostLvl3);
        }
        else if (_isSimpleLizaTower)
        {
            GameManager.Instance.GainYellowBlueprint(_blueprintCostLvl3);
        }

        // Start the downgrade animation
        StartCoroutine(UpgradeSequence(_towerLevel2, 2));
    }
    #endregion

    #region Downgrade Level 2 to Level 1
    public void DowngradeToLevel1()
    {
        if (_isBuilding)
        {
            return;
        }

        // Refund the Level 2 upgrade cost
        if (_isTripleMelTower)
        {
            GameManager.Instance.GainRedBlueprint(_blueprintCostLvl2);
        }
        else if (_isBigBettyTower)
        {
            GameManager.Instance.GainGreenBlueprint(_blueprintCostLvl2);
        }
        else if (_isSimpleLizaTower)
        {
            GameManager.Instance.GainYellowBlueprint(_blueprintCostLvl2);
        }

        // Start the downgrade animation
        StartCoroutine(UpgradeSequence(_towerLevel1, 1));
    }
    #endregion
}