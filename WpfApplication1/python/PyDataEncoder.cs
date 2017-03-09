using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Python.Runtime;
using Newtonsoft.Json;
using System.Diagnostics;

namespace HRSim
{
    class PyDataEncoder
    {
        private dynamic dataEncoder;
        private dynamic py_utils;
        private static PyDataEncoder instance;

        private PyDataEncoder()
        {
            Init();
        }

        public static PyDataEncoder Instance
        {
            get
            {
                return instance ?? (instance = new PyDataEncoder());
            }
        }

        public void Init()
        {
            using (Py.GIL())
            {
                PythonEngine.Initialize();
                dynamic sys = PythonEngine.ImportModule("sys");
                dataEncoder = Py.Import("simple_dqn.data_encoder");
                py_utils = Py.Import("simple_dqn.py_utils");
            }
        }

        public Action CreateActionFromInfo(Playfield pf, Player mPlayer, Player ePlayer, PlayerKeyInfo.ActionKeyInfo keyInfo)
        {
            Handmanager.Handcard card = null;


            //if (keyInfo.cardEntitiy > pf.nextEntity-1)
            //{
            //    keyInfo.cardEntitiy = pf.nextEntity-1;
            //    debug1++;
            //}
            //if (keyInfo.ownEntity > pf.nextEntity - 1)
            //{
            //    keyInfo.ownEntity = pf.nextEntity - 1;
            //    debug1++;
            //}
            //if (keyInfo.targetEntity > pf.nextEntity - 1)
            //{
            //    keyInfo.targetEntity = pf.nextEntity - 1;
            //    debug1++;
            //}

            //if (debug1 > 1)
            //{
            //    int debug = 1;
            //}

            if (keyInfo.cardEntitiy != -10 && keyInfo.cardEntitiy != -1)
            {
                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (hc.entity == keyInfo.cardEntitiy)
                    {
                        card = hc;
                        break;
                    }
                }
                if (card == null)
                {
                    foreach (Handmanager.Handcard hc in mPlayer.owncards)
                    {
                        if (hc.entity == keyInfo.cardEntitiy)
                        {
                            card = hc;
                            break;
                        }
                    }
                }
                //if (card == null)
                //{
                //    pf.printBoard();
                //    int debug = 1;
                //    return null;
                //}
            }

            Minion ownMinion = null;
            if (keyInfo.ownEntity != -10)
            {
                bool found = false;
                foreach (Minion m in mPlayer.ownMinions)
                {
                    if (m.entitiyID == keyInfo.ownEntity)
                    {
                        ownMinion = m;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    foreach (Minion m in ePlayer.ownMinions)
                    {
                        if (m.entitiyID == keyInfo.ownEntity)
                        {
                            ownMinion = m;
                            found = true;
                            break;
                        }
                    }
                }
                //if (!found)
                //{
                //    pf.printBoard();
                //    pf.printActions();
                //    int debug = 1;
                //    return null;
                //}
            } 

            Minion target = null;
            Minion mDRMinion = null;

            if (keyInfo.targetEntity == 0 || keyInfo.targetEntity == 1)
            {
                target = ePlayer.ownHero;
            }
            else if (keyInfo.targetEntity != -10)
            {
                bool found = false;
                foreach (Minion m in ePlayer.ownMinions)
                {
                    if (m.entitiyID == keyInfo.targetEntity)
                    {
                        target = m;
                        found = true;
                        break;
                    }
                    if (m.name == CardDB.cardName.damagedgolem)
                    {
                        mDRMinion = m;
                    }
                }
                if (!found)
                {
                    foreach (Minion m in mPlayer.ownMinions)
                    {
                        if (m.entitiyID == keyInfo.targetEntity)
                        {
                            target = m;
                            found = true;
                            break;
                        }
                        if (m.name == CardDB.cardName.damagedgolem)
                        {
                            mDRMinion = m;
                        }
                    }
                }
                if (!found)
                {
                    //int debug = 1;
                    target = mDRMinion;
                }
            }

            return new Action(keyInfo.actionType, card, ownMinion, keyInfo.place, target, 0, 1, keyInfo.manaCost);
        }

