using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

namespace Curio.Gameplay
{
    public class Battery : IActor
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private FillBarUI healthFillBar;
        [SerializeField] private Material damageMat;
        [SerializeField] private MeshRenderer batteryMesh;
        [SerializeField] private MMF_Player hitFeedback;
        [SerializeField] private MMF_Player destroyFeedback;


        private HealthScript healthScript;

        public UnityEvent<IActor> onDeadEvent;
        private bool isAlive;

        public override int TeamID => 1; //enemy is 0, battery is 1

        public override UnityEvent<IActor> OnDeadEvent => onDeadEvent;

        public override Transform ActorTransfrom => transform;

        public override bool IsAlive => isAlive;

        public override bool IsPlayer => false;

        public override string ActorName => "Battery";

        private BaseArena _baseArena;

        private void Start()
        {
            healthScript = new HealthScript();
            healthScript.SetHealth(maxHealth);
            healthFillBar.SetFillvalue(1);
            healthScript.OnHealthRatioChangeEvent.AddListener(healthFillBar.SetFillvalue);
            isAlive = true;
        }

        public void SetBaseArena(BaseArena baseArena)
        {
            _baseArena = baseArena;
        }

        public override void Damage(ProjectileData projectileData)
        {
            Damage(projectileData.damage);
        }

        public override void Damage(int value)
        {
            hitFeedback.PlayFeedbacks();
            if (healthScript.Damage(value) <= 0)
            {
                if (isAlive)
                {
                    isAlive = false;
                    destroyFeedback.PlayFeedbacks();
                    batteryMesh.material = damageMat;
                    _baseArena.BatteryDestroyed(this);
                }
            }
        }
    }
}