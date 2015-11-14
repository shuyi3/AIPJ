using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_027 : SimTemplate //southseacaptain
	{

//    eure anderen piraten haben +1/+1.
        public override void onAuraStarts(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnSouthseacaptain++;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if((TAG_RACE)m.handcard.card.race == TAG_RACE.PIRATE && own.entitiyID != m.entitiyID) p.minionGetBuffed(m, 1, 1);
                }
            }
            else
            {
                p.playerSecond.anzOwnSouthseacaptain++;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PIRATE && own.entitiyID != m.entitiyID) p.minionGetBuffed(m, 1, 1);
                }
            }

        }

        public override void onAuraEnds(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnSouthseacaptain--;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PIRATE && own.entitiyID != m.entitiyID) p.minionGetBuffed(m, -1, -1);
                }
            }
            else
            {
                p.playerSecond.anzOwnSouthseacaptain--;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PIRATE && own.entitiyID != m.entitiyID) p.minionGetBuffed(m, -1, -1);
                }
            }
        }


	}
}