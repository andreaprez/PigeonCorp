using System;
using PigeonCorp.Command;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;
using PigeonCorp.Research.Entity;
using PigeonCorp.ValueModifiers.UseCase;
using Zenject;

namespace PigeonCorp.Research.Adapter
{
    public class ResearchInstaller
    {
        private IApplicableBonus _buyButtonApplicable;
        private IApplicableBonus _hatcheriesApplicable;
        private IApplicableBonus _shippingApplicable;
        private IApplicableBonus _researchApplicable;
        private IApplicableBonus _evolutionApplicable;
        
        public void Install(
            ResearchEntity entity,
            ResearchUserData data,
            ResearchTitleData config,
            MainTopBarEntity mainTopBarEntity,
            ICommand<float> subtractCurrencyCommand,
            UC_GetMainBuyButtonValueModifiers getMainBuyButtonModifiersUC,
            UC_GetHatcheriesValueModifiers getHatcheriesModifiersUC,
            UC_GetShippingValueModifiers getShippingModifiersUC,
            UC_GetResearchValueModifiers getResearchModifiersUC,
            UC_GetEvolutionValueModifiers getEvolutionModifiersUC
        )
        {
            _buyButtonApplicable = getMainBuyButtonModifiersUC.Execute();
            _hatcheriesApplicable = getHatcheriesModifiersUC.Execute();
            _shippingApplicable = getShippingModifiersUC.Execute();
            _researchApplicable = getResearchModifiersUC.Execute();
            _evolutionApplicable = getEvolutionModifiersUC.Execute();
            
            InitEntity(entity, data, config);
            
            ProjectContext.Instance.Container
                .Resolve<ResearchMediator>()
                .Initialize(
                    entity,
                    config,
                    mainTopBarEntity,
                    subtractCurrencyCommand,
                    getResearchModifiersUC
            );
        }
        
        private void InitEntity(
            ResearchEntity entity,
            ResearchUserData data,
            ResearchTitleData config
        )
        {
            for (int i = 0; i < config.BonusTypesConfiguration.Count; i++)
            {
                var bonusConfig = config.BonusTypesConfiguration[i];
                var bonusState = FindBonusStateByType(data, bonusConfig.Type);

                BonusEntity bonus = null;
                switch (bonusConfig.Type)
                {
                    case BonusType.BUY_BUTTON_RATE_MULTIPLIER:
                        bonus = new MultiplierBonusEntity();
                        bonus.ApplicableBonusEntity = _buyButtonApplicable;
                        break;
                    case BonusType.EGG_VALUE_MULTIPLIER:
                        bonus = new MultiplierBonusEntity();
                        bonus.ApplicableBonusEntity = _evolutionApplicable;
                        break;
                    case BonusType.EGG_LAYING_RATE_MULTIPLIER:
                        bonus = new MultiplierBonusEntity();
                        bonus.ApplicableBonusEntity = _hatcheriesApplicable;
                        break;
                    case BonusType.HATCHERY_CAPACITY_INCREMENT:
                        bonus = new IncrementBonusEntity();
                        bonus.ApplicableBonusEntity = _hatcheriesApplicable;
                        break;
                    case BonusType.VEHICLE_SHIPPING_RATE_INCREMENT:
                        bonus = new IncrementBonusEntity();
                        bonus.ApplicableBonusEntity = _shippingApplicable;
                        break;
                    case BonusType.RESEARCH_DISCOUNT:
                        bonus = new DiscountBonusEntity();
                        bonus.ApplicableBonusEntity = _researchApplicable;
                        break;
                    case BonusType.HATCHERY_DISCOUNT:
                        bonus = new DiscountBonusEntity();
                        bonus.ApplicableBonusEntity = _hatcheriesApplicable;
                        break;
                    case BonusType.VEHICLE_DISCOUNT:
                        bonus = new DiscountBonusEntity();
                        bonus.ApplicableBonusEntity = _shippingApplicable;
                        break;
                    case BonusType.PIGEON_DISCOUNT:
                        bonus = new DiscountBonusEntity();
                        bonus.ApplicableBonusEntity = _buyButtonApplicable;
                        break;
                    default:
                        throw new Exception("No bonus found of type: " + bonusConfig.Type);
                }

                if (bonus != null)
                {
                    bonus.Id = i;
                    bonus.Type = bonusConfig.Type;
                    bonus.UnitType = bonusConfig.UnitType;
                    bonus.Name.Value = bonusConfig.Name;
                    bonus.Icon.Value = bonusConfig.Icon;
                    bonus.Tier.Value = bonusState.CurrentTier;
                    bonus.CurrentValue.Value = bonusConfig.Tiers[bonus.Tier.Value].Value;

                    if (bonus.Tier.Value < bonusConfig.Tiers.Count - 1)
                    {
                        bonus.NextValue.Value = bonusConfig.Tiers[bonus.Tier.Value + 1].Value;
                        bonus.NextCost.Value = bonusConfig.Tiers[bonus.Tier.Value + 1].Cost;
                    }
                
                    entity.Bonuses.Add(bonus);
                }
            }
        }
        
        private BonusState FindBonusStateByType(ResearchUserData data, BonusType type)
        {
            foreach (var bonus in data.Bonuses)
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