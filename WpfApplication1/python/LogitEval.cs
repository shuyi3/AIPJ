using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using System.Diagnostics;

namespace HRSim
{
    class LogitEval
    {
        dynamic model = null;
        dynamic np = null;
        dynamic logit_eval = null;
        dynamic py_utils = null;
        dynamic evaluator = null;
        dynamic encoder = null;
        HashSet<int> featureSparseSet = new HashSet<int>();
        private Behavior bh;
        int offset = 0;

        static string[] minionCardArray = {"damagedgolem", "clockworkgnome", "boombot", "manawyrm", "cogmaster", "annoyotron", "mechwarper",
               "snowchugger", "harvestgolem", "spidertank", "tinkertowntechnician", "mechanicalyeti",
               "goblinblastmage", "loatheb", "archmageantonidas", "drboom", "unknown"};

        static string[] cardArray = {"damagedgolem", "clockworkgnome", "boombot", "manawyrm", "cogmaster", "annoyotron", "mechwarper",
               "snowchugger", "harvestgolem", "spidertank", "tinkertowntechnician", "mechanicalyeti",
               "goblinblastmage", "loatheb", "archmageantonidas", "drboom", "unknown",
                "thecoin", "armorplating", "fireblast", "frostbolt", "fireball", "flamestrike"};
        
        static Dictionary<CardDB.cardName, int> cardIdxDict = new Dictionary<CardDB.cardName,int>()
                                                               {
                                                                {CardDB.cardName.damagedgolem, 0},                                                              
                                                                {CardDB.cardName.clockworkgnome, 1},
                                                                {CardDB.cardName.boombot, 2},
                                                                {CardDB.cardName.manawyrm, 3},
                                                                {CardDB.cardName.cogmaster, 4},
                                                                {CardDB.cardName.annoyotron, 5},
                                                                {CardDB.cardName.mechwarper, 6},
                                                                {CardDB.cardName.snowchugger, 7},
                                                                {CardDB.cardName.harvestgolem, 8},
                                                                {CardDB.cardName.spidertank, 9},
                                                                {CardDB.cardName.tinkertowntechnician, 10},
                                                                {CardDB.cardName.mechanicalyeti, 11},
                                                                {CardDB.cardName.goblinblastmage, 12},
                                                                {CardDB.cardName.loatheb, 13},
                                                                {CardDB.cardName.archmageantonidas, 14},
                                                                {CardDB.cardName.drboom, 15},
                                                                {CardDB.cardName.unknown, 16},
                                                                {CardDB.cardName.thecoin, 17},
                                                                {CardDB.cardName.armorplating, 18},
                                                                {CardDB.cardName.fireblast, 19},
                                                                {CardDB.cardName.frostbolt, 20},
                                                                {CardDB.cardName.fireball, 21},
                                                                {CardDB.cardName.flamestrike, 22}
                                                               };

        private static LogitEval instance;

        private LogitEval()
        {
            Init();
        }

        public static LogitEval Instance
        {
            get
            {
                return instance ?? (instance = new LogitEval());
            }
        }

        public void Init()
        {
            string logit_weight_file = @"C:\Code\dqn-master\simple_dqn\endturn_ft_weight\weights_back_1.hdf5";
            bh = new BehaviorControl();

            using (Py.GIL())
            {
                PythonEngine.Initialize();
                dynamic sys = PythonEngine.ImportModule("sys");
                np = Py.Import("numpy");
                logit_eval = Py.Import("simple_dqn.logit_eval");
                py_utils = Py.Import("simple_dqn.py_utils");
                encoder = Py.Import("simple_dqn.encoder");
                evaluator = logit_eval.LogitEval(logit_weight_file);
            }
        }

        public dynamic Train()
        {
            dynamic res = evaluator.train_model();
            return res;
        }

        public PyList getHandFeature(Playfield p, bool own)
        {
            Player mPlayer, ePlayer;

            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            int[] featureArray = new int[cardArray.Length * 2];
            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                int idx = cardIdxDict[hc.card.name];
                featureArray[idx] = featureArray[idx] + 1;
            }

            foreach (Handmanager.Handcard hc in ePlayer.owncards)
            {
                int idx = cardIdxDict[hc.card.name] + cardArray.Length;
                featureArray[idx] = featureArray[idx] + 1;
            }

            PyList hand_feature = new PyList();

