using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.AI
{
    public enum FSMStates { Dead, Idle, Attack, Patrol, Flee }; // states
    public enum Event { Die, Idle, SpotEnemy, Patrol, Flee };
    public interface CharacterFSM
    {
        void processEvent(Event e);
        //{
        //    States oldState = currentState;
        //    bool pass = false;
        //    switch (currentState)
        //    {
        //        case States.Idle:
        //            if (e == Event.Die)
        //            {
        //                //R.I.P
        //                currentState = States.Dead;
        //            }
        //            else if(e== Event.Flee)
        //            {
        //                //fall back
        //                currentState = States.Flee;
        //            }
        //            else if (e == Event.SpotEnemy)
        //            {
        //                //enemy spotted
        //                currentState = States.Attack;
        //            }
        //            else if (e == Event.Patrol)
        //            {
        //                currentState = States.Patrol;
        //            }
        //            break;
        //        case States.Attack:
        //            if (e == Event.Die)
        //            {
        //                //R.I.P
        //                currentState = States.Dead;
        //            }
        //            else if (e == Event.Flee)
        //            {
        //                //fall back
        //                currentState = States.Flee;
        //            }
        //            else if (e == Event.Idle)
        //            {
        //                //rest time
        //                currentState = States.Idle;
        //            }
        //            else if (e == Event.Patrol)
        //            {
        //                currentState= States.Patrol;
        //            }
        //            break;
        //        case States.Dead:
        //            if (e == Event.SpotEnemy)
        //            {
        //                currentState = States.Attack;
        //            }
        //            else if (e == Event.Flee)
        //            {
        //                //fall back
        //                currentState = States.Flee;
        //            }
        //            else if (e == Event.Idle)
        //            {
        //                //enemy spotted
        //                currentState = States.Idle;
        //            }
        //            else if (e == Event.Patrol)
        //            {
        //                currentState= States.Patrol;
        //            }
        //            break;
        //        case States.Flee:
        //            if (e == Event.SpotEnemy)
        //            {
        //                //enemy spotted
        //                currentState = States.Attack;
        //            }
        //            else if (e == Event.Die)
        //            {
        //                //R.I.P
        //                currentState = States.Dead;
        //            }
        //            else if (e == Event.Idle)
        //            {
        //                //rest time
        //                currentState = States.Idle;
        //            }
        //            else if (e == Event.Patrol)
        //            {
        //                currentState = States.Patrol;
        //            }
        //            break;
        //        case States.Patrol:
        //            if (e == Event.Die)
        //            {
        //                //R.I.P
        //                currentState = States.Dead;
        //            }
        //            else if (e == Event.Flee)
        //            {
        //                //fall back
        //                currentState = States.Flee;
        //            }
        //            else if (e == Event.SpotEnemy)
        //            {
        //                //enemy spotted
        //                currentState = States.Attack;
        //            }
        //            else if (e == Event.Idle)
        //            {
        //                //rest time
        //                currentState = States.Idle;
        //            }
        //            break;
        //    }
        //
        //    if (oldState == currentState && pass == false)
        //        return;
        //    switch (currentState)
        //    {
        //        case States.Idle:
        //            enterIdle();
        //            break;
        //        case States.Attack:
        //            enterAttack();
        //            break;
        //        case States.Dead:
        //            enterDead();
        //            break;
        //        case States.Flee:
        //            enterFlee();
        //            break;
        //        case States.Patrol:
        //            enterPatrol();
        //            break;
        //    }
        //}

         void enterDead();
         void enterIdle();
         void enterAttack();
         void enterPatrol();
         void enterFlee();

         void Action(Event e);
    }
}
