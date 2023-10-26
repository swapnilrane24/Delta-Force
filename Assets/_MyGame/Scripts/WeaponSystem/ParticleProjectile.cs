using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

namespace Curio.Gameplay
{
    public class ParticleProjectile : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private List<ParticleCollisionEvent> collisionEvents;
        [SerializeField] private int damage = 5;
        [SerializeField] private float distance = 50;

        private bool hitEnvironment;
        protected int _teamID;
        protected Actor _actor;
        protected float actualSpeed;

        // Start is called before the first frame update
        void Start()
        {
            collisionEvents = new List<ParticleCollisionEvent>();
        }

        public void InitializeProjectile(int damageVal, float speed, float range, int teamID, Actor actor)
        {
            _teamID = teamID;
            distance = range;
            damage = damageVal;
            actualSpeed = speed;
            _actor = actor;
        }

        void OnParticleCollision(GameObject other)
        {
            int numCollisionEvents = particle.GetCollisionEvents(other, collisionEvents);

            bool canDamage = false;
            hitEnvironment = true;

            if (other.TryGetComponent<Actor>(out Actor actor))
            {
                hitEnvironment = false;
                if (actor.TeamID != _teamID)
                {
                    canDamage = true;
                }
            }

            if (canDamage)
            {
                ProjectileData projectileData = new ProjectileData();
                projectileData.damage = damage;
                projectileData.attackerActor = _actor;
                other.SendMessage("Damage", projectileData, SendMessageOptions.DontRequireReceiver);
                if (_actor.IsPlayer)
                {
                    DeathMatchManager.Instance.GetDamageNumber().Spawn(transform.position, damage);
                }
            }

            if (canDamage || hitEnvironment)
            {

                //HitFx();
                gameObject.SetActive(false);
            }
        }
    }
}