            foreach (int ft in featureArray)
            {
                PyInt num = new PyInt(ft);
                hand_feature.Append(num);
                num.Dispose();
            }

            return hand_feature;
        }

        public PyList getHandFeature(PlayerKeyInfo ownInfo, PlayerKeyInfo enemyInfo)
        {

            int[] featureArray = new int[cardArray.Length * 2];
            foreach (PlayerKeyInfo.CardKeyInfo hc in ownInfo.handcardJsonList)
            {
                int idx = cardIdxDict[CardDB.Instance.cardNamestringToEnum(hc.cardName)];
                featureArray[idx] = featureArray[idx] + 1;
            }

            foreach (PlayerKeyInfo.CardKeyInfo hc in enemyInfo.handcardJsonList)
            {
                int idx = cardIdxDict[CardDB.Instance.cardNamestringToEnum(hc.cardName)] + cardArray.Length;
                featureArray[idx] = featureArray[idx] + 1;
            }

            PyList hand_feature = new PyList();

            foreach (int ft in featureArray)
            { 
                PyInt num = new PyInt(ft);
                hand_feature.Append(num);
                num.Dispose();
            }

            return hand_feature;
        }

        //public dynamic getPlayfieldFeature(Playfield p, bool own)
        //{
        //    PyList featureList = new PyList();

        //    Player mPlayer, ePlayer;

        //    if (own)
        //    {
        //        mPlayer = p.playerFirst;
        //        ePlayer = p.playerSecond;
        //    }
        //    else
        //    {
        //        mPlayer = p.playerSecond;
        //        ePlayer = p.playerFirst;
        //    }

        //    int hpboarder = 10;
        //    if (mPlayer.ownHeroName == HeroEnum.warlock && ePlayer.ownHeroName != HeroEnum.mage) hpboarder = 6;
        //    int aggroboarder = 11;

        //    if (p.isOwnTurn != own)
        //    {
        //        PyInt num = new PyInt(-p.lastTurnEvaluatePenality);
        //        featureList.Append(new PyInt(-p.lastTurnEvaluatePenality));
                
        //    }
        //    else
        //    {
        //        featureList.Append(new PyInt(-p.evaluatePenality));
        //    }

        //    featureList.Append(new PyInt(mPlayer.owncards.Count * 5));

        //    featureList.Append(new PyInt(mPlayer.ownMaxMana));
        //    featureList.Append(new PyInt(-ePlayer.ownMaxMana));

        //    featureList.Append(new PyInt(mPlayer.ownMaxMana * 20 - ePlayer.ownMaxMana * 20));

        //    featureList.Append(new PyInt(-2 * ePlayer.spellpower));

        //    int hpFeature = mPlayer.ownHero.Hp + mPlayer.ownHero.armor > hpboarder ? 1 : 0;

        //    if (mPlayer.ownHero.Hp + mPlayer.ownHero.armor > hpboarder)
        //    {
        //        featureList.Append(new PyInt(mPlayer.ownHero.Hp + mPlayer.ownHero.armor));
        //    }
        //    else
        //    {
        //        featureList.Append(new PyInt(-2 * (hpboarder + 1 - mPlayer.ownHero.Hp - mPlayer.ownHero.armor) * (hpboarder + 1 - mPlayer.ownHero.Hp - mPlayer.ownHero.armor)));
        //    }


        //    if (ePlayer.ownHero.Hp + ePlayer.ownHero.armor > aggroboarder)
        //    {
        //        featureList.Append(new PyInt(-ePlayer.ownHero.Hp - ePlayer.ownHero.armor));
        //    }
        //    else
        //    {
        //        featureList.Append(new PyInt(3 * (aggroboarder + 1 - ePlayer.ownHero.Hp - ePlayer.ownHero.armor)));
        //    }

        //    if (ePlayer.ownHero.armor + ePlayer.ownHero.Hp <= 10) //RR should be updated by lethal check
        //    {
        //        featureList.Append(new PyInt(mPlayer.owncarddraw * 100));
        //    }
        //    else
        //    {
        //        featureList.Append(new PyInt(0));
        //    }

        //    if (mPlayer.ownMaxMana < 4)
        //    {
        //        featureList.Append(new PyInt(mPlayer.owncarddraw * 2));
        //    }
        //    else
        //    {
        //        featureList.Append(new PyInt(mPlayer.owncarddraw * 5));
        //    }

