using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;

namespace AMOFGameEngine.Data
{
    public class CharacterManager
    {
        List<Character> characters;
        Dictionary<string, Entity> charaEntMap;
        Camera cam;
        AnimationState animState;
        Keyboard keyboard;
        Mouse mouse;
        Mogre.Vector3 moveOffset;
        float lastRotateAngle;

        public CharacterManager(Camera cam,Keyboard keyboard,Mouse mouse)
        {
            this.cam = cam;
            this.keyboard = keyboard;
            this.mouse = mouse;
            raySceneQuery = cam.SceneManager.CreateRayQuery(new Ray());
            charaEntMap = new Dictionary<string, Entity>();
            characters = new List<Character>();
            moveOffset = new Mogre.Vector3();
            Root.Singleton.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
        }

        bool FrameStarted(FrameEvent evt)
        {
            foreach (KeyValuePair<string, Entity> kpl in charaEntMap)
            {
                kpl.Value.GetAnimationState("IdleTop").AddTime(evt.timeSinceLastFrame);
                kpl.Value.GetAnimationState("IdleBase").AddTime(evt.timeSinceLastFrame);
            }
            return true;
        }

        public void Initization()
        {

        }

        public void SpawnCharacter(Mogre.Vector3 spawnPos,string characterID)
        {
            Character chara= characters.Where(o => o.CharaID == characterID).First();
            if (chara != null)
            {
                Entity charaEnt = cam.SceneManager.CreateEntity(characterID, chara.CharaMeshName + ".mesh");
                SceneNode charaNode = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
                charaNode.AttachObject(charaEnt);
                charaNode.SetPosition(spawnPos.x, spawnPos.y, spawnPos.z);

                animState = charaEnt.GetAnimationState("IdleBase");
                animState.Loop = true;

                animState = charaEnt.GetAnimationState("IdleTop");
                animState.Loop = true;

                charaEntMap.Add(characterID, charaEnt);
            }
            chara.CharaState = CharacterState.CHARA_ALIVE;
        }

        public void SpawnCharacter(Mogre.Vector3 spawnPos, string characterID,string charaMeshName)
        {
            Entity charaEnt = cam.SceneManager.CreateEntity(characterID, charaMeshName + ".mesh");
            SceneNode charaNode = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
            charaNode.AttachObject(charaEnt);
            charaNode.SetPosition(spawnPos.x, spawnPos.y, spawnPos.z);

            AnimationStateSet ass = charaEnt.AllAnimationStates;
            AnimationStateIterator iterator = ass.GetAnimationStateIterator();

            charaEnt.GetAnimationState("IdleBase").Loop = true;
            charaEnt.GetAnimationState("IdleTop").Loop = true;
            
            charaEnt.GetAnimationState("IdleBase").Enabled = true;
            charaEnt.GetAnimationState("IdleTop").Enabled = true;

            charaEntMap.Add(characterID, charaEnt);
        }

        public void DamageCharacter(string characterID,int damage)
        {
            Entity charaEnt = charaEntMap[characterID];
            CharacterDie(characterID);
            //int hp = chara.HitPoint;
            //if (hp > damage)
            //{
            //    hp = hp - damage;
            //    chara.HitPoint = hp;
            //}
            //else
            //{
            //    chara.HitPoint = 0;
            //    CharacterDie(chara);
            //}
        }

        public void CharacterDie(string chara)
        {
            //Character chara = characters.Where(o => o.CharaID == characterID).First();

            //chara.CharaState = CharacterState.CHARA_DEAD;

            Entity charaEnt = charaEntMap[chara];
            if (charaEnt.ParentSceneNode != null)
            {
                SceneNode charaNode = charaEnt.ParentSceneNode;
                charaNode.DetachObject(charaEnt);
            }
        }

        public void Update(float deltaTime)
        {
        }

        public void MoveCharacter(string charaID)
        {
            moveOffset = Mogre.Vector3.ZERO;
            Entity charaEnt = charaEntMap[charaID];
            if (keyboard.IsKeyDown(KeyCode.KC_U))
            {
                moveOffset.z = -0.1f;
            }
            if (keyboard.IsKeyDown(KeyCode.KC_J))
            {
                moveOffset.z = 0.1f;
            }
            if (keyboard.IsKeyDown(KeyCode.KC_K))
            {
                moveOffset.x = 0.1f;
            }
            if (keyboard.IsKeyDown(KeyCode.KC_H))
            {
                moveOffset.x = -0.1f;
            }
            charaEnt.ParentNode.Translate(moveOffset);
        }

        public void SetCharacterLookAtPos(string charaSrcID,string charaTargetID)
        {
            if (string.Compare(charaSrcID, charaTargetID) != 0)
            {
                Entity srcEnt = charaEntMap[charaSrcID];
                Entity targetEnt = charaEntMap[charaTargetID];
                if (srcEnt != null && targetEnt != null)
                {
                    Mogre.Vector3 srcPos = srcEnt.ParentNode.Position;
                    Bone srcHead = srcEnt.Skeleton.GetBone("Head");
                    Mogre.Vector3 srcLookAt = srcHead._getDerivedOrientation() * Mogre.Vector3.UNIT_Z;

                    Mogre.Vector3 targetPos = targetEnt.ParentNode.Position;
                    Mogre.Vector3 targetLookAt = new Mogre.Vector3(targetPos.x - srcPos.x, targetPos.y - srcPos.y, targetPos.z - srcPos.z);

                    float delta = srcLookAt.DotProduct(targetLookAt) / (srcLookAt.Length * targetLookAt.Length);
                    Radian r = Mogre.Math.ACos(delta * -1);
                    if (lastRotateAngle == r.ValueDegrees)
                    {
                        srcEnt.ParentNode.Yaw(new Degree(r.ValueDegrees));
                    }
                    else
                    {
                        lastRotateAngle = r.ValueDegrees;
                        return;
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
