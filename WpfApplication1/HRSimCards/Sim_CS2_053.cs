using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS2_053 : SimTemplate//far sight
    {

        //todo: bonus for it?
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.drawACard(CardDB.cardName.unknown, ownplay);
        }

    }
}