        //    //retval += p.owncarddraw * 5;
        //    featureList.Append(new PyInt(-ePlayer.owncarddraw * 15));

        //    //int owntaunt = 0;
        //    int readycount = 0;
        //    int ownMinionsCount = 0;
        //    int ownMinionVal = 0;
        //    foreach (Minion m in mPlayer.ownMinions)
        //    {
        //        ownMinionVal += 5;
        //        ownMinionVal += m.Hp * 2;
        //        ownMinionVal += m.Angr * 2;
        //        ownMinionVal += m.handcard.card.rarity;
        //        if (!m.playedThisTurn && m.windfury) ownMinionVal += m.Angr;
        //        if (m.divineshild) ownMinionVal += 1;
        //        if (m.stealth) ownMinionVal += 1;
        //        if (m.handcard.card.isSpecialMinion)
        //        {
        //            ownMinionVal += 1;
        //            if (!m.taunt && m.stealth) ownMinionVal += 20;
        //        }
        //        else
        //        {
        //            if (m.Angr <= 2 && m.Hp <= 2 && !m.divineshild) ownMinionVal -= 5;
        //        }
        //        //if (!m.taunt && m.stealth && penman.specialMinions.ContainsKey(m.name)) retval += 20;
        //        //if (m.poisonous) retval += 1;
        //        if (m.divineshild && m.taunt) ownMinionVal += 4;
        //        //if (m.taunt && m.handcard.card.name == CardDB.cardName.frog) owntaunt++;
        //        //if (m.handcard.card.isToken && m.Angr <= 2 && m.Hp <= 2) ownMinionVal -= 5;
        //        //if (!penman.specialMinions.ContainsKey(m.name) && m.Angr <= 2 && m.Hp <= 2) ownMinionVal -= 5;
        //        if (m.handcard.card.name == CardDB.cardName.direwolfalpha || m.handcard.card.name == CardDB.cardName.flametonguetotem || m.handcard.card.name == CardDB.cardName.stormwindchampion || m.handcard.card.name == CardDB.cardName.raidleader) ownMinionVal += 10;
        //        if (m.handcard.card.name == CardDB.cardName.bloodmagethalnos) ownMinionVal += 10;
        //        if (m.handcard.card.name == CardDB.cardName.nerubianegg)
        //        {
        //            if (m.Angr >= 1) ownMinionVal += 2;
        //            if ((!m.taunt && m.Angr == 0) && (m.divineshild || m.maxHp > 2)) ownMinionVal -= 10;
        //        }
        //        if (m.Ready) readycount++;
        //        if (m.Hp <= 4 && (m.Angr > 2 || m.Hp > 3)) ownMinionsCount++;
        //    }
        //    featureList.Append(new PyInt(ownMinionVal));

        //    /*if (ePlayer.ownMinions.Count >= 0)
        //    {
        //        int anz = ePlayer.ownMinions.Count;
        //        if (owntaunt == 0) retval -= 10 * anz;
        //        retval += owntaunt * 10 - 11 * anz;
        //    }*/


        //    bool useAbili = false;
        //    bool usecoin = false;
        //    List<Action> actions;

        //    if (p.isOwnTurn != own)
        //    {
        //        actions = mPlayer.lastTurnActions;
        //    }
        //    else
        //    {
        //        actions = mPlayer.playactions;
        //    }

