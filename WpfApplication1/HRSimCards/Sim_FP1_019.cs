using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_FP1_019 : SimTemplate //poisonseeds
	{
        CardDB.Card d = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_158t);
//    vernichtet alle diener und ruft für jeden einen treant (2/2) als ersatz herbei.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int ownanz = p.playerFirst.ownMinions.Count;
            int enemanz = p.playerSecond.ownMinions.Count;
            p.allMinionsGetDestroyed();
            for (int i = 0; i < ownanz; i++)
            {
                p.callKid(d, 1, true);
            }
            for (int i = 0; i < enemanz; i++)
            {
                p.callKid(d, 1, false);
            }
		}

	}
}