using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_158 : SimTemplate //souloftheforest
	{

//    verleiht euren dienern „todesröcheln:/ ruft einen treant (2/2) herbei.“

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            List<Minion> temp = (ownplay) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;

            foreach (Minion m in temp)
            {
                m.souloftheforest++;
            }
		}

	}
}