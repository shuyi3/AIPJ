using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_DS1_175 : SimTemplate //timberwolf
	{

//    eure anderen wildtiere haben +1 angriff.
        public override void onAuraStarts(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnTimberWolfs++;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PET && m.entitiyID != own.entitiyID) p.minionGetBuffed(m, 1, 0);
                }
            }
            else
            {
                p.playerSecond.anzOwnTimberWolfs++;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PET && m.entitiyID != own.entitiyID) p.minionGetBuffed(m, 1, 0);
                }
            }

        }

        public override void onAuraEnds(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnTimberWolfs--;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PET && m.entitiyID != own.entitiyID) p.minionGetBuffed(m, -1, 0);
                }
            }
            else
            {
                p.playerSecond.anzOwnTimberWolfs--;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PET && m.entitiyID != own.entitiyID) p.minionGetBuffed(m, -1, 0);
                }
            }
        }

	}
}