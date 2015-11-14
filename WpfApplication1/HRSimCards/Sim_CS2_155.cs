using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_155 : SimTemplate //archmage
	{

//    zauberschaden +1/
        public override void onAuraStarts(Playfield p, Minion own)
		{
            if (own.own)
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