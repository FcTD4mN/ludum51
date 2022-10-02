using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

namespace Ludum51.Player.Stat.Modifier
{
    public enum StatModType
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300,
    }

    public class StatModifier : MonoBehaviour
    {
        // Start is called before the first frame update
        public readonly float Value;
        public readonly StatModType Type;
        public readonly int Order;
        public readonly object Source;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public StatModifier(float value, StatModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }
        public StatModifier(float value, StatModType type, int order) : this(value, type, (int)type, null) { }
        public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
    }
}