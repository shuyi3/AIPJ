using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_FP1_030 : SimTemplate //loatheb
	{
//    kampfschrei:/ im nächsten zug kosten zauber für euren gegner (5) mehr.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            //add #enemy loatheb played by 1
            if (own.own) //playerFirst
            {
                p.playerSecond.enemyLoatheb++;
                foreach (Handmanager.Handcard h in p.playerSecond.owncards){
                    if (h.card.type == CardDB.cardtype.SPELL)
                        h.manacost += 5;
                }
            }
            else {
                p.playerFirst.enemyLoatheb++;
                foreach (Handmanager.Handcard h in p.playerFirst.owncards)
                {
                    if (h.card.type == CardDB.cardtype.SPELL)
                        h.manacost += 5;
                }
            }
		}
	}
}