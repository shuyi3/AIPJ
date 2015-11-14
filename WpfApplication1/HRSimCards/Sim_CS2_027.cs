using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_027 : SimTemplate //mirrorimage
	{
        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.CS2_mirror);
//    ruft zwei diener (0/2) mit spott/ herbei.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int posi = (ownplay) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
            
            p.callKid(kid, posi, ownplay);
            p.callKid(kid, posi, ownplay);
		}

	}
}