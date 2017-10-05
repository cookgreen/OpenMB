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
        private List<Character> characters;
        private Dictionary<string, Entity> charaEntMap;
        private Camera cam;
        private Keyboard keyboard;
        private Mouse mouse;
        private List<Character> characherLst;
        private Mogre.Vector3 spawnPosition;
        private List<Mods.XML.ModCharacterDfnXML> characterDfns;

        public CharacterManager(Camera cam,Keyboard keyboard,Mouse mouse)
        {
            this.cam = cam;
            this.keyboard = keyboard;
            this.mouse = mouse;
            charaEntMap = new Dictionary<string, Entity>();
            characters = new List<Character>();
            characherLst = new List<Character>();
            Root.Singleton.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
        }

        public void Init(List<Mods.XML.ModCharacterDfnXML> characterDfns)
        {
            this.characterDfns = characterDfns;
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

        public void SpawnCharacter(string charaID)
        {
            Mods.XML.ModCharacterDfnXML charaDfn = characterDfns.Where(o => o.ID == charaID).FirstOrDefault();

            Character character = new Character("chara_" + GameManager.Singleton.AllGameObjects.Count, this.cam, this.keyboard, this.mouse);
            character.InitPos = spawnPosition;
            character.Create(charaDfn);
            characherLst.Add(character);
            GameManager.Singleton.AllGameObjects.Add(character);
        }

        public void SpawnPlayer(string charaID)
        {
            Mods.XML.ModCharacterDfnXML charaDfn = characterDfns.Where(o => o.ID == charaID).FirstOrDefault();

            Player character = new Player("player",this.cam, this.keyboard, this.mouse);
            character.InitPos = spawnPosition;
            character.Create(charaDfn);
            characherLst.Add(character);
            GameManager.Singleton.AllGameObjects.Add(character);
        }

        public Player GetPlayer()
        {
            var player = from gameObj in GameManager.Singleton.AllGameObjects
                         where gameObj.UniqueId == Utilities.Helper.GetStringHash("player")
                         select gameObj;
            return (Player)(player.Count() > 0 ? player.FirstOrDefault() : null);
        }

        public NameValuePairList GetCharacterInfo(string charaID)
        {
            Mods.XML.ModCharacterDfnXML charaDfn = characterDfns.Where(o => o.ID == charaID).FirstOrDefault();
            NameValuePairList npl = new NameValuePairList();
            npl["CharacteName"] = charaDfn.Name;
            return npl;
        }
    }
}
