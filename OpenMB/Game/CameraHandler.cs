using OpenMB.Map;
using Mogre;
using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3 = Mogre.Vector3;

namespace OpenMB.Game
{
    public enum CameraState
    {
        Free,//Free Movement
        Follow,//Follow a Character
    }
    public class CameraHandler
    {
        private GameMap map;
        private Camera camera;
        private Vector3 cameraMovement;
        private CameraState cameraState;
        private int currentAgentId;
        public CameraHandler(GameMap map, CameraState cameraState = CameraState.Free)
        {
            this.map = map;
            camera = map.Camera;
            this.cameraState = cameraState;
            currentAgentId = -1;
        }

        public void InjectMouseMove(MouseEvent arg)
        {
            Degree deCameraYaw = new Degree(arg.state.X.rel * -0.1f);
            camera.Yaw(deCameraYaw);
            Degree deCameraPitch = new Degree(arg.state.Y.rel * -0.1f);
            camera.Pitch(deCameraPitch);
        }

        public void InjectMousePressed(MouseEvent arg, MouseButtonID id)
        {
            if (id == MouseButtonID.MB_Left)
            {
                cameraState = CameraState.Follow;
                //Choose a character to follow
                if (currentAgentId != -1)
                {
                    camera = map.GetAgentById(currentAgentId).DetachCamera();
                }
                if (currentAgentId == -1)
                {
                    currentAgentId = 0;
                }
                else if (currentAgentId != map.GetAgents().Count - 1)
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
            if (cameraState == CameraState.Follow &&
               (arg.key == KeyCode.KC_W || arg.key == KeyCode.KC_A || arg.key == KeyCode.KC_S || arg.key == KeyCode.KC_D))
            {
                camera = map.GetAgentById(currentAgentId).DetachCamera();
                cameraState = CameraState.Free;
                currentAgentId = -1;
            }
            else
            {
                cameraMovement = new Vector3(0, 0, 0);
                switch (arg.key)
                {
                    case KeyCode.KC_A:
                        cameraMovement.x = -10;
                        break;
                    case KeyCode.KC_D:
                        cameraMovement.x = 10;
                        break;
                    case KeyCode.KC_W:
                        cameraMovement.z = -10;
                        break;
                    case KeyCode.KC_S:
                        cameraMovement.z = 10;
                        break;
                    case KeyCode.KC_Q:
                        cameraMovement.y = -10;
                        break;
                    case KeyCode.KC_E:
                        cameraMovement.y = 10;
                        break;
                }
            }
        }

        public void InjectKeyReleased(KeyEvent arg)
        {
            cameraMovement = new Vector3(0, 0, 0);
        }

        public void Update(float timeSinceLastFrame)
        {
            if (map.GetAgents() == null || map.GetAgents().Count == 0)
            {
                cameraState = CameraState.Free;
            }
            switch (cameraState)
            {
                case CameraState.Free:
                    moveCamera();
                    break;
                case CameraState.Follow:
                    var agent = map.GetAgentById(currentAgentId);
                    if (agent != null)
                    {
                        agent.UpdateCamera(timeSinceLastFrame);
                    }
                    break;
            }
            GameManager.Instance.trayMgr.refreshCursor();
        }
        private void moveCamera()
        {
            if (GameManager.Instance.keyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                camera.MoveRelative(cameraMovement);
            camera.MoveRelative(cameraMovement / 10);
        }
    }
}
