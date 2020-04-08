using System.Reflection;
using PigeonCorp.Persistence.TitleData;

namespace PigeonCorp.Persistence.Gateway
{
    public class ScriptableObjectGateway : ITitleDataGateway
    {
        private readonly TitleDataHolder _titleDataHolder;

        public ScriptableObjectGateway(TitleDataHolder titleDataHolder)
        {
            _titleDataHolder = titleDataHolder;
        }
        
        public T Get<T>()
        {
            var requiredField = default(T);
            
            var titleDataFields = typeof(TitleDataHolder).GetFields();
            foreach(FieldInfo field in titleDataFields)
            {
                if (field.FieldType == typeof(T))
                {
                    var rawObject = field.GetValue(_titleDataHolder);
                    requiredField = (T)rawObject;
                    break;
                }
            }

            return requiredField;
        }
    }
}