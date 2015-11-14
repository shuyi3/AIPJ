using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_029 : SimTemplate //millhousemanastorm
	{

//    kampfschrei:/ im nächsten zug kosten zauber für euren gegner (0) mana.
        //todo implement the nomanacosts for the enemyturn
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            //add #enemy millhouse played by 1
            if (own.own) //playerFirst
            {
                p.playerSecond.enemyMillhouse++;
                foreach (Handmanager.Handcard h in p.playerSecond.owncards)
                {
                    if (h.card.type == CardDB.cardtype.SPELL)
                        h.manacost = 0;
                }
            }
            else
            {
                p.playerFirst.enemyMillhouse++;
                foreach (Handmanager.Handcard h in p.playerFirst.owncards)
                {
                    if (h.card.type == CardDB.cardtype.SPELL)
                        h.manacost = 0;
                }
            }
        }


	}
}