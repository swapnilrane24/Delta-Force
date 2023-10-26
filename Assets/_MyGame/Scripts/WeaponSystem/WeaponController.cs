using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Curio.Gameplay
{
    public class WeaponController : MonoBehaviour
    {
        //[SerializeField] protected bool isPlayerWeapon = false;
        [SerializeField] protected Transform projectileSpawnPosition;
        [SerializeField] protected Transform raycastStartSpot;
        [SerializeField] protected Transform leftArmIk;

        [SerializeField] protected WeaponConfig weaponConfig;

        [Header("Effect")]
        [SerializeField] protected SoundPlay soundPlay;
        [SerializeField] protected Material gunUnknowMat;
        [SerializeField] protected Material gunOriginalMat;
        [SerializeField] protected MeshRenderer[] gunMeshList;

        private float actualROF;
        private float currentAccuracy;
        private float fireTimer;
        private int ammoLeft;
        private bool weaponInitialized;
        private Transform _parentTransform;
        private bool isReloading;

        public UnityEvent onFireEvent;
        [HideInInspector] public UnityEvent onReloadingStartEvent;
        [HideInInspector] public UnityEvent onReloadingEndEvent;

        private float reloadWait;
        private Vector3 targetDirection;
        private int _teamID;

        public int AmmoLeft => ammoLeft;
        public float AmmoLeftToTotalRatio => ammoLeft / (1f * weaponConfig.MagazineSize);
        public Transform LeftArmIk => leftArmIk;
        public WeaponConfig WeaponConfig => weaponConfig;
        public bool IsReloading => isReloading;
        public bool CanReload => ammoLeft < weaponConfig.MagazineSize;
        public Vector3 TargetDirection { get => targetDirection; set => targetDirection = value; }
        public float AttackRange => weaponConfig.gunRange;

        private Actor _actor;
        private ObjectPoolGeneric<Projectile> projectilePool;
        private int actualDamage;

        public void SetParentTransform(Transform parentTransform)
        {
            _parentTransform = parentTransform;
        }

        public void InitializeWeapon(int teamID, Actor actor)
        {
            _actor = actor;
            _teamID = teamID;
            weaponConfig.LoadWeaponStats();

            if (weaponConfig.FireRate != 0)
                actualROF = 1.0f / weaponConfig.FireRate;
            else
                actualROF = 0.01f;

            fireTimer = actualROF;
            actualDamage = weaponConfig.Damage;

            ammoLeft = weaponConfig.MagazineSize;

            if (raycastStartSpot == null)
                raycastStartSpot = gameObject.transform;

            if (projectileSpawnPosition == null)
                projectileSpawnPosition = gameObject.transform;

            weaponInitialized = true;

            if (weaponConfig.WeaponFireType == WeaponFireType.PROJECTILE)
            {
                projectilePool = new ObjectPoolGeneric<Projectile>(SpawnProjectile,
                    ActivateProjectile, DeactivateProjectile, 1);
            }

            //reloadWait = new WaitForSeconds(weaponConfig.ReloadTime);
            //Debug.Log(ammoLeft + "|" + weaponConfig.MagazineSize);
        }


        public virtual void OnUpdate()
        {
            if (weaponInitialized == false) return;
            fireTimer += Time.deltaTime;

            if (weaponConfig.AutoReload && ammoLeft <= 0)
            {
                if (isReloading == false)
                    StartReload();
            }

            if (isReloading)
            {
                if (reloadWait < Time.time)
                {
                    isReloading = false;

                    if (weaponConfig.MagazineInFx && _actor.IsPlayer)
                        soundPlay.Play(weaponConfig.MagazineInFx);

                    ammoLeft = weaponConfig.MagazineSize;

                    onReloadingEndEvent?.Invoke();
                }
            }

            //CheckForUserInput();
        }

        public void CheckForUserInput()
        {
            if (isReloading) return;
            switch (weaponConfig.WeaponFireType)
            {
                case WeaponFireType.RAYCAST:
                    if (fireTimer >= actualROF)
                    {
                        RayFire();
                    }
                    break;
                case WeaponFireType.PROJECTILE:

                    if (fireTimer >= actualROF)
                    {
                        ProjectileFire();
                    }

                    break;
                case WeaponFireType.BEAM:
                    break;
            }
        }

        public void StartReload()
        {
            reloadWait = weaponConfig.ReloadTime + Time.time;
            isReloading = true;
            onReloadingStartEvent?.Invoke();

            if (weaponConfig.MagazineOutFx && _actor.IsPlayer)
                soundPlay.Play(weaponConfig.MagazineOutFx);
        }

        protected void RayFire()
        {
            fireTimer = 0;
            if (ammoLeft > 0)
            {
                if (!weaponConfig.IsInfiniteAmmo) ammoLeft--;

                for (int i = 0; i < weaponConfig.BulletsPerShot; i++)
                {
                    float accuracyVary = (100 - weaponConfig.gunAccuracy) / 500;
                    //Vector3 direction = raycastStartSpot.forward;
                    Vector3 direction = targetDirection;
                    direction.x += Random.Range(-accuracyVary, accuracyVary);
                    direction.y += Random.Range(-accuracyVary, accuracyVary);
                    direction.z += Random.Range(-accuracyVary, accuracyVary);

                    if (currentAccuracy <= 0.0f)
                        currentAccuracy = 0.0f;

                    Ray ray = new Ray(raycastStartSpot.position, direction);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, weaponConfig.gunRange))
                    {
                        hit.collider.SendMessage("Damage", actualDamage, SendMessageOptions.DontRequireReceiver);
                    }
                    
                }

                //Play Fire Sound
                soundPlay.Play(weaponConfig.FireSfx[Random.Range(0, weaponConfig.FireSfx.Length)]);
                onFireEvent?.Invoke();
 
            }
        }

        protected void ProjectileFire()
        {
            fireTimer = 0;
            if (ammoLeft > 0)
            {
                if (!weaponConfig.IsInfiniteAmmo) ammoLeft--;

                for (int i = 0; i < weaponConfig.BulletsPerShot; i++)
                {
                    float accuracyVary = (100 - weaponConfig.gunAccuracy) / 500;
                    //Vector3 direction = raycastStartSpot.forward;
                    //Vector3 direction = _parentTransform.forward;
                    Vector3 direction = targetDirection;
                    direction.x += Random.Range(-accuracyVary, accuracyVary);
                    //direction.y += Random.Range(-accuracyVary, accuracyVary);
                    direction.z += Random.Range(-accuracyVary, accuracyVary);

                    if (currentAccuracy <= 0.0f)
                        currentAccuracy = 0.0f;

                    if (weaponConfig.Projectile)
                    {
                        //Projectile proj = Instantiate(weaponConfig.Projectile);
                        Projectile proj = projectilePool.GetObject();
                        proj.transform.position = projectileSpawnPosition.position;
                        proj.transform.rotation = Quaternion.LookRotation(direction);
                        proj.InitializeProjectile(actualDamage, weaponConfig.ProjectileSpeed, weaponConfig.gunRange, _teamID, _actor);
                    }
                    else
                    {
                        Debug.Log("Projectile to be instantiated is null.  Make sure to set the Projectile field in the inspector.");
                    }

                }

                //Play Fire Sound
                soundPlay.Play(weaponConfig.FireSfx[Random.Range(0, weaponConfig.FireSfx.Length)]);
                onFireEvent?.Invoke();
            }
        }

        public void ResetWeaponRate()
        {
            fireTimer = actualROF;
        }

        public void ResetWeapon()
        {
            ammoLeft = weaponConfig.MagazineSize;
        }

        //Used by UI
        public void GunUnknown(bool isUnknown)
        {
            for (int i = 0; i < gunMeshList.Length; i++)
            {
                gunMeshList[i].material = isUnknown == true ? gunUnknowMat : gunOriginalMat;
            }
        }

        protected Projectile SpawnProjectile()
        {
            Projectile projectile = Instantiate(weaponConfig.Projectile);
            projectile.SetObjectPoolGeneric(projectilePool);
            return projectile;
        }

        protected void ActivateProjectile(Projectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        protected void DeactivateProjectile(Projectile projectile)
        {
            //objectPoolGeneric.ReturnObject(projectile);
            projectile.gameObject.SetActive(false);
        }

        public void IncreaseFireRate(float percent)
        {
            float oldRof = actualROF;
            actualROF = oldRof - percent * (1.0f / weaponConfig.FireRate);
            //Debug.Log("ROF:" + oldRof + "|" + actualROF);
        }

        public void IncreaseDamage(float percent)
        {
            float oldDamage = actualDamage;
            actualDamage = Mathf.CeilToInt(oldDamage + weaponConfig.Damage * percent);
            //Debug.Log("Dam:" + oldDamage + "|" + actualDamage);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (weaponConfig)
                Gizmos.DrawWireSphere(transform.position, weaponConfig.gunRange);
        }
#endif

    }
}