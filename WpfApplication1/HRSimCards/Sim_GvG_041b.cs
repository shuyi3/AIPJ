using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_041b : SimTemplate //* Dark Wispers
    {
        //   Summon 5 Wisps;

        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.CS2_231);
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            for (int i = 0; i < 5; i++)
            {
                int posi = (ownplay) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
                p.callKid(kid, posi, ownplay);
            }
        }
    }
}