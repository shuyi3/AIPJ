using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using System.Diagnostics;

namespace HRSim
{
    class DNNEval
    {
        dynamic model = null;
        dynamic np = null;
        dynamic board_encoder = null;
        dynamic py_utils = null;
        HashSet<int> featureSparseSet = new HashSet<int>();
        int offset = 0;


        //static string[] minionCardArray = { "annoyotron", "archmageantonidas", "boombot", "clockworkgnome", 
        //                         "cogmaster", "damagedgolem", "drboom",
        //                            "goblinblastmage", "harvestgolem", "loatheb", "manawyrm", "mechanicalyeti", "mechwarper",
        //                         "snowchugger", "spidertank", "tinkertowntechnician", "unknown" };

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

        private static DNNEval instance;

        private DNNEval()
        {
            Init();
        }

        public static DNNEval Instance
        {
            get
            {
                return instance ?? (instance = new DNNEval());
            }
        }

        public void Init()
        {
            using (Py.GIL())
            {
                PythonEngine.Initialize();
                dynamic sys = PythonEngine.ImportModule("sys");
                np = Py.Import("numpy");
                board_encoder = Py.Import("simple_dqn.encoder");
                py_utils = Py.Import("simple_dqn.py_utils");
                dynamic cnn_model = Py.Import("simple_dqn.cnn_model");
                //model = cnn_model.CNNPhaseEval();
                model = cnn_model.CNNPhaseActionPolicy();
                //model = cnn_model.CNNPhasePolicy();
                //dynamic models = Py.Import("keras.models");
                //dynamic layers = Py.Import("keras.layers");
                //dynamic optimizers = Py.Import("keras.optimizers");

                //dynamic Dense = layers.Dense;
                //dynamic Dropout = layers.Dropout;
                //dynamic Activation = layers.Activation;
                //dynamic sequential = models.Sequential;
                //dynamic SGD = optimizers.SGD;
                //model = sequential();

                //model = cnn_model.compile_cnn_model(Py.kw("weight_file", policy_weight_file));
            }
        }

        public void SaveExp(GameRecord gc)
        { 
            //PyList feature_list = new PyList();
            //PyList result_list = new PyList();

            //int result = gc.result;
            //for (int i = 0; i < gc.playSec.Count; i+=2)
            //{
            //    if (i % 4 == 0)
            //        result = result == 1 ? 0 : 1;
                
            //    PlayerKeyInfo pk = gc.playSec[i];
            //    PyList feature = new PyList();
            //    foreach (int ft in pk.endTurnFeatrueList)
            //    {
            //        feature.Append(new PyInt(ft));
            //    }
            //    feature_list.Append(feature);
            //    result_list.Append(new PyInt(result));
            //}
        }

        public float PredictValue(dynamic feature_list)
        {
            dynamic result = model.predict(feature_list);
            return result;
        }

        public float predict(dynamic feature_list)
        {
            dynamic result = model.predict_proba(feature_list).item(1);
            return result;
        }

        public int encodeHeroHp(int heroHp)
        {
            int hpFeature;

            if (heroHp <= 6)
            {
                hpFeature = 0;
            }
            else if (heroHp <= 12)
            {
                hpFeature = 1;
            }
            else if (heroHp <= 15)
            {
                hpFeature = 2;
            }
            else if (heroHp <= 20)
            {
                hpFeature = 3;
            }
            else
            {
                hpFeature = 4;
            }

            return hpFeature;
        }

        public void encodeHeroFeature(Playfield p, bool own)
        {
            Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                ePlayer = p.playerFirst;
                mPlayer = p.playerSecond;
            }

            int ownMaxMana = mPlayer.mana;
            int enemyMaxMana = ePlayer.mana;
            int ownHeroHp = mPlayer.ownHero.Hp + mPlayer.ownHero.armor;
            int enemyHeroHp = ePlayer.ownHero.Hp + ePlayer.ownHero.armor;

            int ownHeroFeature = encodeHeroHp(ownHeroHp);
            int ownManaFeature = ownMaxMana - 1;

            int enemyHeroFeature = encodeHeroHp(enemyHeroHp);
            int enemyManaFeature = Math.Min(9, enemyMaxMana);

            featureSparseSet.Add(offset + ownManaFeature);
            offset += 10;
            featureSparseSet.Add(offset + ownHeroFeature);
            offset += 5;
            featureSparseSet.Add(offset + enemyManaFeature);
            offset += 10;
            featureSparseSet.Add(offset + enemyHeroFeature);
            offset += 5;

        }

