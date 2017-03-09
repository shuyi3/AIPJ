using System.Collections.Generic;

namespace HRSim
{

    public abstract class Behavior
    {

        public virtual List<int> getPlayfieldFeature(Playfield p, bool own)
        {
            return null;
        }

        public virtual float getPlayfieldValue(Playfield p, bool own)
        {
            return 0;
        }

        public virtual int getEnemyMinionValue(Minion m, Playfield p)
        {
            return 0;
        }

    }

}