using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_085 : SimTemplate //mindcontroltech
	{
        //todo list
//    kampfschrei:/ falls euer gegner mind. 4 diener hat, übernehmt zufällig die kontrolle über einen davon.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            if (own.own)
            {
                if (p.playerSecond.ownMinions.Count >= 4)
                {
                    List<Minion> temp = new List<Minion>(p.playerSecond.ownMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//we take the weekest
                    Minion targett;
                    targett = temp[0];
                    if (targett.taunt && temp.Count >= 2 && !temp[1].taunt) targett = temp[1];
                    p.minionGetControlled(targett, true, false);

                }
            }
            else
            {
                if (p.playerFirst.ownMinions.Count >= 4)
                {
                    List<Minion> temp = new List<Minion>(p.playerFirst.ownMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//we take the weekest
                    Minion targett;
                    targett = temp[0];
                    if (targett.taunt && temp.Count >= 2 && !temp[1].taunt) targett = temp[1];
                    p.minionGetControlled(targett, false, false);

                }
            }
		}

	}

}