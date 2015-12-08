using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_003 : SimTemplate //Unstable Portal
    {

        //    Add a random minion to your hand. It costs (3) less.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            if (p.drawACard(CardDB.Instance.getRandomCollectibleCardName(), ownplay, true) == -1)
                return;
            if (ownplay)
            {
                p.playerFirst.owncards[p.playerFirst.owncards.Count - 1].manacost -= 3;
                p.playerFirst.owncards[p.playerFirst.owncards.Count - 1].permCostChange -= 3;
            }
            else {
                p.playerSecond.owncards[p.playerSecond.owncards.Count - 1].manacost -= 3;
                p.playerSecond.owncards[p.playerSecond.owncards.Count - 1].permCostChange -= 3;
            }
        }

    }

}