using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_284 : SimTemplate //azuredrake
	{

//    zauberschaden +1/. kampfschrei:/ zieht eine karte.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            p.drawACard(p.getArandomCardFromDeck(own.own), own.own);
		}

        public override void onAuraStarts(Playfield p, Minion m)
        {
            if (m.own)
            {
                p.playerFirst.spellpower++;
            }
            else
            {
                p.playerSecond.spellpower++;
            }
        }

        public override void onAuraEnds(Playfield p, Minion m)
        {
            if (m.own)
            {
                p.playerFirst.spellpower--;
            }
            else
            {
                p.playerSecond.spellpower--;
            }
        }


	}
}