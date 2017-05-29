using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine
{
    class CharacterListener
    {
        protected Character mChar = null;
		protected ExCamera mExCamera = null;
		public ExCamera.Mode mMode;

		public CharacterListener()
		{			
			mChar = null;
			mExCamera = null;
			mMode = 0;
		}

		public void setCharacter (Character character) 
		{
			mChar = character;
		}

		public void setExtendedCamera (ExCamera cam) 
		{
			mExCamera = cam;
		}

		public void setCameraMode(ExCamera.Mode camereraMode)
		{
			this.mMode = camereraMode;
		}

        public bool Update(float timeSinceLastFrame)
        {
            if (mChar != null)
            {
                mChar.update(timeSinceLastFrame);

                if (mExCamera != null)
                {
                    switch (mMode)
                    {
                        case ExCamera.Mode.Chasing: // 3rd person chase
                            mExCamera.update(timeSinceLastFrame,
                                mChar.getCameranode()._getDerivedPosition(),
                                mChar.getSightNode()._getDerivedPosition());
                            break;
                        case ExCamera.Mode.Fixed: // 3rd person fixed
                            mExCamera.update(timeSinceLastFrame,
                                new Vector3(0, 200, 0),
                                mChar.getSightNode()._getDerivedPosition());
                            break;
                        case ExCamera.Mode.FirstPerson: // 1st person
                            mExCamera.update(timeSinceLastFrame,
                                mChar.getWorldPosition(),
                                //new Math3D.Vector3(mChar.getWorldPosition().x+5,mChar.getWorldPosition().y+15,mChar.getWorldPosition().z),
                                mChar.getSightNode()._getDerivedPosition());
                            break;
                    }
                }
            }

            return true;
        }
    }
}
