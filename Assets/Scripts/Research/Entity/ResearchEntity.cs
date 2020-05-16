using System.Collections.Generic;

namespace PigeonCorp.Research.Entity
{
    public class ResearchEntity
    {
        public readonly List<BonusEntity> Bonuses;

        public ResearchEntity()
        {
            Bonuses = new List<BonusEntity>();
        }
    }
}