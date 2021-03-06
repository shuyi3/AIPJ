﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
    class Sim_FP1_004 : SimTemplate//* Mad Scientist
    {
        // Deathrattle: Put a Secret: from your deck into the battlefield.

        public override void onDeathrattle(Playfield p, Minion m)
        {
            if (m.own)
            {
                if (p.ownHeroStartClass == TAG_CLASS.MAGE)
                {
                    p.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_289);
                }
                if (p.ownHeroStartClass == TAG_CLASS.HUNTER)
                {
                    p.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_554);
                }
                if (p.ownHeroStartClass == TAG_CLASS.PALADIN)
                {
                    p.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_130);
                }
            }
            else
            {
                if (p.enemyHeroStartClass == TAG_CLASS.MAGE || p.enemyHeroStartClass == TAG_CLASS.HUNTER || p.enemyHeroStartClass == TAG_CLASS.PALADIN)
                {
                    if (p.enemySecretCount <= 4)
                    {
                        p.enemySecretCount++;
                        SecretItem si = p.sf.Probabilitymaker.getNewSecretGuessedItem(p.getNextEntity(), p.ownHeroStartClass);
                        if (p.enemyHeroStartClass == TAG_CLASS.PALADIN)
                        {
                            si.canBe_redemption = false;
                        }
                        if (p.sf.Settings.useSecretsPlayArround)
                        {
                            p.enemySecretList.Add(si);
                        }
                    }
                }
            }
        }
    }

}
