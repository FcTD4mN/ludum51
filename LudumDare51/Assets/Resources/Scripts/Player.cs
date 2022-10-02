using Ludum51.Player.Stat;
using UnityEngine;
using System.Collections.ObjectModel;

namespace Ludum51.Player
{
    public class Player : MonoBehaviour
    {
        public PlayerStat Health;
        public PlayerStat Damage;
        public PlayerStat Speed;
        public PlayerStat WeaponSpeed;
        public PlayerStat projectile;
        public PlayerStat Cooldown;
        public PlayerStat Zone;

        public float test { get { return this.getStat(); } }

        void Start()
        {
            Health.BaseValue = 100;
            Damage.BaseValue = 5;
            Speed.BaseValue = 2;
            PowerUp PU = new PowerUp();
            PU.Equip(this);
            Debug.Log(test);
        }

        void update()
        {

        }

        public void pushCard(Card card)
        {
            Debug.Log("wiaejdfqoi");
        }

        public float getStat()
        {
            return this.Damage.Value;
        }

    }


    public class PowerUp
    {
        public string Name;

        public void Equip(Player player)
        {
            /* player.Strength.AddModifier(new Stat.StatModifier(10, Stat.StatModType.Flat, this)); */
        }

        /* public receiveCard(float value, StatModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        public receiveCard(float value, StatModType type) : this(value, type, (int)type, null) { } */

        public void Unequip(Player player)
        {

        }
    }
}