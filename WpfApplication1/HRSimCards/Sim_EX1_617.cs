using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_617 : SimTemplate //deadlyshot
	{
//    vernichtet einen zufälligen feindlichen diener.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            List<Minion> temp2 = (ownplay) ? new List<Minion>(p.playerSecond.ownMinions) : new List<Minion>(p.playerFirst.ownMinions);
            temp2.Sort((a, b) => a.Angr.CompareTo(b.Angr));
            foreach (Minion enemy in temp2)
            {
                p.minionGetDestroyed(enemy);
                break;
            }
		}

	}
}