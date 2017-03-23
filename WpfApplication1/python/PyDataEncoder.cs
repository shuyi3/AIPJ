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
        private bool debug = false;
        private int debugCounter = 0;

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

        public Action CreateActionFromInfo(Playfield pf, PlayerKeyInfo.ActionKeyInfo keyInfo)
        {
            Handmanager.Handcard card = null;
            Player mPlayer = pf.getCurrentPlayer(true);
            Player ePlayer = pf.getCurrentPlayer(false);

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
                        if (hc.card.name.ToString() == keyInfo.cardName)
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
                foreach (Minion m in mPlayer.ownMinions)
                {
                    if (m.entitiyID == keyInfo.ownEntity)
                    {
                        ownMinion = m;
                        break;
                    }
                }
                if (ownMinion == null)
                {
                    foreach (Minion m in mPlayer.ownMinions)
                    {
                        if (m.handcard.card.name.ToString() == keyInfo.ownName)
                        {
                            ownMinion = m;
                            break;
                        }
                    }
                }
                if (ownMinion == null)
                {                    
                    return null;
                }
            } 

            Minion target = null;

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
                }
                foreach (Minion m in mPlayer.ownMinions)
                {
                    if (m.entitiyID == keyInfo.targetEntity)
                    {
                        target = m;
                        found = true;
                        break;
                    }
                }
                if (target == null)
                {
                    foreach (Minion m in ePlayer.ownMinions)
                    {
                        if (m.handcard.card.name.ToString() == keyInfo.targetName)
                        {
                            target = m;
                            found = true;
                            break;
                        }
                    }
                    foreach (Minion m in mPlayer.ownMinions)
                    {
                        if (m.handcard.card.name.ToString() == keyInfo.targetName)
                        {
                            target = m;
                            found = true;
                            break;
                        }
                    }
                }
                if (target == null)
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
                    Action action = CreateActionFromInfo(tempPf, actionKeyInfo);
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
                        Action action = CreateActionFromInfo(tempPf, actionKeyInfo);
                        int target = 0;
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
                        PythonUtils.AppendRecycle(targetList, new PyInt(target)); 
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

        public class Node
        {
            public Playfield pf;
            public int pcAction;
            public int nonPCaction;
            public Node par;
            public bool triedLeft;
            public bool triedRight;
            public Action action;
            public Node(Playfield pf, Node par, int pcAction, int nonPCaction, Action action)
            {
                this.pf = pf;
                this.pcAction = pcAction;
                this.nonPCaction = nonPCaction;
                this.par = par;
                this.triedLeft = false;
                this.triedRight = false;
                this.action = action;
            }
        }

        public Node tryDoAction(Node cur, PlayerKeyInfo.ActionKeyInfo keyInfo)
        {
            Node ret = null;
            Action action = CreateActionFromInfo(cur.pf, keyInfo);
            Playfield temp = null;
            bool noError = true;

            if (action != null)
            {
                try
                {
                    temp = new Playfield(cur.pf);
                    temp.getNextEntity();
                    temp.doAction(action);
                }
                catch (NullReferenceException e)
                {
                    noError = false;
                }
                if (noError)
                {
                    if (action.actionType == actionEnum.playcard || action.actionType == actionEnum.useHeroPower)
                    {
                        ret = new Node(temp, cur, cur.pcAction + 1, cur.nonPCaction, action);
                    }
                    else
                    {
                        ret = new Node(temp, cur, cur.pcAction, cur.nonPCaction + 1, action);
                    }
                }
            }
            return ret;
        }

        public LinkedList<Action> searchValidSeq(Playfield pf, PlayerKeyInfo p1Info)
        {
            //this.debugCounter++;
            //Console.WriteLine(this.debugCounter);
            //if (this.debugCounter == 11)
            //{
            //    this.debug = true;
            //}
            Playfield tempPf = new Playfield(pf);
            Player mPlayer = tempPf.getCurrentPlayer(true);
            Player ePlayer = tempPf.getCurrentPlayer(false);
            List<int> pcActionId = new List<int>();
            List<int> nonPcActionId = new List<int>();

            for (int i = 0; i < p1Info.playedActionJsonList.Count; i++){
                PlayerKeyInfo.ActionKeyInfo keyInfo = p1Info.playedActionJsonList[i];
                if (keyInfo.actionType == actionEnum.useHeroPower || keyInfo.actionType == actionEnum.playcard)
                {
                    pcActionId.Add(i);
                }
                else
                {
                    nonPcActionId.Add(i);
                }
            }

            Node root = new Node(new Playfield(pf), null, 0, 0, null);
            Node cur = root;
            while (cur.pcAction + cur.nonPCaction < p1Info.playedActionJsonList.Count)
            {
                if (this.debug) Console.WriteLine("pc:" + cur.pcAction + "/" + pcActionId.Count + "| nonPc:" + cur.nonPCaction + "/" + nonPcActionId.Count);
                Node nextNode = null;
                if (cur.pcAction < pcActionId.Count && !cur.triedLeft)
                {
                    //try left                    
                    cur.triedLeft = true;
                    nextNode = tryDoAction(cur, p1Info.playedActionJsonList[pcActionId[cur.pcAction]]);
                }
                else if (cur.nonPCaction < nonPcActionId.Count && !cur.triedRight)
                {
                    cur.triedRight = true;
                    nextNode = tryDoAction(cur, p1Info.playedActionJsonList[nonPcActionId[cur.nonPCaction]]);
                }
                else
                {
                    nextNode = cur.par;
                }
                if (nextNode != null)
                {
                    cur = nextNode;
                }
            }

            LinkedList<Action> ret = new LinkedList<Action>();
            while (cur.action != null)
            {
                ret.AddFirst(cur.action);
                cur = cur.par;
            }
            return ret;
        }

        public void EncodeModified(string fileName)
        {
            StreamReader file = new StreamReader(fileName);
            string line = null;
            int count = 0;
            while ((line = file.ReadLine()) != null)
            {
                //Helpfunctions.Instance.logg(line);
                if (count % 1000 == 0)
                {
                    Console.WriteLine("Finished " + count + " lines.");
                }

                GameRecord gameRecord = JsonConvert.DeserializeObject<GameRecord>(line);
                int lastEntity = 1000;
                foreach (StateKeyInfo stKeyInfo in gameRecord.playSec)
                {
                    PlayerKeyInfo p1Info = stKeyInfo.attackPlayer;
                    PlayerKeyInfo p2Info = stKeyInfo.defensePlayer;

                    bool isOwnTurn = p1Info.turn == 0 ? true : false;
                    lastEntity = stKeyInfo.nextEntity;
                    Playfield tempPf = null;
                    if (isOwnTurn)
                    {
                        tempPf = new Playfield(lastEntity, isOwnTurn, p1Info, p2Info);
                    }
                    else
                    {
                        tempPf = new Playfield(lastEntity, isOwnTurn, p2Info, p1Info);
                    }

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
                   
                    LinkedList<Action> actionSeq = searchValidSeq(tempPf, p1Info);
                    stKeyInfo.attackPlayer.canPlayHeroPower = new List<int>();
                    stKeyInfo.attackPlayer.playedActionJsonList = new List<PlayerKeyInfo.ActionKeyInfo>();
                    stKeyInfo.attackPlayer.handcardChange = new List<List<PlayerKeyInfo.CardKeyInfo>>();

                    foreach (Action action in actionSeq)
                    {
                        if (action.actionType == actionEnum.useHeroPower || action.actionType == actionEnum.playcard)
                        {
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
                            StateKeyInfo interState = new StateKeyInfo(tempPf.nextEntity, ownkeyInfo, enemykeyInfo, false, 0.0);
                            stKeyInfo.attackPlayer.handcardChange.Add(interState.attackPlayer.handcardJsonList);
                            bool canPlayHeroPower = mPlayer.ownAbilityReady && mPlayer.mana >= 2;
                            if (canPlayHeroPower)
                            {
                                stKeyInfo.attackPlayer.canPlayHeroPower.Add(1);
                            }
                            else
                            {
                                stKeyInfo.attackPlayer.canPlayHeroPower.Add(0);
                            }
                        }

                        Debug.Assert(stKeyInfo.attackPlayer.canPlayHeroPower.Count == stKeyInfo.attackPlayer.handcardChange.Count);
                        
                        tempPf.getNextEntity();
                        tempPf.doAction(action);
                        stKeyInfo.attackPlayer.playedActionJsonList.Add(new PlayerKeyInfo.ActionKeyInfo(action));
                        if (debug) action.print();
                    }
                    lastEntity = tempPf.getNextEntity() + 1;
                    //Console.WriteLine("===================");
                    //Console.WriteLine("End turn Next Entity:" + lastEntity);
                    //Console.WriteLine("===================");
                }
                count++;
                Helpfunctions.Instance.WriteResultToFileAbs(fileName + ".3.txt", JsonConvert.SerializeObject(gameRecord));
            }
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
                    bool canPlayHeroPower = mPlayer.ownAbilityReady && mPlayer.mana >= 2;
                    if (canPlayHeroPower)
                    {
                        stKeyInfo.attackPlayer.canPlayHeroPower.Add(1);
                    }
                    else
                    {
                        stKeyInfo.attackPlayer.canPlayHeroPower.Add(0);
                    }

                    //Console.WriteLine("===================");
                    //tempPf.printBoard();
                    //Console.WriteLine("Next Entity:" + tempPf.nextEntity);
                    //Console.WriteLine("===================");

                    foreach (PlayerKeyInfo.ActionKeyInfo actionKeyInfo in p1Info.playedActionJsonList)
                    {
                        Action action = CreateActionFromInfo(tempPf, actionKeyInfo);

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
                        if (action.own != null) actionKeyInfo.ownName = action.own.name.ToString();
                        if (action.target != null) actionKeyInfo.targetName = action.target.name.ToString();
                        if (action.actionType == actionEnum.playcard || action.actionType == actionEnum.useHeroPower)
                        {
                            if (stKeyInfo.attackPlayer.handcardChange == null) stKeyInfo.attackPlayer.handcardChange = new List<List<PlayerKeyInfo.CardKeyInfo>>();
                            stKeyInfo.attackPlayer.handcardChange.Add(interState.attackPlayer.handcardJsonList);

                            canPlayHeroPower = mPlayer.ownAbilityReady && mPlayer.mana >= 2;
                            if (action.actionType == actionEnum.useHeroPower)
                            {
                                Debug.Assert(canPlayHeroPower == false);
                                actionKeyInfo.cardName = "fireblast";
                            }
                            else
                            {
                                actionKeyInfo.cardName = action.card.card.name.ToString();
                            }

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
                Helpfunctions.Instance.WriteResultToFileAbs(fileName + ".2.txt", JsonConvert.SerializeObject(gameRecord));
            }
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                