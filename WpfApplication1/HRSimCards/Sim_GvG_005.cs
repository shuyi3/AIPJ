using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_005 : SimTemplate //Echo of Medivh
    {

        //    Put a copy of each friendly minion into your hand.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            // optimistic
            List<Minion> temp = (ownplay) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;

            foreach (Minion m in temp)
            {
                p.drawACard(m.handcard.card.name, ownplay, true);
            }
            
        }


    }

}