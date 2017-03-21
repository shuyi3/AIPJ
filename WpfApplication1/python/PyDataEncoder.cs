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
                if (card == null)
                {                  
                    return null;
                }
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
                if (!found)
                {                    
                    return null;
                }
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
                    return null;
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

        public void EncodeNormalFeatureH5(string fileName)
        {
            dynamic h5py = Py.Import("h5py");
            StreamReader file = new StreamReader(fileName);
            string line = null;
            PyList[] features = new PyList[9];
            for (int i = 0; i < 9; i++) features[i] = new PyList();
            PyList resultList = new PyList();
            int count = 0;
            string[] featureNames = null;
            while ((line = file.ReadLine()) != null)
            {
                GameRecord gameRecord = JsonConvert.DeserializeObject<GameRecord>(line);
                List<StateKeyInfo> playSec = new List<StateKeyInfo>();
                foreach (StateKeyInfo stKeyInfo in gameRecord.playSec)
                {
                    int result = (stKeyInfo.attackPlayer.turn == gameRecord.result) ? 1 : 0;
                    StateFeature normalFeature = Featurization.normalFeaturization(stKeyInfo);
                    Feature playableFeature = normalFeature.featrueArray[5];
                    PyList[] featureList = normalFeature.getPyFeatureData();
                    if (featureNames == null) featureNames = normalFeature.getFeatureNames();
                    for (int i = 0; i < 9; i++){
                        PythonUtils.AppendRecycle(features[i], featureList[i]);
                    }
                    PythonUtils.AppendRecycle(resultList, new PyInt(result));
                }
            }
            string outFileName = fileName + "norm.hdf5";
            dynamic outFile = h5py.File(outFileName, "w");
            for (int i = 0; i < 9; i++)
                outFile.create_dataset(featureNames[i], Py.kw("data", features[i]));
            outFile.close();

        }

        public void EncodeInteractionFeature(string fileName)
        {
            dynamic h5py = Py.Import("h5py");
            StreamReader file = new StreamReader(fileName);
            PyList[] features = new PyList[6];
            for (int i = 0; i < 6; i++) features[i] = new PyList();
            PyList targetList = new PyList();
            string line = null;
            string[] featureNames = null;
            int count = 0;
            while ((line = file.ReadLine()) != null)
            {
                GameRecord gameRecord = JsonConvert.DeserializeObject<GameRecord>(line);
                List<StateKeyInfo> playSec = new List<StateKeyInfo>();

                int lastEntity = 1000;
                foreach (StateKeyInfo stKeyInfo in gameRecord.playSec)
                {
                    int result = (stKeyInfo.attackPlayer.turn == gameRecord.result) ? 1 : 0;
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

                    Player mPlayer = tempPf.getCurrentPlayer(true);
                    Player ePlayer = tempPf.getCurrentPlayer(false);
                    StateFeature interFeature = Featurization.interactionFeaturization(tempPf);                   

                    foreach (PlayerKeyInfo.ActionKeyInfo actionKeyInfo in p1Info.playedActionJsonList)
                    {                        
                        Action action = CreateActionFromInfo(tempPf, mPlayer, ePlayer, actionKeyInfo);
                        int target;
                        if (action.actionType == actionEnum.playcard)
                        {
                            target = Featurization.cardIdxDict[action.card.card.name];
                            
                        }
                        else if (action.actionType == actionEnum.useHeroPower)
                        {
                            target = Featurization.cardIdxDict[CardDB.cardName.fireblast];
                        }
                        tempPf.getNextEntity();
                        tempPf.doAction(action);
                        PyList[] featureList = interFeature.getPyData();
                        if (featureNames == null) featureNames = interFeature.getFeatureNames();
                        for (int i = 0; i < interFeature.numFeatures; i++)
                        {
                            PythonUtils.AppendRecycle(features[i], featureList[i]);
                        }
                        PythonUtils.AppendRecycle(targetList, new PyInt(result)); 
                        interFeature = Featurization.interactionFeaturization(tempPf);
                    }
                    lastEntity = tempPf.getNextEntity() + 1;                    
                }
                count++;
                if (count % 1000 == 0) Console.WriteLine(count);
            }
            string outFileName = fileName + "inter.hdf5";
            dynamic outFile = h5py.File(outFileName, "w");
            for (int i = 0; i < features.Length; i++)
                outFile.create_dataset(featureNames[i], Py.kw("data", features[i]));
            outFile.create_dataset("Target", Py.kw("data", targetList));
            outFile.close();
        }


        public void Encode(string fileName)
        {
            StreamReader file = new StreamReader(fileName);
            string line = null;
            int count = 0;
            while ((line = file.ReadLine()) != null)  
            {              
                if (count % 500 == 0)
                {
                    Console.WriteLine("Read " + count + " lines.");
                }
               
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

                    //hero power
                    stKeyInfo.attackPlayer.canPlayHeroPower = new List<int>();
                    stKeyInfo.attackPlayer.canPlayHeroPower.Add(1);

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

                            bool canPlayHeroPower = mPlayer.ownAbilityReady && mPlayer.mana >= 2;
                            if (action.actionType == actionEnum.useHeroPower)
                                Debug.Assert(canPlayHeroPower == false);

                            if (canPlayHeroPower)
                            {
                                stKeyInfo.attackPlayer.canPlayHeroPower.Add(1);
                            }
                            else
                            {
                                stKeyInfo.attackPlayer.canPlayHeroPower.Add(0);
                            }
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
                Helpfunctions.Instance.WriteResultToFile(@"\svs_result_3.1.txt", JsonConvert.SerializeObject(gameRecord));
            }
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                