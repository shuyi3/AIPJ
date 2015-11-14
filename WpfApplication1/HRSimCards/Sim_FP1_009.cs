using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_FP1_009 : SimTemplate //deathlord
	{
        CardDB.Card c = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_017);//nerubian
//    spott. todesröcheln:/ euer gegner legt einen diener aus seinem deck auf das schlachtfeld.
        public override void onDeathrattle(Playfield p, Minion m)
        {
            int place = (m.own) ? p.playerSecond.ownMinions.Count : p.playerFirst.ownMinions.Count;
            p.callKid(c, place, !m.own);
        }

	}
}