        public void replay(GameRecord gameRecord)
        {
            //GameRecord gameRecord = JsonConvert.DeserializeObject<GameRecord>(line);
            int count = 0;
            foreach (StateKeyInfo stKeyInfo in gameRecord.playSec)
            {
                count++;
                PlayerKeyInfo p1Info = stKeyInfo.attackPlayer;
                PlayerKeyInfo p2Info = stKeyInfo.defensePlayer;

                if (count == 14)
                {
                    int debug = 1;
                }

                bool isOwnTurn = p1Info.turn == 0 ? true : false;

                Playfield tempPf = null;
                if (isOwnTurn)
                {
                    tempPf = new Playfield(stKeyInfo.nextEntity, isOwnTurn, p1Info, p2Info);
                }
                else
                {
                    tempPf = new Playfield(stKeyInfo.nextEntity, isOwnTurn, p2Info, p1Info);
                }

                Helpfunctions.Instance.logg("=====================");
                //tempPf.printBoard();
                foreach (PlayerKeyInfo.ActionKeyInfo actionKeyInfo in p1Info.playedActionJsonList)
                {
                    Player mPlayer = tempPf.getCurrentPlayer(true);
                    Player ePlayer = tempPf.getCurrentPlayer(false);
                    //Console.WriteLine("Action: " + actionKeyInfo.ToString());
                    //tempPf.printBoard();
                    //tempPf.printBoard();
                    Action action = CreateActionFromInfo(tempPf, mPlayer, ePlayer, actionKeyInfo);
                    if (action != null)
                    {
                        tempPf.getNextEntity();
                        tempPf.doAction(action);
                        //action.print();
                    }
                    else
                    {
                        //tempPf.printBoard();
                        foreach (var cardJson in p1Info.playedCardJsonList)
                        {
                            Console.WriteLine(cardJson.cardName + ", " + cardJson.entity);
                        }
                    }
                }
            }
        }

