using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_248 : SimTemplate //feralspirit
	{
        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_tk11);//spiritwolf
//    ruft zwei geisterwölfe (2/3) mit spott/ herbei. überladung:/ (2)

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int posi = (ownplay) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;

            p.callKid(kid, posi, ownplay);
            p.callKid(kid, posi, ownplay);
            if (ownplay) p.ueberladung += 2;
		}

	}
}