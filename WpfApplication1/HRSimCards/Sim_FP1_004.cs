using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_FP1_004 : SimTemplate//* Mad Scientist //TODO: rework
    {
        // Deathrattle: Put a Secret: from your deck into the battlefield.

        public override void onDeathrattle(Playfield p, Minion m)
        {
            if (m.own)
            {
                CardDB.cardIDEnum secToDraw = p.getArandomSecretFromDeck(m.own);
                if (secToDraw != CardDB.cardIDEnum.None)
                {
                    p.playerFirst.ownSecretsIDList.Add(secToDraw);
                    GraveYardItem gyi = new GraveYardItem(secToDraw, p.getNextEntity(), m.own);
                    p.graveYard.Add(gyi);
                }
            }
            else
            {
                CardDB.cardIDEnum secToDraw = p.getArandomSecretFromDeck(m.own);
                if (secToDraw != CardDB.cardIDEnum.None)
                {
                    p.playerSecond.ownSecretsIDList.Add(secToDraw);
                    GraveYardItem gyi = new GraveYardItem(secToDraw, p.getNextEntity(), m.own);
                    p.graveYard.Add(gyi);
                }
            }
        }

        //public override void onDeathrattle(Playfield p, Minion m)
        //{
        //    if (m.own)
        //    {
        //        if (p.playerFirst.ownHeroStartClass == TAG_CLASS.MAGE)
        //        {
        //            p.playerFirst.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_289);
        //        }
        //        if (p.playerFirst.ownHeroStartClass == TAG_CLASS.HUNTER)
        //        {
        //            p.playerFirst.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_554);
        //        }
        //        if (p.playerFirst.ownHeroStartClass == TAG_CLASS.PALADIN)
        //        {
        //            p.playerFirst.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_130);
        //        }
        //    }
        //    else
        //    {
        //        //if (p.playerSecond.ownHeroStartClass == TAG_CLASS.MAGE || p.playerSecond.ownHeroStartClass == TAG_CLASS.HUNTER || p.playerSecond.ownHeroStartClass == TAG_CLASS.PALADIN)
        //        //{
        //        //    if (p.playerSecond.ownSecretsIDList.Count <= 4)
        //        //    {
        //        //        p.playerSecond.ownSecretsIDList.Count++;
        //        //        SecretItem si = Probabilitymaker.Instance.getNewSecretGuessedItem(p.getNextEntity(), p.playerFirst.ownHeroStartClass);
        //        //        if (p.playerSecond.ownHeroStartClass == TAG_CLASS.PALADIN)
        //        //        {
        //        //            si.canBe_redemption = false;
        //        //        }
        //        //        //if (Settings.Instance.useSecretsPlayArround)
        //        //        //{
        //        //        //    p.playerSecond.ownSecretList.Add(si);
        //        //        //}
        //        //    }
        //        //}
        //        if (p.playerSecond.ownHeroStartClass == TAG_CLASS.MAGE)
        //        {
        //            p.playerSecond.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_289);
        //        }
        //        if (p.playerSecond.ownHeroStartClass == TAG_CLASS.HUNTER)
        //        {
        //            p.playerSecond.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_554);
        //        }
        //        if (p.playerSecond.ownHeroStartClass == TAG_CLASS.PALADIN)
        //        {
        //            p.playerSecond.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_130);
        //        }
        //    }
        //}
    }

}
