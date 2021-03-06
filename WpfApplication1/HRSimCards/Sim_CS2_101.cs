using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_101 : SimTemplate //reinforce
	{

        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.CS2_101t);//silverhandrecruit

//    heldenfähigkeit/\nruft einen rekruten der silbernen hand (1/1) herbei.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int posi = (ownplay) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;

            p.callKid(kid, posi, ownplay);
        }

	}
}