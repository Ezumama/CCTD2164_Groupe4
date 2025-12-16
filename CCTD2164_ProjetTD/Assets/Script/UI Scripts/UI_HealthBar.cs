using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private GameObject _generator;
    [SerializeField] private Image[] energyImages;

    private Health _health;
    private float _currentHp;
    private float _maxHp;

    private Color _green, _red, _orange;

    void Start()
    {
        _health = _generator.GetComponent<Health>();
        if (_health == null)
        {
            //Debug.LogError("Le GameObject assigné à '_generator' ne possède pas le composant Health !", _generator);
            Destroy(this);
        }

        _maxHp = _health.maxHealth;
        _currentHp = _health.currentHealth;
        _green = new Color(0f, 0.27f, 0.21f);
        _red = new Color(0.85f, 0.01f, 0.14f);
        _orange = new Color(0.85f, 0.46f, 0f);
    }

    void Update()
    {
        if (_health == null)
        {
            //Destroy(gameObject);
            return;
        }

        _currentHp = _health.currentHealth;
        _hpBar.fillAmount = _currentHp / _maxHp;

        if (_currentHp > (_maxHp / 2))
        {
            _hpBar.color = _green;
        }
        else if (_currentHp < (_maxHp / 2) && _currentHp > (_maxHp / 4))
        {
            _hpBar.color = _orange;
        }
        else if (_currentHp < (_maxHp / 4))
        {
            _hpBar.color = _red;
        }
    }

    public void UpdateEnergyDisplay()
    {
        for (int i = 0; i < energyImages.Length; i++)
        {
            // Show image if index is less than current energy
            energyImages[i].enabled = (i < GameManager.Instance.CurrentEnergyAmount);
        }
    }
}
