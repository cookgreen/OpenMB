using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
    public enum MotionState
    {
        NOTHING,
        BLOCK_HAPPENED
    }

    public class MotionCheckResult
    {
        public MotionState ms { get; set; }
        public Motion MotionHappened { get; set; }

        public MotionCheckResult()
        {
            ms = MotionState.NOTHING;
            MotionHappened = null;
        }
    }

    public class MotionManager
    {
        private static MotionManager instance;
        public static MotionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MotionManager();
                }
                return instance;
            }
        }

        public MotionCheckResult CheckMotionColide(Motion motion1, Motion motion2)
        {
            MotionCheckResult motionCheckResult = new MotionCheckResult();

            string motion1Name = GetMotionNameBySkeletonAnimation(motion1.GetSkeletonAnimationName());
            string motion2Name = GetMotionNameBySkeletonAnimation(motion2.GetSkeletonAnimationName());

            if (motion1Name == "AttackUpper" && motion2Name == "BlockUpper")
            {
                motionCheckResult.ms = MotionState.BLOCK_HAPPENED;
                motionCheckResult.MotionHappened = motion2;
            }
            else if(motion1Name == "AttackRight" && motion2Name == "BlockRight")
            {
                motionCheckResult.ms = MotionState.BLOCK_HAPPENED;
                motionCheckResult.MotionHappened = motion2;
            }
            else if(motion1Name == "AttackLeft" && motion2Name == "BlockLeft")
            {
                motionCheckResult.ms = MotionState.BLOCK_HAPPENED;
                motionCheckResult.MotionHappened = motion2;
            }
            else if(motion1Name == "AttackCentral" && motion2Name == "BlockCentral")
            {
                motionCheckResult.ms = MotionState.BLOCK_HAPPENED;
                motionCheckResult.MotionHappened = motion2;
            }

            else if (motion2Name == "AttackUpper" && motion1Name == "BlockUpper")
            {
                motionCheckResult.ms = MotionState.BLOCK_HAPPENED;
                motionCheckResult.MotionHappened = motion2;
            }
            else if (motion2Name == "AttackRight" && motion1Name == "BlockRight")
            {
                motionCheckResult.ms = MotionState.BLOCK_HAPPENED;
                motionCheckResult.MotionHappened = motion1;
            }
            else if (motion2Name == "AttackLeft" && motion1Name == "BlockLeft")
            {
                motionCheckResult.ms = MotionState.BLOCK_HAPPENED;
                motionCheckResult.MotionHappened = motion1;
            }
            else if (motion2Name == "AttackCentral" && motion1Name == "BlockCentral")
            {
                motionCheckResult.ms = MotionState.BLOCK_HAPPENED;
                motionCheckResult.MotionHappened = motion1;
            }

            return motionCheckResult;
        }

        private string GetMotionNameBySkeletonAnimation(string skeletonAnimName)
        {
            return skeletonAnimName;
        }
    }
}
