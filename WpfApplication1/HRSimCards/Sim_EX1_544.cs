using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_544 : SimTemplate //flare
    {

        //    alle diener verlieren verstohlenheit/. zerstört alle feindlichen geheimnisse/. zieht eine karte.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            foreach (Minion m in p.playerFirst.ownMinions)
            {
                m.stealth = false;
            }
            foreach (Minion m in p.playerSecond.ownMinions)
            {
                m.stealth = false;
            }
            if (ownplay)
            {
                p.playerSecond.ownSecretsIDList.Clear();
            }
            else
            {
                p.playerFirst.ownSecretsIDList.Clear();
            }
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
        }

    }

}