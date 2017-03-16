using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{
    public class QLearningAgent : PlayerAgent{

    private static long   serialVersionUID    = 0L;

    private static bool      debug;

    // q-learning constants
    private static float  DISCOUNT_FACTOR     = 0.9f;
    private static float  LEARNING_RATE       = 0.0003f;
    private static float  EPSILON             = 0.03f;
    private static int    NUM_FEATURES        = 40;
    private static int    TRAINING_EPS        = 10;
    private static int    EVALUATION_EPS      = 5;

    // agent parameters
    private int                 maxEpisodes;            // max number of episodes to play
    private int                 numEpisodes;            // number of episodes played
    private int                 evalEpisodes;           // number of episodes left for evaluation
    private float               evalTotalReward;        // cumulative reward during evaluation
    private bool                freezeQ;                // do not update Q when set to true
    private List<float>         weights;                // weights of features to learn
    private float reward = 0;

    // episode parameters
    private float               episodeReward;          // current total reward for this episode
    //private QLState             lastQls;                // cache the last state for comparing to current
    private Playfield           lastState;
    private float               qLast;
    private bool playerSide;
    private TranspositionTable tt;

    public QLearningAgent(bool playerSide, Playfield p) {

        lastState = new Playfield(p);
        this.playerSide = playerSide;

        maxEpisodes = 100;
        // initialize agent parameters
        numEpisodes             = 0;
        evalEpisodes            = 0;
        evalTotalReward         = 0f;
        freezeQ                 = false;
        weights                 = new List<float>();
        for(int i = 0; i < NUM_FEATURES; i++)
            weights.Add((float)GameManager.getRNG().NextDouble() * 2f - 1f);    // set initial weights to random value between -1 and 1
            //weights.Add(0f);
        printWeights();
        normalizeWeights();

        qLast = Q(lastState);

        Helpfunctions.Instance.logg("Q-Learning Agent initialized for " + maxEpisodes + " episodes");
    }

    public override void updateState(Playfield playField)
    {
        lastState = new Playfield(playField);
        qLast = Q(lastState);
    }

    public void getAllpossibleStates(Playfield state, ref List<Playfield> statesList) 
    {
        List<Action> moves = Movegenerator.Instance.getMoveList(state, false, true, true, 0.0);
        if (moves.Count == 0)
        {
            if (tt.addToMap(state) == false)
            {
                statesList.Add(state);
            }
            return;
        }
        foreach (Action action in moves)
        {
            Playfield afterState = new Playfield(state);
            afterState.doAction(action);
            //if (tt.addToMap(afterState) == false)
            //{
            //    statesList.Add(afterState);
                getAllpossibleStates(afterState, ref statesList);
            //}
        }
    }

    public Playfield QStep()
    {
        GC.Collect();
        float maxQValue = Single.MinValue;
        Playfield bestState = lastState;
        //epsilon greedy             

        //List<Action> moves = Movegenerator.Instance.getMoveList(lastState, false, false, true);
        //int prevCount = lastState.playerSecond.ownMinions.Count;
        tt = new TranspositionTable();
        lastState.debugMinions();
        tt.addToMap(new Playfield(lastState));
        List<Playfield> moves = new List<Playfield>();
        //Playfield currentState = new Playfield(lastState).endTurn(false, false); //if need end turn
        //moves.Add(new Playfield(lastState));
        getAllpossibleStates(lastState, ref moves);

        Helpfunctions.Instance.logg("movesize = " + moves.Count);

        foreach (Playfield p in moves) {
            Helpfunctions.Instance.logg("===============P:hashkey = " + tt.getHashkey(p));
            p.printActions();
            p.printBoard();
        }

        if (moves.Count == 0)
        {
            return bestState;
        }

        if (GameManager.getRNG().NextDouble() < EPSILON)
        {
            bestState = moves[GameManager.getRNG().Next(moves.Count)];
        }
        else
        {
            foreach (Playfield posState in moves)
            {
                float QValue = Q(posState);
                if (QValue > maxQValue)
                {
                    maxQValue = QValue;
                    bestState = posState;
                    //if (afterState.playerSecond.ownMinions.Count == 0 && prevCount != 0 && afterState.playerFirst.ownMinions.Count != 0)//hardcode player second
                    //{
                    //    reward = 1;
                    //    Helpfunctions.Instance.logg("board reward received");
                    //}
                    if ((playerSide && bestState.getGameResult() == 0) || (!playerSide && bestState.getGameResult() == 1))
                    {
                        //reward = afterState.turnCounter;
                        reward = 1; //is it good?
                        Helpfunctions.Instance.logg("win reward received");
                    }
                    else
                    {
                        reward = 0;
                    }
                }
            }
        }

        //update weights
        float difference = reward + DISCOUNT_FACTOR * maxQValue - qLast;
        //if(debug) System.out.printf("%.5f\n", difference);
        List<float> features = getFeatures(lastState);
        //printFeatures(); //self play? 相减？ 检验正确性（update每一步打出来） tile coding？ binary？
        for (int j = 0; j < NUM_FEATURES; j++)
        {
            //if(debug) System.out.printf("w%d = %.5f + %.5f * %.5f * %.1f = ", i, weights.get(i), LEARNING_RATE, difference, features.get(i));
            weights[j] = weights[j] + LEARNING_RATE * difference * features[j];
            //if(debug) System.out.printf("%.5f\n", weights.get(i));
        }
        normalizeWeights();

        lastState = bestState;
        qLast = maxQValue;


        Helpfunctions.Instance.logg("best:");
        bestState.printActions();

        return bestState;

    }

    public override Action getMove()
    {
        float maxQValue = Single.MinValue;
        Action bestAction = null;
        Playfield bestState = null;
        //epsilon greedy             

        List<Action> moves = Movegenerator.Instance.getMoveList(lastState, false, true, true, 0.0);
        //int prevCount = lastState.playerSecond.ownMinions.Count;

        if (moves.Count == 0)
        {
            return bestAction;
        }

        if (GameManager.getRNG().NextDouble() < EPSILON)
        {
            bestAction = moves[GameManager.getRNG().Next(moves.Count)];
        }
        else
        {
            foreach (Action action in moves)
            {
                Playfield afterState = new Playfield(lastState);
                afterState.doAction(action);
                float QValue = Q(afterState);
                if (QValue > maxQValue)
                {
                    maxQValue = QValue;
                    bestAction = action;
                    bestState = afterState;
                    //if (afterState.playerSecond.ownMinions.Count == 0 && prevCount != 0 && afterState.playerFirst.ownMinions.Count != 0)//hardcode player second
                    //{
                    //    reward = 1;
                    //    Helpfunctions.Instance.logg("board reward received");
                    //}
                    if ((playerSide && afterState.getGameResult() == 0) || (!playerSide && afterState.getGameResult() == 1))
                    {
                        //reward = afterState.turnCounter;
                        reward = 1; //is it good?
                        Helpfunctions.Instance.logg("win reward received");
                    }
                    else {
                        reward = 0;
                    }
                }
            }
        }

        //update weights
        float difference = reward + DISCOUNT_FACTOR * maxQValue - qLast;
        //if(debug) System.out.printf("%.5f\n", difference);
        List<float> features = getFeatures(lastState);
        //printFeatures(); //self play? 相减？ 检验正确性（update每一步打出来） tile coding？ binary？
        lastState.debugMinions();
        for (int j = 0; j < NUM_FEATURES; j++)
        {
            //if(debug) System.out.printf("w%d = %.5f + %.5f * %.5f * %.1f = ", i, weights.get(i), LEARNING_RATE, difference, features.get(i));
            weights[j] = weights[j] + LEARNING_RATE * difference * features[j];
            //if(debug) System.out.printf("%.5f\n", weights.get(i));
        }
        normalizeWeights();

        lastState = bestState;
        qLast = maxQValue;

        return bestAction;
    }

    public void QLearning(){
    
        for (int i = 0; i < maxEpisodes; i++){
            
            //lastState = new Playfield(initState);
            lastState.drawInitCards();
            qLast = Q(lastState);

            int score = lastState.getGameResult();
            while (score == -1)
            {
                List<Action> moves = Movegenerator.Instance.getMoveList(lastState, false, true, true, 0.0);

                if (moves.Count == 0)
                {
                    lastState.endTurn(false, false);
                    lastState.drawTurnStartCard();
                }
                else {
                    float maxQValue = 0;
                    Action bestAction = null;
                    Playfield bestState = null;    
                    //psilon greedy             
                    if (GameManager.getRNG().NextDouble() < EPSILON)
                    {
                        bestAction = moves[GameManager.getRNG().Next(moves.Count)];
                    }else{
                        foreach (Action action in moves){
                            Playfield afterState = new Playfield(lastState);
                            afterState.doAction(action);                 
                            float QValue = Q(afterState);
                            if (QValue > maxQValue){
                                maxQValue = QValue;
                                bestAction = action;
                                bestState = afterState;
                            }
                        }
                    }

                    //update weights
                    float difference = reward + DISCOUNT_FACTOR * maxQValue - qLast;
                    //if(debug) System.out.printf("%.5f\n", difference);
                    List<float> features = getFeatures(lastState);
                   
                    for(int j = 0; j < NUM_FEATURES; j++) {
                        //if(debug) System.out.printf("w%d = %.5f + %.5f * %.5f * %.1f = ", i, weights.get(i), LEARNING_RATE, difference, features.get(i));
                        weights[j] = weights[j] + LEARNING_RATE * difference * features[j];
                        //if(debug) System.out.printf("%.5f\n", weights.get(i));
                    }
                    normalizeWeights();

                    lastState = bestState;
                    qLast = maxQValue;
                }
               
                score = lastState.getGameResult();
            }
        }
    
    }


    //public Map<Integer, Action> initialStep(StateView state, HistoryView stateHistory) {
    //    // initialize episode parameters
    //    episodeReward = 0f;

    //    // initialize step parameters
    //    QLState qls = new QLState(state);

    //    if(debug) System.out.printf("Initial step:\n");

    //    // analyze the state and determine next action for each footman
    //    for(QLUnit f: qls.getFriendly()) {
    //        int fid = f.getID();
    //        // choose a random target with probability EPSILON
    //        if(rng.nextFloat() < EPSILON) qls.setRandomTargetFor(fid);
    //        // otherwise choose greedy option (target footman with best Q score for current policy)
    //        else qls.setTarget(fid, getQMaxTarget(qls, fid).b);
    //    }

    //    lastQls = qls;                                  // cache this state
    //    if(debug) qls.print();

    //    return translateActions(qls);
    //}

    //public Map<Integer, Action> middleStep(StateView state, HistoryView stateHistory) {
    //    // initialize step parameters
    //    Map<Integer, Action> actions = new HashMap<Integer, Action>();
    //    Map<Integer, Integer> bestTargets = new HashMap<Integer, Integer>();
    //    int step = state.getTurnNumber();
    //    QLState qls = new QLState(state);
    //    // get the combat logs for the last step
    //    List<DamageLog> damageLogs = stateHistory.getDamageLogs(step - 1);
    //    List<DeathLog> deathLogs = stateHistory.getDeathLogs(step - 1);
    //    // mark that event has occurred if any friendly units took damage or enemies died
    //    bool eventOccurred = false;
    //    for(DamageLog log: damageLogs) {
    //        if(log.getDefenderController() == 0) {
    //            eventOccurred = true;
    //            break;
    //        }
    //    }

    //    if(!eventOccurred) {
    //        for(DeathLog log: deathLogs) {
    //            if(log.getController() == 1) {
    //                eventOccurred = true;
    //                break;
    //            }
    //        }
    //    }

    //    if(debug) System.out.printf("\nStep %d:\n", step);

    //    // decompose reward for each footman and determine best target for next move
    //    for(QLUnit f: lastQls.getFriendly()) {
    //        bool died = false;                       // did the friendly footman die this turn?
    //        int fid = f.getID();                        // friendly footman id
    //        int tid = lastQls.getTargetFor(fid);        // enemy target id

    //        float reward = -0.1f;                       // each step costs -0.1

    //        if(debug) System.out.printf("Reward for ATTACK(%d,%d) = %.1f", fid, tid, reward);

    //        // check for damage dealt / taken between fid and tid
    //        for(DamageLog log: damageLogs) {
    //            if(log.getAttackerID() == fid) {
    //                reward += (float)log.getDamage();   // damage dealt rewards +damage
    //                if(debug) System.out.printf(" + %d", log.getDamage());
    //            }
    //            if(log.getDefenderID() == fid) {
    //                reward -= (float)log.getDamage();   // damage taken costs -damage
    //                if(debug) System.out.printf(" - %d", log.getDamage());
    //            }
    //        }

    //        // check for death of fid and tid
    //        for(DeathLog log: deathLogs) {
    //            if(log.getDeadUnitID() == tid) {
    //                reward += 100f;                     // killing enemy rewards +100
    //                if(debug) System.out.print(" + 100");
    //            }
    //            if(log.getDeadUnitID() == fid) {
    //                reward -= 100f;                     // dying costs -100
    //                died = true;
    //                if(debug) System.out.print(" - 100");
    //            }
    //        }

    //        if(debug) System.out.printf(" = %.1f\n", reward);

    //        episodeReward += reward;                    // accumulate reward

    //        float qLast = Q(lastQls, fid, tid);
    //        float qMax;
    //        int bestTarget;

    //        if(died) {
    //            qMax = qLast;
    //        } else {
    //            // identify best targets for next move
    //            Pair<Float, Integer> qMaxTarget = getQMaxTarget(qls, fid);
    //            qMax = qMaxTarget.a;
    //            bestTarget = qMaxTarget.b;
    //            bestTargets.put(fid, bestTarget);
    //        }

    //        // update the Q function with feedback
    //        if(!freezeQ) {
    //            if(debug) System.out.printf("Difference = %.1f + %.1f * %.3f - %.3f = ", reward, DISCOUNT_FACTOR, qMax, qLast);
    //            float difference = reward + DISCOUNT_FACTOR * qMax - qLast;
    //            if(debug) System.out.printf("%.5f\n", difference);
    //            List<Float> features = getFeatures(lastQls, fid, tid);
    //            // update weights for each feature
    //            for(int i = 0; i < NUM_FEATURES; i++) {
    //                if(debug) System.out.printf("w%d = %.5f + %.5f * %.5f * %.1f = ", i, weights.get(i), LEARNING_RATE, difference, features.get(i));
    //                weights.set(i, weights.get(i) + LEARNING_RATE * difference * features.get(i));
    //                if(debug) System.out.printf("%.5f\n", weights.get(i));
    //            }
    //            normalizeWeights();
    //        }
    //    }

    //    if(eventOccurred) {                             // reallocate targets at event point
    //        for(QLUnit f: qls.getFriendly()) {
    //            int fid = f.getID();
    //            // choose a random target with probability EPSILON
    //            if(rng.nextFloat() < EPSILON) qls.setRandomTargetFor(fid);
    //            // otherwise choose greedy option (target footman with best Q score for current policy)
    //            else qls.setTarget(fid, bestTargets.get(fid));
    //        }

    //        actions = translateActions(qls);            // generate actions
    //    } else {                                        // otherwise maintain same targets
    //        qls.copyTargetsFrom(lastQls);
    //    }

    //    lastQls = qls;                                  // update last state
    //    if(debug) qls.print();

    //    return actions;
    //}

    //@Override
    //public void terminalStep(StateView state, HistoryView stateHistory) {
    //    if(evalEpisodes == 0) {
    //        if(debug) {
    //            System.out.printf("Completed episode %d with reward %.1f\n", numEpisodes, episodeReward);
    //            printWeights();
    //        } else {
    //            System.out.print("|");
    //        }
    //        numEpisodes++;
    //        if(numEpisodes % TRAINING_EPS == 0) {       // finished training
    //            freezeQ = true;                         // stop Q from updating
    //            evalEpisodes = EVALUATION_EPS;          // evaluate for next EVALUATION_EPS episodes
    //            evalTotalReward = 0;                    // reset cumulative reward
    //        }
    //    } else {                                        // evaluation
    //        if(debug) System.out.printf("Completed evaluation episode with reward %.1f\n", episodeReward);
    //        else System.out.print("*");
    //        evalTotalReward += episodeReward;           // accumulate reward
    //        if(--evalEpisodes == 0) {                   // finished evaluating
    //            System.out.println();
    //            freezeQ = false;
    //            float avgReward = evalTotalReward / EVALUATION_EPS;
    //            System.out.printf("Episodes Played: %d, Average Reward: %.1f\n", numEpisodes, avgReward);
    //            if(debug) printWeights();
    //            if(numEpisodes == maxEpisodes) System.exit(0);
    //        }
    //    }
    //}

    // returns a list of feature values for a = ATTACK(fid, eid)
    private List<float> getFeatures(Playfield p) {
        Player mPlayer, ePlayer;
        if (playerSide){
            mPlayer = p.playerFirst;
            ePlayer = p.playerSecond;
        }else{
            mPlayer = p.playerSecond;
            ePlayer = p.playerFirst;
        }
        List<float> features = new List<float>();
        //Minions
                //public float getPlayfieldValue(Playfield p, bool own)

        features.Add((mPlayer.ownHero.Hp + mPlayer.ownHero.armor)/30);
        //features.Add(mPlayer.ownMaxMana);
        //features.Add(mPlayer.spellpower);
        features.Add((mPlayer.ownHero.frozen)? 0: mPlayer.ownWeaponAttack);

        //int minionCount = 0;
        int TwomimionCount = 0;
        int ThreemimionCount = 0;
        int FourminionCount = 0;
        int FiveSixminionCount = 0;
        int SevenminionCount = 0;
        int manawyrm = 0;
        int sorcerer = 0;
        int flamewaker = 0;
        int archmage = 0;
        int madscientist = 0;
        int drake = 0;
        int foereaper = 0;
        int ragnaros = 0;
        int deathrattle = 0;
        int special = 0;
        float minionValue = 0;
        foreach (Minion m in mPlayer.ownMinions)
        {
            minionValue += (m.Angr != 0) ? (float)Math.Sqrt(m.Angr) * m.Hp : 0.5f * m.Hp;
            if (m.silenced) continue;

            switch (m.Hp) {

                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    TwomimionCount++;
                    break;
                case 3:
                    ThreemimionCount++; 
                    break;
                case 4:
                    FourminionCount++;
                    break;
                case 5:
                    FiveSixminionCount++;
                    break;
                case 6:
                    FiveSixminionCount++;
                    break;
                default:
                    SevenminionCount++;
                    break;
            
            }

            switch (m.name)
            {

                case CardDB.cardName.manawyrm:
                    manawyrm++;
                    break;
                case CardDB.cardName.sorcerersapprentice:
                    sorcerer++;
                    break;
                case CardDB.cardName.flamewaker:
                    flamewaker++;
                    break;
                case CardDB.cardName.archmageantonidas:
                    archmage++;
                    break;
                case CardDB.cardName.madscientist:
                    madscientist++;
                    break;
                case CardDB.cardName.azuredrake:
                    drake++;
                    break;
                case CardDB.cardName.foereaper4000:
                    foereaper++;
                    break;
                case CardDB.cardName.ragnarosthefirelord:
                    ragnaros++;
                    break;

            }

            if (m.handcard.card.deathrattle)
                deathrattle++;

            if (m.divineshild || m.stealth || m.taunt) {
                special++;
            }
            //features.Add(m.Hp);
            //features.Add(m.Angr);
            //features.Add((m.silenced)? 0: Convert.ToSingle(m.handcard.card.isSpecialMinion));
            //features.Add(Convert.ToSingle(m.taunt));
            //features.Add(Convert.ToSingle(m.stealth));
            //features.Add(Convert.ToSingle(m.divineshild));
            //minionCount++;
        }

        //features.Add(minionCount);
        features.Add(TwomimionCount);
        features.Add(ThreemimionCount);
        features.Add(FourminionCount);
        features.Add(FiveSixminionCount);
        features.Add(SevenminionCount);
        features.Add(manawyrm);
        features.Add(sorcerer);
        features.Add(flamewaker);
        features.Add(archmage);
        features.Add(madscientist);
        features.Add(drake);
        features.Add(foereaper);
        features.Add(ragnaros);
        features.Add(deathrattle);
        features.Add(special);
        features.Add(minionValue);

        float handMinionValue = 0f;
        float handSpellValue = 0f;

        foreach (Handmanager.Handcard hc in mPlayer.owncards)
        {
            int manacost = hc.getManaCost(p, playerSide);
            if (manacost > mPlayer.mana + 1) continue;
            if (hc.card.type == CardDB.cardtype.MOB)
            {
                handMinionValue += (hc.card.Attack != 0) ? (float)Math.Sqrt(hc.card.Attack) * hc.card.Health : 0.5f * hc.card.Health;
            }
            if (hc.card.type == CardDB.cardtype.SPELL)
            {
                handSpellValue += hc.card.cost;
            }
        }

        features.Add(handMinionValue);
        features.Add(handSpellValue);

        //int emptySlotCount = 7 - minionCount;
        //for (int i = 0; i < emptySlotCount * 6; i++){
        //    features.Add(0);
        //}

        //int mobsInHand = 0;
        //int bigMobsInHand = 0;
        //foreach (Handmanager.Handcard hc in mPlayer.owncards)
        //{
        //    if (hc.card.type == CardDB.cardtype.MOB)
        //    {
        //         mobsInHand++;
        //         if (hc.card.Attack >= 3) bigMobsInHand++;
        //    }
        //}

        //features.Add(mobsInHand);
        //features.Add(bigMobsInHand);
        //features.Add(mPlayer.owncards.Count);

        features.Add(ePlayer.ownHero.Hp + ePlayer.ownHero.armor);
        //features.Add(ePlayer.ownMaxMana);
        //features.Add(ePlayer.spellpower);
        features.Add((ePlayer.ownHero.frozen)? 0: ePlayer.ownWeaponAttack);

        //minionCount = 0;
        //foreach (Minion m in ePlayer.ownMinions)
        //{
        //    features.Add(m.Hp);
        //    features.Add(m.Angr);
        //    features.Add((m.silenced)? 0: Convert.ToSingle(m.handcard.card.isSpecialMinion));
        //    features.Add(Convert.ToSingle(m.taunt));
        //    features.Add(Convert.ToSingle(m.stealth));
        //    features.Add(Convert.ToSingle(m.divineshild));
        //    minionCount++;
        //}

        //emptySlotCount = 7 - minionCount;
        //for (int i = 0; i < emptySlotCount * 6; i++){
        //    features.Add(0);
        //}

        //mobsInHand = 0;
        //bigMobsInHand = 0;
        //foreach (Handmanager.Handcard hc in ePlayer.owncards)
        //{
        //    if (hc.card.type == CardDB.cardtype.MOB)
        //    {
        //         mobsInHand++;
        //         if (hc.card.Attack >= 3) bigMobsInHand++;
        //    }
        //}

        //features.Add(mobsInHand);
        //features.Add(bigMobsInHand);
        //features.Add(ePlayer.owncards.Count);

         //minionCount = 0;
         TwomimionCount = 0;
         ThreemimionCount = 0;
         FourminionCount = 0;
         FiveSixminionCount = 0;
         SevenminionCount = 0;
         manawyrm = 0;
         sorcerer = 0;
         flamewaker = 0;
         archmage = 0;
         madscientist = 0;
         drake = 0;
         foereaper = 0;
         ragnaros = 0;
         deathrattle = 0;
         special = 0;
         minionValue = 0;
        foreach (Minion m in ePlayer.ownMinions)
        {
            minionValue += (m.Angr != 0) ? m.Angr * (float)Math.Sqrt(m.Hp) : 0.25f * (float)Math.Sqrt(m.Hp);
            if (m.silenced) continue;

            switch (m.Hp)
            {

                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    TwomimionCount++;
                    break;
                case 3:
                    ThreemimionCount++;
                    break;
                case 4:
                    FourminionCount++;
                    break;
                case 5:
                    FiveSixminionCount++;
                    break;
                case 6:
                    FiveSixminionCount++;
                    break;
                default:
                    SevenminionCount++;
                    break;

            }

            switch (m.name)
            {

                case CardDB.cardName.manawyrm:
                    manawyrm++;
                    break;
                case CardDB.cardName.sorcerersapprentice:
                    sorcerer++;
                    break;
                case CardDB.cardName.flamewaker:
                    flamewaker++;
                    break;
                case CardDB.cardName.archmageantonidas:
                    archmage++;
                    break;
                case CardDB.cardName.madscientist:
                    madscientist++;
                    break;
                case CardDB.cardName.azuredrake:
                    drake++;
                    break;
                case CardDB.cardName.foereaper4000:
                    foereaper++;
                    break;
                case CardDB.cardName.ragnarosthefirelord:
                    ragnaros++;
                    break;

            }

            if (m.handcard.card.deathrattle)
                deathrattle++;

            if (m.divineshild || m.stealth || m.taunt)
            {
                special++;
            }
            //features.Add(m.Hp);
            //features.Add(m.Angr);
            //features.Add((m.silenced)? 0: Convert.ToSingle(m.handcard.card.isSpecialMinion));
            //features.Add(Convert.ToSingle(m.taunt));
            //features.Add(Convert.ToSingle(m.stealth));
            //features.Add(Convert.ToSingle(m.divineshild));
            //minionCount++;
        }

        //features.Add(minionCount);
        features.Add(TwomimionCount);
        features.Add(ThreemimionCount);
        features.Add(FourminionCount);
        features.Add(FiveSixminionCount);
        features.Add(SevenminionCount);
        features.Add(manawyrm);
        features.Add(sorcerer);
        features.Add(flamewaker);
        features.Add(archmage);
        features.Add(madscientist);
        features.Add(drake);
        features.Add(foereaper);
        features.Add(ragnaros);
        features.Add(deathrattle);
        features.Add(special);
        features.Add(minionValue);

        handMinionValue = 0f;
        handSpellValue = 0f;

        foreach (Handmanager.Handcard hc in ePlayer.owncards)
        {
            int manacost = hc.getManaCost(p, playerSide);
            if (manacost > ePlayer.mana) continue;
            if (hc.card.type == CardDB.cardtype.MOB)
            {
                handMinionValue += (hc.card.Attack != 0) ? hc.card.Attack * (float)Math.Sqrt(hc.card.Health) : 0.25f * (float)Math.Sqrt(hc.card.Health);
            }
            if (hc.card.type == CardDB.cardtype.SPELL)
            {
                handSpellValue += hc.card.cost;
            }
        }

        features.Add(handMinionValue);
        features.Add(handSpellValue);
        
        return features;

    }

    // normalize weights
    private void normalizeWeights() {
        float total = 0;
        foreach(float w in weights){
            total += w;
        }
        for(int i = 0; i < NUM_FEATURES; i++){
            weights[i] = weights[i] / total;
        }
    }

    // evaluate linear approximation Q function for s = qls and a = ATTACK(fid, eid)
    private float Q(Playfield afterState) {
        float q = 0;
        List<float> features = getFeatures(afterState);

        // w0 + w * f(s, a)
        for(int i = 0; i < NUM_FEATURES; i++)
            q += weights[i] * features[i];

        return q;
    }

    public void printWeights() {
        string weightString = "[";
        for (int i = 0; i < NUM_FEATURES; i++)
        {
            if (i == NUM_FEATURES - 1)
            {
                weightString += weights[i] + "]";
            }
            else
            {
                weightString += weights[i] + ",";
            }
        }
        Helpfunctions.Instance.logg("weight");
        Helpfunctions.Instance.logg(weightString);
    }

    }
}
