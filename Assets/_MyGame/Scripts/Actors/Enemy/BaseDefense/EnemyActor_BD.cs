using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

namespace Curio.Gameplay
{
    public class EnemyActor_BD : EnemyActor
    {
        public override void InitializeActor(TeamManager teamManager, int teamID)
        {
            
        }

        public void InitializeActorBD(float healtMultiplier, float damageMultiplier)
        {
            selectedGunID = weaponSwitch.RandomGunIndex;
            isAlive = true;
            weaponSwitch.SwitchWeapon(selectedGunID);
            currentWeapon = weaponSwitch.SelectedGun;
            currentWeapon.InitializeWeapon(_teamID, this);
            currentWeapon.SetParentTransform(transform);
            AddWeaponListners();

            characterIK.SetIK(currentWeapon.LeftArmIk);
            anim.runtimeAnimatorController = currentWeapon.WeaponConfig.WeaponAnimController;

            healthScript = new HealthScript();
            healthScript.SetHealth(Mathf.FloorToInt(health * healtMultiplier));

            actorApperence.SelectApperence(0);//0 for red
            healthFillBar.SetFillvalue(1);
            healthScript.OnHealthRatioChangeEvent.AddListener(healthFillBar.SetFillvalue);
            healthFillBar.SetFillColor(redTeamColor);

            pickUpDrop.PickUpWeaponIndex(selectedGunID);

            enemy.InitializeEnemy(this);
        }

        public override void Damage(ProjectileData projectileData)
        {
            if (projectileData.attackerActor.IsAlive)
            {
                if (healthScript.Damage(projectileData.damage) <= 0)
                {
                    gameObject.SetActive(false);
                    onDeadEvent?.Invoke(this);
                    pickUpDrop.Drop();
                }
            }
        }

        public override void ListenFriendDeathAlert(Transform deathLocation)
        {
           
        }

        public override void DeathMatchTimeUp()
        {
            
        }

        public override void RespawnActor()
        {
            
        }

        protected override void Update()
        {
            
        }
    }
}