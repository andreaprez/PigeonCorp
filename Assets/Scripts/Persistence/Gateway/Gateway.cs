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

        #region UserData
        
        public void UpdateUserStateData(UserStateUserData data)
        {
            UserDataGateway.Update(data);
        }

        public UserStateUserData GetUserStateData()
        {
            return UserDataGateway.Get<UserStateUserData>();
        }
        
        #endregion

        
        #region TitleData

        public MainBuyButtonTitleData GetMainBuyButtonConfig()
        {
            return TitleDataGateway.Get<MainBuyButtonTitleData>();
        }
        
        #endregion

    }
}