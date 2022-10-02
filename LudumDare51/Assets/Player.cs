using Ludum51.Player.Stat;
using UnityEngine;

namespace Ludum51.Player
{
    public class Player : MonoBehaviour
    {
        public PlayerStat Strength;
        public PlayerStat Dexterity;
        public PlayerStat Health;

        public void Start()
        {
            Health.BaseValue = 10;
            Strength.BaseValue = 5;
            Dexterity.BaseValue = 2;
        }
    }



    public class PowerUp
    {
        public string Name;

        public void Equip(Player player)
        {
            player.Strength.AddModifier(new Stat.Modifier.StatModifier(10, Stat.Modifier.StatModType.Flat));
            Debug.Log(player.Strength.Value);
        }

        public void Unequip(Player player)
        {

        }
    }
}