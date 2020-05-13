using System;
using System.Collections.Generic;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;

namespace PigeonCorp.Research
{
    public class ResearchModel
    {
        public readonly List<BonusModel> Bonuses;
        
        private readonly ResearchTitleData _config;
        private readonly ResearchUserData _userData;
        
        private readonly IApplicableBonus _buyButtonApplicable;
        private readonly IApplicableBonus _hatcheriesApplicable;
        private readonly IApplicableBonus _shippingApplicable;
        private readonly IApplicableBonus _researchApplicable;
        private readonly IApplicableBonus _evolutionApplicable;

        public ResearchModel(
            ResearchTitleData config,
            ResearchUserData userData,
            IApplicableBonus buyButtonApplicable,
            IApplicableBonus hatcheriesApplicable,
            IApplicableBonus shippingApplicable,
            IApplicableBonus researchApplicable
        )
        {
            _config = config;
            _userData = userData;
            
            _buyButtonApplicable = buyButtonApplicable;
            _hatcheriesApplicable = hatcheriesApplicable;
            _shippingApplicable = shippingApplicable;
            _researchApplicable = researchApplicable;

            Bonuses = new List<BonusModel>();
            InitBonuses();
        }
        
        public ResearchUserData Serialize()
        {
            return new ResearchUserData(this);
        }
        
        public BonusModel GetBonusByType(BonusType type)
        {
            foreach (var bonus in Bonuses)
            {
                if (bonus.Type == type)
                {
                    return bonus;
                }
            }

            throw new Exception("No bonus found of type: " + type);
        }
        
        private void InitBonuses()
        {
            for (int i = 0; i < _config.BonusTypesConfiguration.Count; i++)
            {
                var bonusConfig = _config.BonusTypesConfiguration[i];
                var bonusState = FindBonusStateByType(bonusConfig.Type);

                BonusModel bonus = null;
                switch (bonusConfig.Type)
                {
                    case BonusType.BUY_BUTTON_RATE_MULTIPLIER:
                        bonus = new MultiplierBonus(bonusConfig, bonusState, _buyButtonApplicable);
                        break;
                    case BonusType.EGG_VALUE_MULTIPLIER:
                        //bonus = new MultiplierBonus(bonusConfig, bonusState, _evolutionApplicable);
                        break;
                    case BonusType.EGG_LAYING_RATE_MULTIPLIER:
                        bonus = new MultiplierBonus(bonusConfig, bonusState, _hatcheriesApplicable);
                        break;
                    case BonusType.HATCHERY_CAPACITY_INCREMENT:
                        bonus = new IncrementBonus(bonusConfig, bonusState, _hatcheriesApplicable);
                        break;
                    case BonusType.VEHICLE_SHIPPING_RATE_INCREMENT:
                        bonus = new IncrementBonus(bonusConfig, bonusState, _shippingApplicable);
                        break;
                    case BonusType.RESEARCH_DISCOUNT:
                        bonus = new DiscountBonus(bonusConfig, bonusState, _researchApplicable);
                        break;
                    case BonusType.HATCHERY_DISCOUNT:
                        bonus = new DiscountBonus(bonusConfig, bonusState, _hatcheriesApplicable);
                        break;
                    case BonusType.VEHICLE_DISCOUNT:
                        bonus = new DiscountBonus(bonusConfig, bonusState, _shippingApplicable);
                        break;
                    case BonusType.PIGEON_DISCOUNT:
                        //bonus = new DiscountBonus(bonusConfig, bonusState, ???);
                        break;
                    default:
                        throw new Exception("No bonus found of type: " + bonusConfig.Type);
                }
                if (bonus != null) Bonuses.Add(bonus);
            }
        }
        
        private BonusState FindBonusStateByType(BonusType type)
        {
            foreach (var bonus in _userData.Bonuses)
            {
                if (bonus.Type == type)
                {
                    return bonus;
                }
            }
            
            var defaultBonus = new BonusState()
            {
                CurrentTier = 0,
                Type = type
            };
            return defaultBonus;
        }
    }
}