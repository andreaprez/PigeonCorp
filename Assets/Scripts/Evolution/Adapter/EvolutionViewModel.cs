using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace PigeonCorp.Evolution.Adapter
{
    public class EvolutionViewModel
    {
        public readonly ReactiveProperty<bool> IsOpen;
        public readonly ReactiveProperty<Sprite> PigeonIcon;
        public readonly ReactiveProperty<string> PigeonName;
        public readonly ReactiveProperty<float> EggValue;
        public readonly ReactiveProperty<float> CurrentFarmValue;
        public readonly ReactiveProperty<float> RequiredFarmValue;
        public readonly ReactiveProperty<bool> ButtonInteractable;
        public readonly ReactiveProperty<bool> EvolutionAvailable;
        public readonly List<EvolutionEggViewModel> EvolutionEggViewModels;


        public EvolutionViewModel()
        {
            IsOpen = new ReactiveProperty<bool>();
            PigeonIcon = new ReactiveProperty<Sprite>();
            PigeonName = new ReactiveProperty<string>();
            EggValue = new ReactiveProperty<float>();
            CurrentFarmValue = new ReactiveProperty<float>();
            RequiredFarmValue = new ReactiveProperty<float>();
            ButtonInteractable = new ReactiveProperty<bool>();
            EvolutionAvailable = new ReactiveProperty<bool>();
            EvolutionEggViewModels = new List<EvolutionEggViewModel>();
        }
    }
}