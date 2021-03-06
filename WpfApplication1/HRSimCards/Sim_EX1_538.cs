using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_538 : SimTemplate //unleashthehounds
	{

//    ruft für jeden feindlichen diener einen jagdhund (1/1) mit ansturm/ herbei.

        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_538t);//hound

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int anz = p.playerSecond.ownMinions.Count;
            int posi = p.playerFirst.ownMinions.Count;
            
            for (int i = 0; i < anz; i++)
            {
                p.callKid(kid, posi, ownplay);
            }
		}

	}
}