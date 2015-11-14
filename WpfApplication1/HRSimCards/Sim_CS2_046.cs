using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_046 : SimTemplate //bloodlust
	{

//    verleiht euren dienern +3 angriff in diesem zug.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            List<Minion> temp = (ownplay) ? p.playerFirst.ownMinions: p.playerSecond.ownMinions;
            foreach (Minion m in temp)
            {
                p.minionGetTempBuff(m, 3, 0);
            }
		}

	}
}