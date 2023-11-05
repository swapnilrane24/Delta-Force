using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Micosmo.SensorToolkit;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using Micosmo.SensorToolkit.Example;

namespace Curio.Gameplay
{
    public class PlayerActor : Actor
    {
        [SerializeField] private float deathMatchVirtualCameFov, defenseVirtualCameFov;
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private Color blueTeamColor, redTeamColor;
        [SerializeField] private GameObject statsCanvas;
        [SerializeField] private MMF_Player respawnFeedback;
        [SerializeField] private GameObject shieldFx;
        [SerializeField] protected WeaponListData weaponData;
        [SerializeField] private PlayerCharacterController playerCharacterController;
        [SerializeField] private FillBarUI healthFillBar;
        [SerializeField] private ActorApperenceDecide actorApperence;
        [SerializeField] private RaySensor enemyDetection;
        [SerializeField] private Image gunImage;
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private GameObject reloadinIndicator;

        private bool isShieldOn;

        public override void InitializeActor(TeamManager teamManager, int teamID)
        {
            cinemachineVirtualCamera.m_Lens.FieldOfView = deathMatchVirtualCameFov;

            isPlayer = true;
            selectedGunID = weaponData.SelectedWeaponID;
            base.InitializeActor(teamManager, teamID);
            playerCharacterController.InitializePlayerController(this);
            healthFillBar.SetFillvalue(1);
            healthScript.OnHealthRatioChangeEvent.AddListener(healthFillBar.SetFillvalue);
            actorApperence.SelectApperence(_teamID);
            actorName = "You";
            actorNameUI.SetActorName("YOU");
            actorNameUI.SetTextColor(_teamID == 0 ? redTeamColor : blueTeamColor);

            if (enemyDetection)
            {
                enemyDetection.Length = currentWeapon.AttackRange;
                enemyDetection.OnDetected.AddListener(DetectedEnemy);
                enemyDetection.OnLostDetection.AddListener(LostEnemy);
            }
        }

        public void InitializePlayer_BD()
        {
            cinemachineVirtualCamera.m_Lens.FieldOfView = defenseVirtualCameFov;
            isAlive = true;
            isPlayer = true;
            _teamID = 1;//enemy is 0, battery is 1
            selectedGunID = weaponData.SelectedWeaponID;
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

            playerCharacterController.InitializePlayerController(this);
            healthFillBar.SetFillvalue(1);
            healthScript.OnHealthRatioChangeEvent.AddListener(healthFillBar.SetFillvalue);
            actorApperence.SelectApperence(1);//we know player is blue team
            //actorName = "You";
            actorNameUI.SetActorName("");
            actorNameUI.SetTextColor(blueTeamColor);

            if (enemyDetection)
            {
                enemyDetection.Length = currentWeapon.AttackRange;
                enemyDetection.OnDetected.AddListener(DetectedEnemy);
                enemyDetection.OnLostDetection.AddListener(LostEnemy);
            }
        }

        protected override void SetGunInfo()
        {
            //Debug.Log("SetGunInfo:" + currentWeapon.AmmoLeft);
            gunImage.sprite = currentWeapon.WeaponConfig.WeaponIcon;
            ammoText.text = "" + currentWeapon.AmmoLeft;
        }

        protected override void ReloadStart()
        {
            base.ReloadStart();
            reloadinIndicator.SetActive(true);
        }

        protected override void ReloadEnd()
        {
            base.ReloadEnd();
            ammoText.text = "" + currentWeapon.AmmoLeft;
            reloadinIndicator.SetActive(false);
        }

        private void DetectedEnemy(GameObject value, Sensor sensor)
        {
            if (value.TryGetComponent<EnemyActor>(out EnemyActor enemy))
            {
                if (enemy.TeamID != TeamID)
                    enemy.DetectedByPlayer(true);
            }
        }

        private void LostEnemy(GameObject value, Sensor sensor)
        {
            if (value.TryGetComponent<EnemyActor>(out EnemyActor enemy))
            {
                if (enemy.TeamID != TeamID)
                    enemy.DetectedByPlayer(false);
            }
        }

        public void SetPosition(Vector3 position)
        {
            playerCharacterController.SetPosition(position);
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    SwitchWeaponCheat();
            //}

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (currentWeapon.CanReload)
                {
                    currentWeapon.StartReload();
                }
            }

            if (isAlive)
            {
                healthScript.HealthRegenerationProcess();
            }
        }

        private void SwitchWeaponCheat(int gunId)
        {
            if (currentWeapon)
            {
                RemoveWeaponListners();
            }

            selectedGunID = gunId;
            //selectedGunID++;

            weaponSwitch.SwitchWeapon(selectedGunID % weaponData.WeaponsList.Length);
            currentWeapon = weaponSwitch.SelectedGun;
            currentWeapon.InitializeWeapon(_teamID, this);
            currentWeapon.SetParentTransform(transform);
            AddWeaponListners();

            characterIK.SetIK(currentWeapon.LeftArmIk);
            anim.runtimeAnimatorController = currentWeapon.WeaponConfig.WeaponAnimController;

            enemyDetection.Length = currentWeapon.AttackRange;
            SetGunInfo();
        }

        public override void Damage(ProjectileData projectileData)
        {
            if (isShieldOn) return;
            base.Damage(projectileData);
        }

        public void SetShieldStatus(bool isActive)
        {
            //Debug.Log(isActive);
            isShieldOn = isActive;
        }

        public override void RespawnActor()
        {
            //GameAdsManager.Instance.ShowNormalAd(() =>
            //{
                respawnFeedback.PlayFeedbacks();
                shieldFx.SetActive(true);
                isAlive = true;
                healthScript.SetHealth(health);
                gameObject.SetActive(true);
                playerCharacterController.SetPosition(_teamManager.GetRandomSpawnPoint());
                healthFillBar.SetFillvalue(1);
                SetShieldStatus(true);
                currentWeapon.ResetWeapon();
                SetGunInfo();
                reloadinIndicator.SetActive(false);
            //});
        }

        public override void DeathMatchTimeUp()
        {
            base.DeathMatchTimeUp();
            statsCanvas.SetActive(false);
        }

        public override void CollecteWeapon(int weaponIndex)
        {
            //base.CollecteWeapon(weaponIndex);
            SwitchWeaponCheat(weaponIndex);
            SetGunInfo();
        }

    }
}