using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_573b : SimTemplate //shandoslesson
	{

//    ruft zwei treants (2/2) mit spott/ herbei.
        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_573t); //special treant
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int pos = (ownplay) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
            p.callKid(kid, pos, ownplay, true);
            p.callKid(kid, pos, ownplay, true);
		}

	}
}