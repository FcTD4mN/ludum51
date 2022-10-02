using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ludum51.Player.Stat
{

    [Serializable]
    public class PlayerStat
    {
        public float BaseValue;
        public float Value
        {
            get
            {
                if (isCalculated || BaseValue != lastBaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isCalculated = false;
                }
                return _value;
            }
        }

        protected bool isCalculated = true;
        protected float _value;
        protected readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> getStatModifiers;
        protected float lastBaseValue = float.MinValue;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public PlayerStat()
        {
            statModifiers = new List<StatModifier>();
            getStatModifiers = statModifiers.AsReadOnly();
        }

        public PlayerStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        public void AddModifier(StatModifier mod)
        {
            isCalculated = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        public int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
            {
                return -1;
            }
            else if (a.Order > b.Order)
            {
                return 1;
            }
            return 0;
        }

        public bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isCalculated = true;
                return true;
            }
            return false;
        }

        public bool RemoveAllModifierFromSource(object source)
        {
            bool didRemove = false;

            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isCalculated = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        protected float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];
                if (mod.Type == StatModType.Flat)
                {
                    finalValue += statModifiers[i].Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }
            return (float)Math.Round(finalValue, 4);
        }


    }
}