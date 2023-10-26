using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        [System.Serializable]
        public enum DamagableActor
        {
            Player,
            Enemy
        }

        [SerializeField] private DamagableActor damagableActor;
        public float Speed = 10;
        public float distance = 50;
        //public Vector3 direction;
        public int damage = 5;

        protected int _teamID;
        protected Actor _actor;
        protected bool isProjectileAlive;
        protected float _path = 0;
        protected float actualSpeed;
        protected Vector3 hitPoint;

        protected ObjectPoolGeneric<Projectile> _objectPoolGeneric;
        private bool hitEnvironment;

        private void OnDisable()
        {
            ReturnItem();
        }

        private void OnEnable()
        {
            hitEnvironment = false;
            actualSpeed = Speed;
            _path = 0;
            isProjectileAlive = true;
        }

        protected virtual void Update()
        {
            transform.position += transform.forward * actualSpeed * Time.deltaTime;
            _path += actualSpeed * Time.deltaTime;

            if (_path >= distance)
            {
                gameObject.SetActive(false);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
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
                //hitPoint = other.ClosestPoint(transform.position);
                HitFx();
                gameObject.SetActive(false);
            }
        }

        protected void HitFx()
        {

        }

        public virtual void InitializeProjectile(int damageVal, float speed, float range, int teamID, Actor actor)
        {
            _teamID = teamID;
            distance = range;
            damage = damageVal;
            actualSpeed = speed;
            _path = 0;
            _actor = actor;
        }

        public void SetObjectPoolGeneric(ObjectPoolGeneric<Projectile> objectPoolGeneric)
        {
            _objectPoolGeneric = objectPoolGeneric;
        }

        public void ReturnItem()
        {
            if (_objectPoolGeneric != null)
                _objectPoolGeneric.ReturnObject(this);
        }

    }
}