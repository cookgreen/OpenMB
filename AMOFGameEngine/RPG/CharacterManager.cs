using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;

namespace AMOFGameEngine.RPG
{
    public class CharacterManager
    {
        List<Character> characters;
        Dictionary<string, Entity> charaEntMap;
        Camera cam;
        Keyboard keyboard;
        Mouse mouse;
        Mogre.Vector3 moveOffset;
        AnimationState animState;
        AnimationState animStateTop;
        List<Character> characherLst;

        public event Action<Mogre.Vector3> CharacterPosChanged;
        private Mogre.Vector3 spawnPosition;

        public CharacterManager(Camera cam,Keyboard keyboard,Mouse mouse)
        {
            this.cam = cam;
            this.keyboard = keyboard;
            this.mouse = mouse;
            charaEntMap = new Dictionary<string, Entity>();
            characters = new List<Character>();
            moveOffset = new Mogre.Vector3();
            characherLst = new List<Character>();
            Root.Singleton.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
        }

        bool FrameStarted(FrameEvent evt)
        {
            foreach (KeyValuePair<string, Entity> kpl in charaEntMap)
            {
                kpl.Value.GetAnimationState("RunBase").AddTime(evt.timeSinceLastFrame);
                kpl.Value.GetAnimationState("RunTop").AddTime(evt.timeSinceLastFrame);
            }
            return true;
        }

        public void Initization()
        {

        }

        public void AddCharacterToManageLst(Character character)
        {
            characherLst.Add(character);
        }

        public void UpdateCharacters(float time)
        {
            for (int i = 0; i < characherLst.Count; i++)
            {
                if (characherLst[i].Alive)
                {
                    characherLst[i].Update(time);
                }
                else
                {
                    characherLst.RemoveAt(i);
                }
            }
        }

        public void SetSpawnPosition(Mogre.Vector3 position)
        {
            this.spawnPosition = position;
        }

        public void SpawnCharacter(string characterName, string charaMeshName)
        {
            Character character = new Character(this.cam, this.keyboard, this.mouse);
            character.CharaName=characterName;
            character.CharaMeshName=charaMeshName;
            character.Position = spawnPosition;
            character.Create();
            characherLst.Add(character);
        }

        public void SpawnPlayer(string playerName, string playerMeshName)
        {
            Character character = new Character(this.cam, this.keyboard, this.mouse,true);
            character.CharaName = playerName;
            character.CharaMeshName = playerMeshName;
            character.InitPos = spawnPosition;
            character.Create();
            characherLst.Add(character);
        }

        public Character GetPlayer()
        {
            var player = from character in characherLst
                         where character.IsPlayer
                         select character;
            return player.Count() > 0 ? player.FirstOrDefault() : null;
        }
    }
}
