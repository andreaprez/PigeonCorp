using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Persistence.UserData;

namespace PigeonCorp.Persistence.Gateway
{
    public class Gateway
    {
        private ITitleDataGateway TitleDataGateway;
        private IUserDataGateway UserDataGateway;
        private static Gateway _instance;
   
        public static Gateway Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Gateway();
                }

                return _instance;
            }
        }
        
        public void Initialize(ITitleDataGateway titleDataGateway, IUserDataGateway userDataGateway)
        {
            TitleDataGateway = titleDataGateway;
            UserDataGateway = userDataGateway;
        }

                
        #region TitleData

        public PigeonTitleData GetPigeonConfig()
        {
            return TitleDataGateway.Get<PigeonTitleData>();
        }

        public UserStateTitleData GetUserStateConfig()
        {
            return TitleDataGateway.Get<UserStateTitleData>();
        }
        
        public HatcheriesTitleData GetHatcheriesConfig()
        {
            return TitleDataGateway.Get<HatcheriesTitleData>();
        }
        
        public ShippingTitleData GetShippingConfig()
        {
            return TitleDataGateway.Get<ShippingTitleData>();
        }
        
        public ResearchTitleData GetResearchConfig()
        {
            return TitleDataGateway.Get<ResearchTitleData>();
        }
        
        public EvolutionTitleData GetEvolutionConfig()
        {
            return TitleDataGateway.Get<EvolutionTitleData>();
        }
        
        #endregion
        
        
        #region UserData
        
        public UserInitializedUserData GetUserInitialized()
        {
            return UserDataGateway.Get<UserInitializedUserData>();
        }
        
        public void UpdateUserInitialized(UserInitializedUserData data)
        {
            UserDataGateway.Update(data);
        }
        
        public UserStateUserData GetUserStateData()
        {
            return UserDataGateway.Get<UserStateUserData>();
        }
        
        public void UpdateUserStateData(UserStateUserData data)
        {
            UserDataGateway.Update(data);
        }
        
        public HatcheriesUserData GetHatcheriesData()
        {
            return UserDataGateway.Get<HatcheriesUserData>();
        }
        
        public void UpdateHatcheriesData(HatcheriesUserData data)
        {
            UserDataGateway.Update(data);
        }
        
        public ShippingUserData GetShippingData()
        {
            return UserDataGateway.Get<ShippingUserData>();
        }
        
        public void UpdateShippingData(ShippingUserData data)
        {
            UserDataGateway.Update(data);
        }
        
        public ResearchUserData GetResearchData()
        {
            return UserDataGateway.Get<ResearchUserData>();
        }
        
        public void UpdateResearchData(ResearchUserData data)
        {
            UserDataGateway.Update(data);
        }
        
        public EvolutionUserData GetEvolutionData()
        {
            return UserDataGateway.Get<EvolutionUserData>();
        }
        
        public void UpdateEvolutionData(EvolutionUserData data)
        {
            UserDataGateway.Update(data);
        }

        #endregion
    }
}