        public int minionOneHot(Minion m)
        {
            int minionNamaIndex = Array.IndexOf(minionCardArray, m.name.ToString());
            int hp = m.Hp;
            int attack = m.Angr;
            int hpIndex = 0;
            if (hp <= 1)
            {
                hpIndex = 0;
            }
            else if (hp == 2)
            {
                hpIndex = 1;
            }
            else if (hp < 5)
            {
                hpIndex = 2;
            }
            else if (hp < 7)
            {
                hpIndex = 3;
            }
            else
            {
                hpIndex = 4;
            }

            int index = minionNamaIndex * 10 + 2 * hpIndex;
            return index;
        }

        public void encodeBoardFeature(Player p)
        {
            //Console.WriteLine("offset = " + offset);

            int featureLeanth = 10 * minionCardArray.Length;

            foreach (Minion m in p.ownMinions)
            {
                int minionIndex = offset + minionOneHot(m);
                Debug.Assert(minionIndex % 2 == 0);
                if (!featureSparseSet.Contains(minionIndex))
                {
                    //Console.WriteLine(m.entitiyID + "," + minionIndex);
                    featureSparseSet.Add(minionIndex);
                }
                else
                {
                    //Console.WriteLine(m.entitiyID + "," + minionIndex);
                    featureSparseSet.Add(minionIndex + 1);
                }
            }

            offset += featureLeanth;
        }

        public void encodeHandFeature(Player p)
        {
            //Console.WriteLine("offset = " + offset);
            int featureLeanth = 2 * cardArray.Length;
            foreach (Handmanager.Handcard hc in p.owncards)
            {
                //Console.WriteLine("hand:" + 2 * Array.IndexOf(cardArray, hc.card.name.ToString()));
                int hcIndex = offset + 2 * Array.IndexOf(cardArray, hc.card.name.ToString());
                if (!featureSparseSet.Contains(hcIndex))
                {
                    featureSparseSet.Add(hcIndex);
                }
                else
                {
                    featureSparseSet.Add(hcIndex + 1);
                }
            }
            offset += featureLeanth;

        }

        public dynamic parsePlayfield(Playfield p, bool own)
        {
            Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                ePlayer = p.playerFirst;
                mPlayer = p.playerSecond;
            }

            offset = 0;
            featureSparseSet.Clear();

            encodeHeroFeature(p, own);
            encodeBoardFeature(mPlayer);
            encodeBoardFeature(ePlayer);
            encodeHandFeature(mPlayer);
            encodeHandFeature(ePlayer);

            PyInt width = new PyInt(458);
            PyInt height = new PyInt(1);

            PyTuple feature_shape = new PyTuple(new PyObject[] { height, width });

            dynamic feature_py_list = np.zeros(feature_shape);

            foreach (int i in featureSparseSet)
            {
                //Console.WriteLine(i);
                feature_py_list.itemset(i, 1);
            }

