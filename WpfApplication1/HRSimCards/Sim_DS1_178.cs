using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_DS1_178 : SimTemplate //tundrarhino
	{

//    eure wildtiere haben ansturm/.
        //todo charge?
        public override void onAuraStarts(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnTundrarhino++;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PET) p.minionGetCharge(m);
                }
            }
            else
            {
                p.playerSecond.anzOwnTundrarhino++;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PET) p.minionGetCharge(m);
                }
            }

        }

        public override void onAuraEnds(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnTundrarhino--;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PET) p.minionLostCharge(m);
                }
            }
            else
            {
                p.playerSecond.anzOwnTundrarhino--;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PET) p.minionLostCharge(m);
                }
            }
        }

	}
}