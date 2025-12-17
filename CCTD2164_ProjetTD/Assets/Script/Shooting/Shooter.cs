using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class Shooter : MonoBehaviour
{
    #region Tower Parts
    [Header("Tower Parts")]
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private float _shootingDistance;
    [SerializeField] private GameObject _feedbackFXOut;
    [SerializeField] private GameObject _towerShootingHead;
    [SerializeField] private GameObject _towerBody;
    //// Making sure the tower canon is facing forward for LookAt
    //[Header("Face forward correction")]
    //[SerializeField] private Vector3 _modelForwardOffset = new Vector3(90f, 0f, 0f);
    #endregion

    #region Tower Specs
    [Header("Tower Specs")]
    [SerializeField] private float _damageAmount;
    [SerializeField] private float _shootingCooldown;
    [SerializeField] private bool _isBigBetty;
    #endregion

    #region Aim Smoothing
    [Header("Aim smoothing")]
    [SerializeField] private float _rotationSpeed = 8f; // How fast the head/body rotates to face the target
    [SerializeField] private float _pitchSpeed = 8f; // How fast the cannon pitches up/down
    #endregion

    [Header("Tag(s) for targets")]
    [SerializeField] private string _tag1;
    // Leave empty if GroundTower only
    [SerializeField] private string _tag2;

    private GameObject _target;

    void Start()
    {
        StartCoroutine(DoShooting());
    }

    private void Update()
    {
        FindTarget();

        Debug.DrawRay(_shootingPoint.position, _shootingPoint.forward * 2f, Color.red);
        #region tower orientation
        if (_target == null)
        {
            Debug.Log("NO TARGET FOUND");
        }
        else
        {
            Debug.Log("TARGET FOUND: " + _target.name);
        }


        if (_target != null && _isBigBetty == false)
        {
            // Smoothly rotate the shooting head towards the target
            Vector3 directionToTarget = _target.transform.position - _towerShootingHead.transform.position;
            if (directionToTarget != Vector3.zero)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(directionToTarget);
                _towerShootingHead.transform.rotation = Quaternion.Slerp(_towerShootingHead.transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        else if (_target != null && _isBigBetty == true)
        {
            // Body rotating left and right
            Vector3 directionToTarget = _target.transform.position - _towerBody.transform.position;
            directionToTarget.y = 0;

            if (directionToTarget != Vector3.zero)
            {
                // Smoothly rotate the body left/right towards the target
                Quaternion desiredBodyRotation = Quaternion.LookRotation(directionToTarget);
                _towerBody.transform.rotation = Quaternion.Slerp(_towerBody.transform.rotation, desiredBodyRotation, _rotationSpeed * Time.deltaTime);
            }

            // Cannon looking up and down
            Vector3 localDirection = _towerBody.transform.InverseTransformPoint(_target.transform.position);

            float angleX = Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg;
            angleX = -angleX;
            Quaternion desiredLocalRotation = Quaternion.Euler(angleX, 0f, 0f);
            _towerShootingHead.transform.localRotation = Quaternion.Slerp(_towerShootingHead.transform.localRotation, desiredLocalRotation, _pitchSpeed * Time.deltaTime);  
        }
#endregion

        //Debug.Log("Damage Amount: " + _damageAmount);
    }

    // Find bots with given tag(s)
    private void FindTarget()
    {
        GameObject closestBot = null;
        float closestDistance = Mathf.Infinity;

        // Collect all objects with given tag(s)
        var bots = GameObject.FindGameObjectsWithTag(_tag1);

        if (!string.IsNullOrEmpty(_tag2))
            bots = bots.Concat(GameObject.FindGameObjectsWithTag(_tag2)).ToArray();

        foreach (GameObject bot in bots)
        {
            float distance = Vector3.Distance(transform.position, bot.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBot = bot;
            }
        }

        _target = closestBot;
    }

    #region worker buff
    public void BuffDamage()
    {
        //Debug.LogError("BuffDamage called");
        _damageAmount = _damageAmount * 2;
    }

    public void StopBuff()
    {
        //Debug.LogError("StopBuff called");
        _damageAmount = _damageAmount / 2;
    }
    #endregion

    void Shooting()
    {
        if (_shootingPoint == null || _towerShootingHead == null) return;

        // Making sure the shooting point follows the orientation of the cannon
        RaycastHit hit;
        Vector3 forward = _shootingPoint.transform.forward;

        if (Physics.Raycast(_shootingPoint.position, forward, out hit, _shootingDistance))
        {
            Debug.DrawRay(_shootingPoint.position, forward * hit.distance, Color.red, 0.2f);


            Health enemy = hit.transform.GetComponent<Health>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damageAmount);       
            }

            // Instantiate FX feedback at the end of canon
            GameObject newMuzzleFlash = Instantiate(_feedbackFXOut, _shootingPoint.position, _shootingPoint.rotation);
            Destroy(newMuzzleFlash, 1);
        }
    }

    IEnumerator DoShooting()
    {
        while (true)
        {
            if (_target != null)
            {
                Shooting();
            }
        

            yield return new WaitForSeconds(_shootingCooldown);
        }
    }
}
