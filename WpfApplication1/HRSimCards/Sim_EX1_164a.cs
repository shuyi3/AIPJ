using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_164a : SimTemplate //* nourish
    {
        //    Gain 2 Mana Crystals
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            if (ownplay)
            {
				p.playerFirst.mana = Math.Min(10, p.playerFirst.mana+2);
				p.playerFirst.ownMaxMana = Math.Min(10, p.playerFirst.ownMaxMana+2);
            }
            else
            {
				p.playerFirst.mana = Math.Min(10, p.playerFirst.mana+2);
				p.playerSecond.ownMaxMana = Math.Min(10, p.playerSecond.ownMaxMana+2);
            }
        }

    }

}