using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_350 : SimTemplate //prophetvelen
	{

//    verdoppelt den schaden und die heilung eurer zauber und heldenfähigkeiten.
		public override void onAuraStarts(Playfield p, Minion own)
		{
            if (own.own)
            {
                p.playerFirst.doublepriest++;
            }
		}

        public override void onAuraEnds(Playfield p, Minion m)
        {
            if (m.own)
            {
                p.playerFirst.doublepriest--;
            }
        }

	}
}