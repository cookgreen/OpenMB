using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.Game.Controller
{
    public abstract class ManualControllerBase
    {
        protected string objectInstanceName;
        protected string objectMeshName;
        protected Camera objectCam;
        protected SceneNode objectSceneNode;
        protected Entity objectEntity;
        protected AnimationState[] objectAnims;
        protected int objectAnimNum;
        protected bool isInManualMode;

        public Entity Entity
        {
            get { return objectEntity; }
        }
        public string MeshName
        {
            get { return objectMeshName; }
        }
        public string Name
        {
            get { return objectInstanceName; }
        }

        public ManualControllerBase(string name, string meshName, Camera camera, bool manualMode = false)
        {
            objectInstanceName = name;
            objectMeshName = meshName;
            objectCam = camera;
            isInManualMode = manualMode;
            ControllerInit();
        }

        public bool ControllerInit()
        {
            objectEntity = objectCam.SceneManager.CreateEntity(objectInstanceName, objectMeshName);
            objectSceneNode = objectCam.SceneManager.RootSceneNode.CreateChildSceneNode();
            objectSceneNode.AttachObject(objectEntity);


            AnimationStateIterator animCollection =
                objectEntity.AllAnimationStates != null ? objectEntity.AllAnimationStates.GetAnimationStateIterator() : null;
            objectAnimNum = animCollection != null ? animCollection.Count() : 0;
            if (objectAnimNum > 0)
                objectAnims = new AnimationState[objectAnimNum];

            ControllerSetup();

            return true;
        }

        public abstract bool ControllerSetup();

        public abstract bool InjectMouseMoved(MouseEvent evt);

        public abstract bool InjectMousePressed(MouseEvent evt, MouseButtonID id);

        public abstract bool InjectMouseReleased(MouseEvent arg, MouseButtonID id);

        public abstract bool InjectKeyPressed(KeyEvent evt);

        public abstract bool InjectKeyReleased(KeyEvent evt);

        public bool ControllerUpdate(float deltaTime)
        {

            ControllerUpdateAnimations(deltaTime);
            if (isInManualMode)
            {
                ControllerUpdateCamera(deltaTime);
            }
            ControllerUpdateBody(deltaTime);
            return true;
        }

        public abstract bool ControllerUpdateAnimations(float deltaTime);

        public abstract bool ControllerUpdateCamera(float deltaTime);

        public abstract bool ControllerUpdateBody(float deltaTime);
    }
}