        //    int actionVal = 0;
        //    foreach (Action a in actions)
        //    {
        //        if (a.actionType == actionEnum.attackWithHero && ePlayer.ownHero.Hp <= 15) actionVal++; //attackfacehp = 15
        //        if (a.actionType == actionEnum.useHeroPower) useAbili = true;
        //        if (mPlayer.ownHeroName == HeroEnum.warrior && a.actionType == actionEnum.attackWithHero && useAbili) actionVal -= 1;
        //        //if (a.actionType == actionEnum.useHeroPower && a.card.card.name == CardDB.cardName.lesserheal && (!a.target.own)) actionVal -= 5;
        //        if (a.actionType != actionEnum.playcard) continue;
        //        if ((a.card.card.name == CardDB.cardName.thecoin || a.card.card.name == CardDB.cardName.innervate)) usecoin = true;
        //        //save spell for all classes: (except for rouge if he has no combo)
        //        if (a.target == null) continue;
        //        if (mPlayer.ownHeroName != HeroEnum.thief && a.card.card.type == CardDB.cardtype.SPELL && (!a.target.own && a.target.isHero) && a.card.card.name != CardDB.cardName.shieldblock) actionVal -= 11;
        //        if (mPlayer.ownHeroName == HeroEnum.thief && a.card.card.type == CardDB.cardtype.SPELL && (a.target.isHero && !a.target.own)) actionVal -= 11;
        //    }
        //    if (usecoin && useAbili && mPlayer.ownMaxMana <= 2) actionVal -= 40;
        //    if (usecoin) actionVal -= 5 * mPlayer.manaTurnEnd;
        //    if (mPlayer.manaTurnEnd >= 2 && !useAbili)
        //    {
        //        actionVal -= 10;
        //        if (mPlayer.ownHeroName == HeroEnum.thief && (mPlayer.ownWeaponDurability >= 2 || mPlayer.ownWeaponAttack >= 2)) actionVal += 10;
        //    }
        //    //if (usecoin && p.mana >= 1) actionVal -= 20;
        //    featureList.Append(new PyInt(actionVal));

        //    int handMobFeature = 0;
        //    int mobsInHand = 0;
        //    int bigMobsInHand = 0;
        //    foreach (Handmanager.Handcard hc in mPlayer.owncards)
        //    {
        //        if (hc.card.type == CardDB.cardtype.MOB)
        //        {
        //            mobsInHand++;
        //            if (hc.card.Attack >= 3) bigMobsInHand++;
        //        }
        //    }

        //    if (ownMinionsCount - ePlayer.ownMinions.Count >= 4 && bigMobsInHand >= 1)
        //    {
        //        handMobFeature += bigMobsInHand * 25;
        //    }
        //    featureList.Append(new PyInt(handMobFeature));

        //    int enemyMinionFeature = 0;
        //    //bool hasTank = false;
        //    foreach (Minion m in ePlayer.ownMinions)
        //    {
        //        enemyMinionFeature += bh.getEnemyMinionValue(m, p);
        //        //hasTank = hasTank || m.taunt;
        //    }
        //    featureList.Append(new PyInt(-enemyMinionFeature));

        //    int otherVal = 0;
        //    otherVal -= ePlayer.ownSecretsIDList.Count;
        //    otherVal -= p.lostDamage;//damage which was to high (like killing a 2/1 with an 3/3 -> => lostdamage =2
        //    otherVal -= p.lostWeaponDamage;

        //    //if (p.ownMinions.Count == 0) retval -= 20;
        //    //if (ePlayer.ownMinions.Count == 0) retval += 20;
        //    //if (ePlayer.ownHero.Hp <= 0) otherVal = 10000;
        //    //soulfire etc
        //    int deletecardsAtLast = 0;
        //    foreach (Action a in actions)
        //    {
        //        if (a.actionType != actionEnum.playcard) continue;
        //        if (a.card.card.name == CardDB.cardName.soulfire || a.card.card.name == CardDB.cardName.doomguard || a.card.card.name == CardDB.cardName.succubus) deletecardsAtLast = 1;
        //        if (deletecardsAtLast == 1 && !(a.card.card.name == CardDB.cardName.soulfire || a.card.card.name == CardDB.cardName.doomguard || a.card.card.name == CardDB.cardName.succubus)) otherVal -= 20;
        //    }
        //    if (ePlayer.ownHero.Hp >= 1 && p.guessingHeroHP <= 0)
        //    {
        //        if (p.turnCounter < 2) otherVal += mPlayer.owncarddraw * 100;
        //        otherVal -= 1000;
        //    }
        //    //if (mPlayer.ownHero.Hp <= 0) retval = -10000;
        //    featureList.Append(new PyInt(otherVal));

        //    return featureList;
        //}


