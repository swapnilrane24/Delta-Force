using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

namespace Curio.Gameplay
{
    public abstract class Actor : IActor
    {
        [SerializeField] protected SoundPlay soundPlay;
        [SerializeField] protected int health;
        [SerializeField] protected WeaponSwitch weaponSwitch;
        [SerializeField] protected Animator anim;
        [SerializeField] protected CharacterIK characterIK;
        [SerializeField] private string walkAnimKey = "Walking";
        [SerializeField] private string shootAnimKey = "Shoot";
        [SerializeField] private string groundAnimKey = "Ground";
        [SerializeField] private string xSpeedAnimKey = "XSpeed";
        [SerializeField] private string ySpeedAnimKey = "YSpeed";
        [SerializeField] private string reloadAnimKey = "Reloading";
        [SerializeField] protected ActorNameUI actorNameUI;
        [SerializeField] protected MMF_Player damageIncreaseFeedback;
        [SerializeField] protected MMF_Player fireRateIncreaseFeedback;
        [SerializeField] protected MMF_Player decreaseFeedback;
        [SerializeField] protected MMF_Player healthIncreaseFeedback;

        protected string actorName;
        protected WeaponController currentWeapon;
        [SerializeField] protected int selectedGunID;
        [SerializeField] protected int _teamID;

        public WeaponController SelectedWeapon => currentWeapon;
        //public UnityEvent<Actor> onDeadEvent;

        public UnityEvent<IActor> onDeadEvent;
        public override UnityEvent<IActor> OnDeadEvent => onDeadEvent;

        public override int TeamID => _teamID;

        public override Transform ActorTransfrom => transform;

        public bool CanPickUpHealth => healthScript.canPickUpHealth;
        protected bool isAlive = true;
        protected HealthScript healthScript;

        protected TeamManager _teamManager;
        protected bool isPlayer;

        private float currentCoundDownTimer;
        public override bool IsPlayer => isPlayer;
        public override bool IsAlive => isAlive;

        public override string ActorName => actorName;

        //

        private void OnDisable()
        {
            //DeathMatchManager.Instance.onDeathMatchFinishEvent.RemoveListener(() => isAlive = true);
            //RemoveWeaponListners();
        }

        public virtual void InitializeActor(TeamManager teamManager, int teamID)
        {
            isAlive = true;
            _teamManager = teamManager;
            _teamID = teamID;
            //selectedGunID = weaponData.SelectedWeaponID;
            weaponSwitch.SwitchWeapon(selectedGunID);
            currentWeapon = weaponSwitch.SelectedGun;
            currentWeapon.InitializeWeapon(_teamID, this);
            currentWeapon.SetParentTransform(transform);
            AddWeaponListners();

            characterIK.SetIK(currentWeapon.LeftArmIk);
            anim.runtimeAnimatorController = currentWeapon.WeaponConfig.WeaponAnimController;

            SetGunInfo();

            healthScript = new HealthScript();
            healthScript.SetHealth(health);

            //DeathMatchManager.Instance.onDeathMatchFinishEvent.AddListener(() => isAlive = false);
        }


        protected virtual void SetGunInfo()
        {

        }

        protected virtual void RemoveWeaponListners()
        {
            currentWeapon?.onFireEvent.RemoveListener(SetGunInfo);

            currentWeapon?.onReloadingEndEvent.RemoveListener(() =>
            {
                //isReloading = false;
                ReloadEnd();
            });

            currentWeapon?.onReloadingStartEvent.RemoveListener(() =>
            {
                //isReloading = true;
                ReloadStart();
            });
        }

        protected virtual void AddWeaponListners()
        {
            currentWeapon?.onFireEvent.AddListener(SetGunInfo);

            currentWeapon.onReloadingEndEvent.AddListener(() =>
            {
                //isReloading = false;
                ReloadEnd();
            });

            currentWeapon.onReloadingStartEvent.AddListener(() =>
            {
                //isReloading = true;
                ReloadStart();
            });
        }

        protected virtual void ReloadStart()
        {
            anim.SetBool(reloadAnimKey, true);
            characterIK.ActivateIK(false);
        }

        protected virtual void ReloadEnd()
        {
            anim.SetBool(reloadAnimKey, false);
            characterIK.ActivateIK(true);
        }

        public virtual void SetActorWalkAnim(bool value)
        {
            anim.SetBool(walkAnimKey, value);
        }

        public virtual void SetActorGrounded(bool value)
        {
            anim.SetBool(groundAnimKey, value);
        }

        public virtual void SetActorShootAnim(bool value)
        {
            anim.SetBool(shootAnimKey, value);
        }

        public virtual void SetActorDirectionAnim(Vector2 direction)
        {
            anim.SetFloat(xSpeedAnimKey, direction.x);
            anim.SetFloat(ySpeedAnimKey, direction.y);
        }

        public override void Damage(ProjectileData projectileData)
        {
            if (projectileData.attackerActor.IsAlive)
            {
                if (healthScript.Damage(projectileData.damage) <= 0)
                {
                    //AlertFriendsOnDeath();
                    isAlive = false;
                    actorNameUI.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                    _teamManager.ActorDead(this);
                    currentCoundDownTimer = DeathMatchManager.Instance.RespawnTime;
                    onDeadEvent?.Invoke(this);
                    bool isRedTeam = TeamID == 0 ? true : false;//we know 0 is for red and 1 is for blue
                                                                //if victim is from red team then attacker is from blue team
                    bool isAttackerTeamRead = isRedTeam == true ? false : true;
                    DeathMatchManager.Instance.KillListner(new KillUIData(projectileData.attackerActor.ActorName, actorName, isAttackerTeamRead));
                }
            }
        }

        public virtual void ListenFriendDeathAlert(Transform deathLocation)
        {

        }

        public virtual void RespawnActor()
        {
            if (GameManager.Instance.GameState == GameState.PLAYING)
            {
                isAlive = true;
                healthScript.SetHealth(health);
                transform.position = _teamManager.GetRandomSpawnPoint();// SpawnPoints.pointsList[Random.Range(0, SpawnPoints.pointsList.Length)].position;
                actorNameUI.gameObject.SetActive(true);
                gameObject.SetActive(true);
            }
        }

        public void CountDownTimer()
        {
            if (IsPlayer == false)
            {
                currentCoundDownTimer -= Time.deltaTime;
                if (currentCoundDownTimer <= 0)
                {
                    RespawnActor();
                }
            }
        }

        public virtual void DeathMatchTimeUp()
        {
            isAlive = false;
        }

        public virtual void CollectFireRatePickUp(float fireRateChange)
        {
            fireRateIncreaseFeedback.PlayFeedbacks();
            currentWeapon.IncreaseFireRate(fireRateChange);
        }

        public virtual void CollectDamagePickUp(float damageChange)
        {
            damageIncreaseFeedback.PlayFeedbacks();
            currentWeapon.IncreaseDamage(damageChange);
        }


        public virtual void CollectHealthPickUp(float healthChange)
        {
            healthIncreaseFeedback.PlayFeedbacks();
            int changeInHealth = Mathf.FloorToInt(health * healthChange);
            //Debug.Log("HP:" + changeInHealth);
            healthScript.IncreaseHealth(changeInHealth);
        }

        public virtual void CollectCoinPickUp()
        {

        }

        public virtual void CollecteWeapon(int weaponIndex)
        {

        }

        public override void Damage(int value)
        {
            
        }
    }
}