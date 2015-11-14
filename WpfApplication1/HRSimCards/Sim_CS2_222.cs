using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_222 : SimTemplate //stormwindchampion
	{

//    eure anderen diener haben +1/+1.
        public override void onAuraStarts(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnStormwindChamps++;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID) p.minionGetBuffed(m, 1, 1);
                }
            }
            else
            {
                p.playerSecond.anzOwnStormwindChamps++;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID) p.minionGetBuffed(m, 1, 1);
                }
            }

        }

        public override void onAuraEnds(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnStormwindChamps--;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID) p.minionGetBuffed(m, -1, -1);
                }
            }
            else
            {
                p.playerSecond.anzOwnStormwindChamps--;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID) p.minionGetBuffed(m, -1, -1);
                }
            }
        }

	}
}