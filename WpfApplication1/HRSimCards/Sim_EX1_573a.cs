using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_573a : SimTemplate //demigodsfavor
	{

//    verleiht euren anderen dienern +2/+2.


		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            List<Minion> temp = (ownplay) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            foreach (Minion m in temp)
            {
                p.minionGetBuffed(m, 2, 2);
            }
		}

	}
}