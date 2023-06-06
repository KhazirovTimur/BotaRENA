using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaycastShot : MonoBehaviour, IShootMechanic
{
    public LayerMask occlusionLayers;
    public GameObject hitEffect;
    private ObjectPoolContainer _hitEffectsPool;
    private AbstractWeapon thisWeapon;

    private void Start()
    {
        thisWeapon = GetComponent<AbstractWeapon>();
        _hitEffectsPool = FindObjectOfType<AllObjectPoolsContainer>()
            .CreateNewPool(hitEffect.GetComponent<IPoolable>(), thisWeapon.GetDefaultPoolCapacity());
    }

    public void DoShot(Transform barrelEnd, float damage)
    {
        if (Physics.Raycast(transform.position, barrelEnd.forward,
                out RaycastHit hit, 1000, occlusionLayers))
        {
            IPoolable bulletHole = _hitEffectsPool.GetPool.Get();
            GameObject newHole = bulletHole.GetGameObject();
            newHole.transform.position = hit.point;
            newHole.transform.rotation = Quaternion.FromToRotation(newHole.transform.forward, -hit.normal);
            newHole.transform.SetParent(hit.transform);
            if (hit.transform.TryGetComponent<IDamagable>(out IDamagable target))
            {
                target.TakeDamage(damage);
            }
        }
        Debug.DrawLine(barrelEnd.position, barrelEnd.position + barrelEnd.forward * 1000, Color.blue, 0.5f);
    }
    
    
    
}
