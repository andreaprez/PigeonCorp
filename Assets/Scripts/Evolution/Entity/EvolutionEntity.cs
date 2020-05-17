using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Evolution.Entity
{
    public class EvolutionEntity
    {
        public readonly List<EvolutionEggEntity> EvolutionEggs;
        public readonly ReactiveProperty<int> CurrentEggId;
        public readonly ReactiveProperty<int> SelectedEggId;
        public readonly ReactiveProperty<Sprite> CurrentPigeonIcon;
        public readonly ReactiveProperty<string> CurrentPigeonName;
        public readonly ReactiveProperty<float> CurrentEggValue;
        public readonly ReactiveProperty<float> CurrentFarmValue;
        public readonly ReactiveProperty<float> RequiredFarmValue;

        public EvolutionEntity()
        {
            EvolutionEggs = new List<EvolutionEggEntity>();
            CurrentEggId = new ReactiveProperty<int>();
            SelectedEggId = new ReactiveProperty<int>();
            CurrentPigeonIcon = new ReactiveProperty<Sprite>();
            CurrentPigeonName = new ReactiveProperty<string>();
            CurrentEggValue = new ReactiveProperty<float>();
            CurrentFarmValue = new ReactiveProperty<float>();
            RequiredFarmValue = new ReactiveProperty<float>();
        }

        public void Evolve()
        {
            CurrentEggId.Value += 1;
        }
    }
}