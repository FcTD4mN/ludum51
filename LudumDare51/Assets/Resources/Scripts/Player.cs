using Ludum51.Player.Stat;
using UnityEngine;
using System.Collections.ObjectModel;

namespace Ludum51.Player
{
    public class Player : MonoBehaviour
    {
        public PlayerStat Health;
        public PlayerStat Strength;
        public PlayerStat Dexterity;
        public float test { get { return this.getStat(); } }

        public void Initialize()
        {
            Health.BaseValue = 100;
            Strength.BaseValue = 5;
            Dexterity.BaseValue = 2;
            PowerUp PU = new PowerUp();
            PU.Equip(this);
        }

        void update()
        {

        }

        public float getStat()
        {
            return this.Strength.Value;
        }

    }


    public class PowerUp
    {
        public string Name;

        public void Equip(Player player)
        {
            player.Strength.AddModifier(new Stat.StatModifier(10, Stat.StatModType.Flat, this));
        }

        public void Unequip(Player player)
        {

        }
    }
}