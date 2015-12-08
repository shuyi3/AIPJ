using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_004 : SimTemplate //vanish
	{

//    lasst alle diener auf die hand ihrer besitzer zurückkehren.
        //todo clear playfield
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.playerFirst.anzOwnRaidleader = 0;
            p.playerFirst.anzOwnStormwindChamps = 0;
            p.playerFirst.anzOwnTundrarhino = 0;
            p.playerFirst.anzOwnTimberWolfs = 0;
            p.playerFirst.anzMurlocWarleader = 0;
            p.playerFirst.anzGrimscaleOracle = 0;
            p.playerFirst.anzOwnAuchenaiSoulpriest = 0;
            p.playerFirst.anzOwnsorcerersapprentice = 0;
            //p.playerFirst.anzOwnsorcerersapprenticeStarted = 0;
            p.playerFirst.anzOwnSouthseacaptain = 0;
            p.playerFirst.doublepriest = 0;
            p.playerFirst.ownBaronRivendare = 0;
            p.playerFirst.anzOwnChromaggus = 0;

            p.playerFirst.spellpower = 0;

            p.playerFirst.winzigebeschwoererin = 0;
            p.playerFirst.managespenst = 0;
            p.playerFirst.soeldnerDerVenture = 0;
            p.playerFirst.beschwoerungsportal = 0;
            p.playerFirst.nerubarweblord = 0;

            //second
            p.playerSecond.anzOwnRaidleader = 0;
            p.playerSecond.anzOwnStormwindChamps = 0;
            p.playerSecond.anzOwnTundrarhino = 0;
            p.playerSecond.anzOwnTimberWolfs = 0;
            p.playerSecond.anzMurlocWarleader = 0;
            p.playerSecond.anzGrimscaleOracle = 0;
            p.playerSecond.anzOwnAuchenaiSoulpriest = 0;
            p.playerSecond.anzOwnsorcerersapprentice = 0;
            //p.playerSecond.anzOwnsorcerersapprenticeStarted = 0;
            p.playerSecond.anzOwnSouthseacaptain = 0;
            p.playerSecond.doublepriest = 0;
            p.playerSecond.ownBaronRivendare = 0;
            p.playerSecond.anzOwnChromaggus = 0;

            p.playerSecond.spellpower = 0;

            p.playerSecond.winzigebeschwoererin = 0;
            p.playerSecond.managespenst = 0;
            p.playerSecond.soeldnerDerVenture = 0;
            p.playerSecond.beschwoerungsportal = 0;
            p.playerSecond.nerubarweblord = 0;

            foreach (Minion m in p.playerFirst.ownMinions)
            {
                p.drawACard(m.name, true, true);
            }

            foreach (Minion m in p.playerSecond.ownMinions)
            {
                p.drawACard(m.name, false, true);
            }

            p.playerFirst.ownMinions.Clear();
            p.playerSecond.ownMinions.Clear();

        }

	}
}