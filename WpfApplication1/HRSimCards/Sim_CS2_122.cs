using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_122 : SimTemplate //raidleader
	{

//    eure anderen diener haben +1 angriff.
        public override void onAuraStarts(Playfield p, Minion own)
		{
            if (own.own)
            {
                p.playerFirst.anzOwnRaidleader++;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID) p.minionGetBuffed(m, 1, 0);
                }
            }
            else
            {
                p.playerSecond.anzOwnRaidleader++;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID) p.minionGetBuffed(m, 1, 0);
                }
            }
            
		}

        public override void onAuraEnds(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnRaidleader--;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID) p.minionGetBuffed(m, -1, 0);
                }
            }
            else
            {
                p.playerSecond.anzOwnRaidleader--;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID) p.minionGetBuffed(m, -1, 0);
                }
            }
        }

	}
}