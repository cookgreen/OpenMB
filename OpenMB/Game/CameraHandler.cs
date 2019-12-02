using OpenMB.Map;
using Mogre;
using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3 = Mogre.Vector3;
using OpenMB.Screen;
using Mogre_Procedural.MogreBites;
using OpenMB.Widgets;

namespace OpenMB.Game
{
    public enum CameraMode
    {
        Free,//Free Movement
        Follow,//Follow a Character
        Manual,//Manual control
		WorldMap,//World map movement
    }
    public class CameraHandler
    {
        private GameMap map;
        private Camera camera;
        private Vector3 cameraMovement;
        private CameraMode cameraMode;
        private CameraMode oldMode;
        private int currentAgentId;
        public CameraHandler(GameMap map, CameraMode cameraMode = CameraMode.Free)
        {
            this.map = map;
            camera = map.Camera;
            this.cameraMode = cameraMode;
            oldMode = cameraMode;
            currentAgentId = -1;
        }

        public void InjectMouseMove(MouseEvent arg)
        {
            if (cameraMode == CameraMode.Manual)
            {
                //ScreenManager.Instance.InjectMouseMove(arg);
                return;
            }
            Degree deCameraYaw = new Degree(arg.state.X.rel * -0.1f);
            camera.Yaw(deCameraYaw);
            Degree deCameraPitch = new Degree(arg.state.Y.rel * -0.1f);
            camera.Pitch(deCameraPitch);
        }

        public void InjectMousePressed(MouseEvent arg, MouseButtonID id)
        {
            if (id == MouseButtonID.MB_Left)
            {
                cameraMode = CameraMode.Follow;
                //Choose a character to follow
                if (currentAgentId != -1)
                {
                    camera = map.GetAgentById(currentAgentId).DetachCamera();
                }
                if (currentAgentId == -1)
                {
                    currentAgentId = 0;
                }
                else if (currentAgentId != map.Agents.Count - 1)
                {
                    currentAgentId++;
                }
                else
                {
                    currentAgentId = 0;
                }
                var agent = map.GetAgentById(currentAgentId);
                if (agent != null)
                {
                    agent.AttachCamera(camera);
                }
            }
        }

        public void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
        {

        }

        public void InjectKeyPressed(KeyEvent arg)
        {
            if (cameraMode == CameraMode.Manual)
            {
                //ScreenManager.Instance.InjectKeyPressed(arg);
            }
            else if (cameraMode == CameraMode.Follow &&
               (arg.key == KeyCode.KC_W ||
               arg.key == KeyCode.KC_A ||
               arg.key == KeyCode.KC_S ||
               arg.key == KeyCode.KC_D))
            {
                camera = map.GetAgentById(currentAgentId).DetachCamera();
                cameraMode = CameraMode.Free;
                currentAgentId = -1;
            }
            else
            {
                cameraMovement = new Vector3(0, 0, 0);
                switch (arg.key)
                {
                    case KeyCode.KC_A:
                        CameraMoveX(-10);
                        break;
                    case KeyCode.KC_D:
                        CameraMoveX(10);
                        break;
                    case KeyCode.KC_W:
                        CameraMoveZ(-10);
                        break;
                    case KeyCode.KC_S:
                        CameraMoveZ(10);
                        break;
                    case KeyCode.KC_Q:
                        CameraMoveY(-10);
                        break;
                    case KeyCode.KC_E:
                        CameraMoveY(10);
                        break;
                }
            }
        }

        public void CameraMoveX(float movement)
        {
            cameraMovement.x = movement;
        }
        public void CameraMoveY(float movement)
        {
            cameraMovement.y = movement;
        }
        public void CameraMoveZ(float movement)
        {
            cameraMovement.z = movement;
        }

        public void InjectKeyReleased(KeyEvent arg)
        {
            if (cameraMode == CameraMode.Manual)
            {
                //ScreenManager.Instance.InjectKeyReleased(arg);
            }
            cameraMovement = new Vector3(0, 0, 0);
        }

        public void Update(float timeSinceLastFrame)
        {
            if ((map.Agents == null ||
                map.Agents.Count == 0) &&
                cameraMode != CameraMode.Manual)
            {
                cameraMode = CameraMode.Free;
            }
            switch (cameraMode)
            {
                case CameraMode.Free:
                    MoveCamera();
                    break;
                case CameraMode.Follow:
                    var agent = map.GetAgentById(currentAgentId);
                    if (agent != null)
                    {
                        agent.UpdateCamera(timeSinceLastFrame);
                    }
                    break;
                case CameraMode.Manual:
                    break;
            }
            UIManager.Instance.RefreshCursor();
        }
        public void MoveCamera()
        {
            if (GameManager.Instance.keyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                camera.MoveRelative(cameraMovement);
            camera.MoveRelative(cameraMovement / 10);
        }

        public void ChangeMode(CameraMode newMode)
        {
            oldMode = cameraMode;
            cameraMode = newMode;
        }

        public void RestoreLastMode()
        {
            cameraMode = oldMode;
        }
    }
}
