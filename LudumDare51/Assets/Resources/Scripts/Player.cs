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
        public PlayerStat Projectile;
        public PlayerStat Cooldown;
        public PlayerStat Zone;
        private PowerUp PU = new PowerUp();

        public float test { get { return this.getStat(); } }

        public void Initialize()
        {
            Health.BaseValue = 100;
            Damage.BaseValue = 5;
            WeaponSpeed.BaseValue = 1;
            Projectile.BaseValue = 1;
            Cooldown.BaseValue = 1;
            Zone.BaseValue = 1;
        }

        void update()
        {

        }

        public void pushCard(Card card)
        {
            PU.Equip(this, card);
        }

        public float getStat()
        {
            return this.Damage.Value;
        }

    }


    public class PowerUp
    {
        public string Name;

        public void Equip(Player player, Card card)
        {
            /* player.Strength.AddModifier(new Stat.StatModifier(10, Stat.StatModType.Flat, this)); */
            switch (card.mPowerUpCategory)
            {
                case PowerUpCategory.Health:
                    player.Health.AddModifier(new Stat.StatModifier(card.mPoints, card.mType, this));
                    break;
                case PowerUpCategory.Damage:
                    player.Damage.AddModifier(new Stat.StatModifier(card.mPoints, card.mType, this));
                    break;
                case PowerUpCategory.Speed:
                    player.Speed.AddModifier(new Stat.StatModifier(card.mPoints, card.mType, this));
                    break;
                case PowerUpCategory.WeaponSpeed:
                    player.WeaponSpeed.AddModifier(new Stat.StatModifier(card.mPoints, card.mType, this));
                    break;
                case PowerUpCategory.Projectile:
                    player.Projectile.AddModifier(new Stat.StatModifier(card.mPoints, card.mType, this));
                    break;
                case PowerUpCategory.Cooldown:
                    player.Cooldown.AddModifier(new Stat.StatModifier(card.mPoints, card.mType, this));
                    break;
                case PowerUpCategory.Zone:
                    player.Zone.AddModifier(new Stat.StatModifier(card.mPoints, card.mType, this));
                    break;
            }
        }

        public void Unequip(Player player)
        {

        }
    }
}