            return feature_py_list;
        }

        public void getNNActionPolicy(Playfield p, bool own)
        {
            Playfield temp = new Playfield(p);
            Player mPlayer = temp.getCurrentPlayer(true);
            Player ePlayer = temp.getCurrentPlayer(false);

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                hc.playProb = 0.0;
            }

            bool canPlayCard = true;
            while (canPlayCard)
            {
                canPlayCard = false;
                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (hc.canplayCard(temp, own))
                    {
                        canPlayCard = true;
                        break;
                    }
                }
                if (canPlayCard)
                {
                    dynamic board_encode = parsePlayfieldCNNAction(p, temp, own);
                    double cardToPlay = model.predict_classes(board_encode)[0];
                    Console.WriteLine("cardToPlay: " + cardToPlay);
                    foreach (Handmanager.Handcard hc in mPlayer.owncards)
                    {
                        if (cardIdxDict[hc.card.name] == cardToPlay)
                        {
                            Console.WriteLine("hc:" + hc.card.name);
                            hc.playProb = 1.0;
                            break;
                        }
                    }

                    List<Action> actions;
                    if (cardIdxDict[CardDB.cardName.fireblast] == cardToPlay)
                    {
                        actions = new List<Action>(Movegenerator.Instance.getHeroPowerMoveList(temp, false, true, true));
                    }
                    else
                    {
                        actions = new List<Action>(Movegenerator.Instance.getPlaycardMoveList(temp, false, true, true, 1.0));
                    }
                    if (actions.Count == 0)
                    {
                        canPlayCard = false;
                        break;
                    }
                    int actionIdx = GameManager.getRNG().Next(actions.Count);
                    temp.doAction(actions[actionIdx]);
                    board_encode.Dispose();
                }
            }
            Player oPlayer = p.getCurrentPlayer(true);
            Console.WriteLine("Action length: " + mPlayer.playactions.Count);
            foreach (Handmanager.Handcard hc in oPlayer.owncards)
            {
                hc.playProb = 0.0;
            }
            foreach (Action action in mPlayer.playactions)
            {
                if (action.actionType == actionEnum.playcard)
                {
                    foreach (Handmanager.Handcard hc in oPlayer.owncards)
                    {
                        Console.WriteLine("Entity: " + action.card.entity);
                        Console.WriteLine("Entity to pair: " + hc.entity);

                        if (hc.entity == action.card.entity)
                        {
                            hc.playProb = 1.0;
                            Console.WriteLine("will play card: " + hc.card.name);
                        }
                    }
                }
            }
        }


        public void getNNPolicy(Playfield p, bool own)
        {
            dynamic board_encode = parsePlayfieldCNN(p, own);
            dynamic prob_list = getNNPolicyCard(board_encode);
            Player mPlayer = p.getCurrentPlayer(true);
            Player ePlayer = p.getCurrentPlayer(false);

            foreach (CardDB.cardName cn in cardIdxDict.Keys)
            {
                int idx = cardIdxDict[cn];
                double prob = prob_list[idx];
                //string formattedString = prob.ToString(".################");

                //Console.WriteLine(cn + ":" + formattedString);
            }

            List<double> cardProbList = new List<double>();
            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                double prob = prob_list[cardIdxDict[hc.card.name]];
                cardProbList.Add(prob);
            }

            MathUtils.SoftMax(cardProbList);
            for (int i = 0; i < mPlayer.owncards.Count; i++)
            {
                Handmanager.Handcard hc = mPlayer.owncards[i];
                //Console.WriteLine("card idx:" + cardIdxDict[hc.card.name]);
                hc.playProb = cardProbList[i];
                Console.WriteLine(hc.card.name + ":" + hc.playProb);
                cardProbList.Add(hc.playProb);
            }

            var mySortedCards = mPlayer.owncards.OrderByDescending(l => l.playProb);
            int totalCost = 0;
            int maxMana = mPlayer.ownMaxMana;
            bool toPlay = true;

            foreach (Handmanager.Handcard hc in mySortedCards)
            {
                if (!toPlay)
                {
                    hc.playProb = 0.0;
                }
                else
                {
                    int manaCost = hc.getManaCost(p, p.isOwnTurn);
                    totalCost += manaCost;
                    if (totalCost > maxMana)
                        toPlay = false;
                    //Console.WriteLine("to play: " + hc.card.name);
                }
            }

            for (int i = 0; i < mPlayer.owncards.Count; i++)
            {
                Handmanager.Handcard hc = mPlayer.owncards[i];
                //Console.WriteLine(hc.card.name + ":" + hc.playProb);
            }

            int offSet = cardIdxDict.Count;
            List<double> targetProbList = new List<double>();

            foreach (Minion m in ePlayer.ownMinions)
            {
                double prob = prob_list[offSet + cardIdxDict[m.name]];
                targetProbList.Add(prob);
            }

            MathUtils.SoftMax(targetProbList);

            for (int i = 0; i < ePlayer.ownMinions.Count; i++)
            {
                Minion m = ePlayer.ownMinions[i];
                //Console.WriteLine("minion idx:" + (offSet + cardIdxDict[m.name]));
                m.targetProb = targetProbList[i];
                //Console.WriteLine(m.name + ":" + m.targetProb);
            }

            board_encode.Dispose();
            prob_list.Dispose();
        }

        public float getNNEval(Playfield p, bool own)
        {
            //dynamic feature_list = parsePlayfield(p, own);
            //Console.WriteLine(feature_list.item(0));
            //return predict(feature_list);
            //dynamic board_encode = parsePlayfieldCNN(p, own);
            //dynamic card_prob = getNNPolicyCard(board_encode);
            //Player mPlayer = p.getCurrentPlayer(true);
            //foreach (Handmanager.Handcard hc in mPlayer.owncards)
            //{
            //    Console.WriteLine("cardIdxDict:" + cardIdxDict[hc.card.name]);
            //    hc.playProb = card_prob[cardIdxDict[hc.card.name]];
            //    Console.WriteLine(hc.card.name + ":" + hc.playProb);
            //}
            return 0.0f;
        }

        public dynamic getNNPolicy(dynamic board_encode)
        {
            dynamic card_prob = model.predict_proba(board_encode);
            return card_prob;
        }

        public dynamic getNNPolicyCard(dynamic board_encode)
        {
            dynamic card_prob = model.predict_policy(board_encode);
            return card_prob[0];
        }

        public dynamic parsePlayfieldCNNAction(Playfield startP, Playfield curP, bool own)
        {

            Player mPlayer, ePlayer;
            List<CardDB.Card> mDeck, eDeck;
            if (own)
            {
                mPlayer = startP.playerFirst;
                ePlayer = startP.playerSecond;
                mDeck = startP.homeDeck;
                eDeck = startP.awayDeck;
            }
            else
            {
                ePlayer = startP.playerFirst;
                mPlayer = startP.playerSecond;
                eDeck = startP.homeDeck;
                mDeck = startP.awayDeck;
            }

            PyInt ownMana = new PyInt(mPlayer.ownMaxMana);
            PyInt ownHp = new PyInt(mPlayer.ownHero.Hp + mPlayer.ownHero.armor);
            PyInt enemyMana = new PyInt(ePlayer.ownMaxMana);
            PyInt enemyHp = new PyInt(ePlayer.ownHero.Hp + ePlayer.ownHero.armor);

            PyList hero_feature = new PyList(new PyObject[] { ownMana, ownHp, enemyMana, enemyHp });
            ownMana.Dispose();
            ownHp.Dispose();
            enemyMana.Dispose();
            enemyHp.Dispose();

            PyList own_hand_list = new PyList();
            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                PyDict hand_card = new PyDict();
                hand_card["cardName"] = new PyString(hc.card.name.ToString());
                PythonUtils.AppendRecycle(own_hand_list, hand_card);
            }

            PyList enemy_hand_list = new PyList();
            foreach (Handmanager.Handcard hc in ePlayer.owncards)
            {
                PyDict hand_card = new PyDict();
                hand_card["cardName"] = new PyString(hc.card.name.ToString());
                PythonUtils.AppendRecycle(enemy_hand_list, hand_card);
            }

            PyList own_minion_list = new PyList();
            foreach (Minion m in mPlayer.ownMinions)
            {
                PyDict minion = new PyDict();
                minion["Hp"] = new PyInt(m.Hp);
                minion["Angr"] = new PyInt(m.Angr);
                minion["name"] = new PyString(m.name.ToString());
                PythonUtils.AppendRecycle(own_minion_list, minion);
            }

            PyList enemy_minion_list = new PyList();
            foreach (Minion m in ePlayer.ownMinions)
            {
                PyDict minion = new PyDict();
                minion["Hp"] = new PyInt(m.Hp);
                minion["Angr"] = new PyInt(m.Angr);
                minion["name"] = new PyString(m.name.ToString());
                PythonUtils.AppendRecycle(enemy_minion_list, minion);
            }

            PyList own_deck_list = new PyList();
            foreach (CardDB.Card c in mDeck)
            {
                PyString card_name = new PyString(c.name.ToString());
                PythonUtils.AppendRecycle(own_deck_list, card_name);
            }

            PyList enemy_deck_list = new PyList();
            foreach (CardDB.Card c in eDeck)
            {
                PyString card_name = new PyString(c.name.ToString());
                PythonUtils.AppendRecycle(enemy_deck_list, card_name);
            }

            PyList own_play_list = new PyList();
            PyList playable_list = new PyList();
            Player curPlayer = curP.getCurrentPlayer(true);
            foreach (Action action in curPlayer.playactions)
            {
                if (action.actionType == actionEnum.playcard)
                {
                    PyString card_name = new PyString(action.card.card.name.ToString());
                    PythonUtils.AppendRecycle(own_play_list, card_name);
                }
            }

            foreach (Handmanager.Handcard hc in curPlayer.owncards)
            {
                if (hc.canplayCard(curP, own))
                {
                    PyString playable_card = new PyString(hc.card.name.ToString());
                    PythonUtils.AppendRecycle(playable_list, playable_card);
                }
            }

            //fire blast
            if (curPlayer.ownAbilityReady && curPlayer.mana >= 2)
            {
                PyString playable_card = new PyString("fireblast");
                PythonUtils.AppendRecycle(playable_list, playable_card);
            }

            dynamic res_list = board_encoder.encode_for_cnn_phase(hero_feature, own_hand_list, own_minion_list,
                enemy_hand_list, enemy_minion_list, own_deck_list, enemy_deck_list, own_play_list, playable_list);

            hero_feature.Dispose();
            own_hand_list.Dispose();
            own_minion_list.Dispose();
            enemy_hand_list.Dispose();
            enemy_minion_list.Dispose();
            own_deck_list.Dispose();
            enemy_deck_list.Dispose();
            own_play_list.Dispose();

            return res_list;
        }

        public dynamic parsePlayfieldCNN(Playfield p, bool own)
        {

            Player mPlayer, ePlayer;
            List<CardDB.Card> mDeck, eDeck;
            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
                mDeck = p.homeDeck;
                eDeck = p.awayDeck;
            }
            else
            {
                ePlayer = p.playerFirst;
                mPlayer = p.playerSecond;
                eDeck = p.homeDeck;
                mDeck = p.awayDeck;
            }

            PyInt ownMana = new PyInt(mPlayer.ownMaxMana);
            PyInt ownHp = new PyInt(mPlayer.ownHero.Hp + mPlayer.ownHero.armor);
            PyInt enemyMana = new PyInt(ePlayer.ownMaxMana);
            PyInt enemyHp = new PyInt(ePlayer.ownHero.Hp + ePlayer.ownHero.armor);

            PyList hero_feature = new PyList(new PyObject[] { ownMana, ownHp, enemyMana, enemyHp });
            ownMana.Dispose();
            ownHp.Dispose();
            enemyMana.Dispose();
            enemyHp.Dispose();

            PyList own_hand_list = new PyList(); 
            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                PyDict hand_card = new PyDict();
                hand_card["cardName"] = new PyString(hc.card.name.ToString());
                PythonUtils.AppendRecycle(own_hand_list, hand_card);
            }

            PyList enemy_hand_list = new PyList();
            foreach (Handmanager.Handcard hc in ePlayer.owncards)
            {
                PyDict hand_card = new PyDict();
                hand_card["cardName"] = new PyString(hc.card.name.ToString());
                PythonUtils.AppendRecycle(enemy_hand_list, hand_card);
            }

            PyList own_minion_list = new PyList();
            foreach (Minion m in mPlayer.ownMinions)
            {
                PyDict minion = new PyDict();
                minion["Hp"] = new PyInt(m.Hp);
                minion["Angr"] = new PyInt(m.Angr);
                minion["name"] = new PyString(m.name.ToString());
                PythonUtils.AppendRecycle(own_minion_list, minion);
            }

            PyList enemy_minion_list = new PyList();
            foreach (Minion m in ePlayer.ownMinions)
            {
                PyDict minion = new PyDict();
                minion["Hp"] = new PyInt(m.Hp);
                minion["Angr"] = new PyInt(m.Angr);
                minion["name"] = new PyString(m.name.ToString());
                PythonUtils.AppendRecycle(enemy_minion_list, minion);
            }

            PyList own_deck_list = new PyList();
            foreach (CardDB.Card c in mDeck)
            {
                PyString card_name = new PyString(c.name.ToString());
                PythonUtils.AppendRecycle(own_deck_list, card_name);
            }

            PyList enemy_deck_list = new PyList();
            foreach (CardDB.Card c in eDeck)
            {
                PyString card_name = new PyString(c.name.ToString());
                PythonUtils.AppendRecycle(enemy_deck_list, card_name);
            }

            dynamic own_play_list = new PyList();
            foreach (Action action in mPlayer.playactions)
            {
                if (action.actionType == actionEnum.playcard)
                {
                    PyString card_name = new PyString(action.card.card.name.ToString());
                    PythonUtils.AppendRecycle(own_play_list, card_name);
                }
            }

            dynamic res_list = board_encoder.encode_for_cnn_phase(hero_feature, own_hand_list, own_minion_list, 
                enemy_hand_list, enemy_minion_list, own_deck_list, enemy_deck_list, own_play_list);

            hero_feature.Dispose();
            own_hand_list.Dispose();
            own_minion_list.Dispose();
            enemy_hand_list.Dispose();
            enemy_minion_list.Dispose();
            own_deck_list.Dispose();
            enemy_deck_list.Dispose();
            own_play_list.Dispose();

            return res_list;
        }
    }
}
