using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class SimpleBullet : MonoBehaviour, IProjectile, IPoolable
{
    
    
    //after this time will be destroyed
    [SerializeField]
    private float _bulletLifeTime = 2.0f;
    
    //main projectile settings, AbstractWeapon set this
    private float _bulletSpeed;
    private float _damage;
    
    private RaycastHit hit;
    
    //Time, when bullet must be destroyed
    private float _endOfLife;
    
    private LayerMask occlusionLayers;
    
    //cache for object pool
    private ObjectPoolContainer poolContainer;


    private void OnEnable()
    {
        ResetItem();
    }

    //set timer
    void Start()
    {
        _endOfLife = Time.time + _bulletLifeTime;
    }

    // Check time to destroy projectile
    void Update()
    {
        CheckDestroyTimer();
    }
    
    
    private void FixedUpdate()
    {
        MoveProjectile();
        CheckObjectsAhead();
    }


    private void CheckDestroyTimer()
    {
        if(Time.time > _endOfLife)
            poolContainer.GetPool.Release(this);
    }


    private void MoveProjectile()
    {
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
    }

    
    //Function prevents "tunneling" fast projectiles through objects
    private void CheckObjectsAhead()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                out hit, _bulletSpeed * 0.02f, occlusionLayers))
        {
            if (hit.transform.TryGetComponent<IDamagable>(out IDamagable target))
            {
                target.TakeDamage(_damage);
            }
            poolContainer.GetPool.Release(this);
        }
    }


    public void SetOcclusionLayers(LayerMask mask)
    {
        occlusionLayers = mask;
    }

    public void SetParentPool(ObjectPoolContainer poolsContainer)
    {
        poolContainer = poolsContainer;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void GetFromPool()
    {
        this.gameObject.SetActive(true);
    }
    
    public void ReleaseToPool()
    {
        this.gameObject.SetActive(false);
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetSpeed(float speed)
    {
        _bulletSpeed = speed;
    }

    public void ResetItem()
    {
        _endOfLife = Time.time + _bulletLifeTime;
    }





}