        public void Encode(string fileName)
        {
            StreamReader file = new StreamReader(fileName);
            //for (int i = 0; i < 10; i++)
            string line = null;
            int count = 0;
            //while ((line = file.ReadLine()) != null)
            //{
            //    count++;
            //}
            //Console.WriteLine(count);
            while ((line = file.ReadLine()) != null)  
            {              
                if (count % 500 == 0)
                {
                    Console.WriteLine("Read " + count + " lines.");
                }
                //if (count < 8945)
                //{
                //    count++;
                //    continue;
                //}
                GameRecord gameRecord = JsonConvert.DeserializeObject<GameRecord>(line);
                List<StateKeyInfo> playSec = new List<StateKeyInfo>();

                int lastEntity = 1000;
                foreach (StateKeyInfo stKeyInfo in gameRecord.playSec)
                {
                    PlayerKeyInfo p1Info = stKeyInfo.attackPlayer;
                    PlayerKeyInfo p2Info = stKeyInfo.defensePlayer;

                    bool isOwnTurn = p1Info.turn == 0 ? true : false;

                    Playfield tempPf = null;
                    if (isOwnTurn)
                    {
                        tempPf = new Playfield(lastEntity, isOwnTurn, p1Info, p2Info);
                    }
                    else
                    {
                        tempPf = new Playfield(lastEntity, isOwnTurn, p2Info, p1Info);
                    }

                    stKeyInfo.nextEntity = tempPf.nextEntity;
                    stKeyInfo.turnSt = 1;
                    Player mPlayer = tempPf.getCurrentPlayer(true);
                    Player ePlayer = tempPf.getCurrentPlayer(false);

                    int length = stKeyInfo.attackPlayer.handcardJsonList.Count;
                    stKeyInfo.attackPlayer.handcardJsonList.Clear();

                    foreach (Handmanager.Handcard hc in mPlayer.owncards)
                    {
                        PlayerKeyInfo.CardKeyInfo hcInfo = new PlayerKeyInfo.CardKeyInfo(hc, true, tempPf);
                        stKeyInfo.attackPlayer.handcardJsonList.Add(hcInfo);
                    }

                    length = stKeyInfo.defensePlayer.handcardJsonList.Count;
                    stKeyInfo.defensePlayer.handcardJsonList.Clear();

                    foreach (Handmanager.Handcard hc in ePlayer.owncards)
                    {
                        PlayerKeyInfo.CardKeyInfo hcInfo = new PlayerKeyInfo.CardKeyInfo(hc, true, tempPf);
                        stKeyInfo.defensePlayer.handcardJsonList.Add(hcInfo);
                    }

                    //Console.WriteLine("===================");
                    //tempPf.printBoard();
                    //Console.WriteLine("Next Entity:" + tempPf.nextEntity);
                    //Console.WriteLine("===================");

                    foreach (PlayerKeyInfo.ActionKeyInfo actionKeyInfo in p1Info.playedActionJsonList)
                    {
                        //Player mPlayer = tempPf.getCurrentPlayer(true);
                        //Player ePlayer = tempPf.getCurrentPlayer(false);
                        //Console.WriteLine("Action: " + actionKeyInfo.ToString());
                        //tempPf.printBoard();
                        Action action = CreateActionFromInfo(tempPf, mPlayer, ePlayer, actionKeyInfo);

                        tempPf.getNextEntity();
                        tempPf.doAction(action);

                        PlayerKeyInfo ownkeyInfo, enemykeyInfo;
                        if (isOwnTurn)
                        {
                            ownkeyInfo = new PlayerKeyInfo(tempPf.playerFirst, tempPf.homeDeck, true, tempPf);
                            enemykeyInfo = new PlayerKeyInfo(tempPf.playerSecond, tempPf.awayDeck, false, tempPf);
                        }
                        else
                        {
                            ownkeyInfo = new PlayerKeyInfo(tempPf.playerSecond, tempPf.awayDeck, false, tempPf);
                            enemykeyInfo = new PlayerKeyInfo(tempPf.playerFirst, tempPf.homeDeck, true, tempPf);
                        }
                        StateKeyInfo interState = new StateKeyInfo(tempPf.nextEntity, ownkeyInfo, enemykeyInfo,
                            false, 0.0);
                        //playSec.Add(interState);
                        if (action.actionType == actionEnum.playcard || action.actionType == actionEnum.useHeroPower)
                        {
                            if (stKeyInfo.attackPlayer.handcardChange == null) stKeyInfo.attackPlayer.handcardChange = new List<List<PlayerKeyInfo.CardKeyInfo>>();
                            //tempPf.printBoard();
                            //foreach (PlayerKeyInfo.CardKeyInfo cardInfo in interState.attackPlayer.handcardJsonList){
                            //    Console.WriteLine(cardInfo.cardName);
                            //    Console.WriteLine(cardInfo.manacost);
                            //    Console.WriteLine(cardInfo.playable);
                            //}
                            stKeyInfo.attackPlayer.handcardChange.Add(interState.attackPlayer.handcardJsonList);
                        }                      
                    }

                    playSec.Add(stKeyInfo);
                    lastEntity = tempPf.getNextEntity() + 1;
                    //Console.WriteLine("===================");
                    //Console.WriteLine("End turn Next Entity:" + lastEntity);
                    //Console.WriteLine("===================");
                }
                gameRecord.playSec = playSec;
                count++;
                Helpfunctions.Instance.WriteResultToFile(@"\svs_result_1.1.txt", JsonConvert.SerializeObject(gameRecord));
            }
        }
    }
}
