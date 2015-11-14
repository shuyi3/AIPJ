using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_103 : SimTemplate//* Coldlight Seer
	// Battlecry:: Give ALL other Murlocs +2 Health.
	
    {
        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            foreach (Minion m in p.playerFirst.ownMinions)
            {
                if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MURLOC && own.entitiyID != m.entitiyID) p.minionGetBuffed(m, 0, 2);
            }
            foreach (Minion m in p.playerSecond.ownMinions)
            {
                if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MURLOC && own.entitiyID != m.entitiyID) p.minionGetBuffed(m, 0, 2);
            }
        }
    }
}