        public string getStateString(PlayerKeyInfo ownkeyInfo, PlayerKeyInfo enemykeyInfo)
        {
            string heroString = "own:" + ownkeyInfo.heroInfo.entity + ", hp:" + ownkeyInfo.heroInfo.Hp + ", mana:" + ownkeyInfo.maxMana +"\n";
            heroString += "enemy:" + enemykeyInfo.heroInfo.entity + ", hp:" + enemykeyInfo.heroInfo.Hp + ", mana:" + enemykeyInfo.maxMana +"\n";
            string minionString = "own minion======:\n";
            foreach (PlayerKeyInfo.MinionKeyInfo mk in ownkeyInfo.minionJsonList)
            {
                minionString += mk.name + ": " + mk.Hp + "\n";
            }

            minionString += "enemy minion======:\n";
            foreach (PlayerKeyInfo.MinionKeyInfo mk in enemykeyInfo.minionJsonList)
            {
                minionString += mk.name + ": " + mk.Hp + "\n";
            }

            string cardString = "own card======:\n";
            foreach (PlayerKeyInfo.CardKeyInfo mk in ownkeyInfo.handcardJsonList)
            {
                cardString += mk.cardName + "\n";
            }

            cardString += "enemy card======:\n";
            foreach (PlayerKeyInfo.CardKeyInfo mk in enemykeyInfo.handcardJsonList)
            {
                cardString += mk.cardName + "\n";
            }

            return heroString + minionString + cardString;
        }

        public void SaveExp(GameRecord gc)
        { 
            //PyList feature_list = new PyList();
            //PyList result_list = new PyList();
            //PyList string_list = new PyList();

            //int result = gc.result;
            //for (int i = 0; i < gc.playSec.Count; i+=2)
            //{
            //    if (i % 4 == 0)
            //    {
            //        result = gc.result == 1 ? 0 : 1;
            //    }
            //    else
            //    {
            //        result = gc.result;
            //    }
                
            //    PlayerKeyInfo ownkeyInfo = gc.playSec[i];
            //    PlayerKeyInfo enemykeyInfo = gc.playSec[i + 1];

            //    PyList turn_feature = new PyList();

            //    PyList end_feature = new PyList();
            //    foreach (int ft in ownkeyInfo.endTurnFeatrueList)
            //    {
            //        PythonUtils.AppendRecycle(end_feature, new PyInt(ft));
            //    }

            //    PyList hand_feature = getHandFeature(ownkeyInfo, enemykeyInfo);
            //    PythonUtils.AppendRecycle(turn_feature, end_feature);
            //    PythonUtils.AppendRecycle(turn_feature, hand_feature);

            //    PyString state_string = new PyString(getStateString(ownkeyInfo, enemykeyInfo));

            //    PythonUtils.AppendRecycle(feature_list, turn_feature);
            //    PythonUtils.AppendRecycle(result_list, new PyInt(result));
            //    PythonUtils.AppendRecycle(string_list, state_string);
            //}

            //evaluator.save_one_episode(feature_list, result_list, string_list);
            //feature_list.Dispose();
            //result_list.Dispose();
            //string_list.Dispose();
        }

        public int getBestCandidate(List<Playfield> candidateStates, bool playerSide)
        {
            int ret = 0;
            PyList handList = new PyList();
            PyList endTurnList = new PyList();

            foreach (Playfield p in candidateStates)
            {
                PyList pyHandFt = getHandFeature(p, playerSide);
                PythonUtils.AppendRecycle(handList, pyHandFt);

                List<int> endTurnFeat = bh.getPlayfieldFeature(p, playerSide);
                PyList pyEndTurnFeat = new PyList();
                foreach (int ft in endTurnFeat)
                {
                    PythonUtils.AppendRecycle(pyEndTurnFeat, new PyInt(ft));
                }
                PythonUtils.AppendRecycle(endTurnList, pyEndTurnFeat);
            }
            PyList feature_list = new PyList();
            PythonUtils.AppendRecycle(feature_list, endTurnList);
            PythonUtils.AppendRecycle(feature_list, handList);

            dynamic result = evaluator.advance(feature_list, Py.kw("verbose", 0));
            ret = result.AsManagedObject(typeof(int));
            result.Dispose();
            feature_list.Dispose();
            return ret;

        }

        public float predict(List<int> endTurnFeature, dynamic hand_feature)
        {
            PyList feature_list = new PyList();
            PyList end_feature = new PyList();
            foreach (int f in endTurnFeature)
            {
                end_feature.Append(new PyInt(f));
            }
            feature_list.Append(np.array(end_feature).reshape(1, -1));
            feature_list.Append(hand_feature.reshape(1, -1));
            //dynamic result = evaluator.model.predict_proba(feature_list, Py.kw("verbose", 0))[0];
            dynamic result = evaluator.model.predict(feature_list, Py.kw("verbose", 0))[0];
            return result;
        }

    }
}
