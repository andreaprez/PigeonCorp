using UniRx;
using UnityEngine;

namespace PigeonCorp.Research.Adapter
{
    public class BonusViewModel
    {
        public readonly ReactiveProperty<bool> ButtonInteractable;
        public readonly ReactiveProperty<bool> ResearchAvailable;
        public readonly ReactiveProperty<string> Name;
        public readonly ReactiveProperty<string> CurrentValue;
        public readonly ReactiveProperty<string> NextValue;
        public readonly ReactiveProperty<float> Cost;

        public BonusViewModel()
        {
            ButtonInteractable = new ReactiveProperty<bool>();
            ResearchAvailable = new ReactiveProperty<bool>();
            Name = new ReactiveProperty<string>();
            CurrentValue = new ReactiveProperty<string>();
            NextValue = new ReactiveProperty<string>();
            Cost = new ReactiveProperty<float>();
        }
    }
}