using UniRx;
using UnityEngine;

namespace PigeonCorp.Evolution.Entity
{
    public class EvolutionEggEntity
    {
        public readonly ReactiveProperty<bool> IsDiscovered;
        public readonly ReactiveProperty<Sprite> PigeonIcon;
        public readonly ReactiveProperty<string> PigeonName;
        public readonly ReactiveProperty<float> EggValue;

        public EvolutionEggEntity()
        {
            IsDiscovered = new ReactiveProperty<bool>();
            PigeonIcon = new ReactiveProperty<Sprite>();
            PigeonName = new ReactiveProperty<string>();
            EggValue = new ReactiveProperty<float>();
        }
    }
} 