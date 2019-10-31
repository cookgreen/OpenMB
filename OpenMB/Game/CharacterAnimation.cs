using Mogre;
using OpenMB.Mods.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
	public enum AnimPlayType
	{
		/// <summary>
		/// Play as base animation
		/// </summary>
		BASE,
		/// <summary>
		/// Play as top animation
		/// </summary>
		TOP,
		/// <summary>
		/// Play as random animation
		/// </summary>
		FULL,
		/// <summary>
		/// Play a time dura animation
		/// </summary>
		TIME,
		/// <summary>
		/// Play a loop animation
		/// </summary>
		LOOP,
		/// <summary>
		/// Play this animation firstly
		/// </summary>
		START,
		/// <summary>
		/// Play this animation lastly
		/// </summary>
		END,
	}

	public enum ChaAnimType
    {
		/// <summary>
		/// No animation
		/// </summary>
        CAT_NONE,
		/// <summary>
		/// Character is idle
		/// </summary>
		CAT_IDLE,
		/// <summary>
		/// Character is running
		/// </summary>
		CAT_RUN,
		/// <summary>
		/// Character is jumping
		/// </summary>
		CAT_JUMP,
		/// <summary>
		/// Character's hands relaxed
		/// </summary>
		CAT_HANDS_RELAXED,
		/// <summary>
		/// Character's hands closed
		/// </summary>
		CAT_HANDS_CLOSED,
		CAT_CUSTOME,
		CAT_CUSTOME_BLOCK,//This animation can block the character's action

		CAT_IDLE_TOP,
        CAT_IDLE_BASE,
		CAT_RUN_TOP,
        CAT_RUN_BASE,
        CAT_JUMP_START,
        CAT_JUMP_LOOP,
        CAT_JUMP_END,
    }
    public class CharacterAnimation
    {
        private string name;
        private ChaAnimType type;
        private AnimationState animationState;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public AnimationState AnimationState
        {
            get
            {
                return animationState;
            }
        }

        public ChaAnimType Type
        {
            get
            {
                return type;
            }
        }

        public bool IsValid
        {
            get
            {
                return animationState != null;
            }
        }

        public CharacterAnimation(string name, AnimationState animationState, ChaAnimType type)
        {
            this.name = name;
            this.animationState = animationState;
            this.type = type;
        }

		public void Play()
		{
		}

		public void Update(float timeSinceLastFrame)
		{
			animationState.AddTime(timeSinceLastFrame);
		}
    }
}
