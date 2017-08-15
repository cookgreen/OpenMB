using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using MOIS;

namespace AMOFGameEngine.Network
{
    public class GameServer
    {
        bool isStarted;
        private ServerMetaData metaData;
        private Dictionary<int, Player> players;
        private TcpListener listener;

        public event Action OnEscapePressed;
        public ServerMetaData MetaData { get { return metaData; } }
        public bool Started { get { return isStarted; } }
        public Dictionary<string, string> Options;
        public GameServer()
        {
            players = new Dictionary<int, Player>();
            metaData = null;
        }

        public void Init()
        {
            listener = new TcpListener(new IPAddress(new byte[]{127,0,0,1}),7458);

            GameManager.Singleton.mKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(mKeyboard_KeyPressed);
            GameManager.Singleton.mKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(mKeyboard_KeyReleased);
        }

        bool mKeyboard_KeyReleased(KeyEvent arg)
        {
            if (arg.key == KeyCode.KC_ESCAPE)
            {
                if (OnEscapePressed != null)
                {
                    OnEscapePressed();
                }
            }
            return true;
        }

        bool mKeyboard_KeyPressed(KeyEvent arg)
        {
            if (arg.key == KeyCode.KC_ESCAPE)
            {
                if (OnEscapePressed != null)
                {
                    OnEscapePressed();
                }
            }
            return true;
        }

        public bool Go()
        {
            listener.Start();
            isStarted = true;
            return true;
        }

        public void Exit()
        {
            isStarted = false;
        }

        bool NewPlayerJoin(string playerName,TcpClient client)
        {
            if (players.Count > 0)
            {
                if (players.Where(o => o.Value.Name == playerName).Count() > 0)
                {
                    BinaryWriter bw = new BinaryWriter(client.GetStream());
                    bw.Write("Your username has already been taken by another player!");
                    return false;
                }
            }
            lock (players)
            {
                Player p = new Player();
                p.Client = client;
                p.Position = new Mogre.Vector3();
                players.Add(players.Count, p);
                return true;
            }
        }

        public void Update()
        {
            if (listener.Pending())
            {
                var client = listener.AcceptTcpClient();
                if (client != null)
                {
                    string playerName;
                    using (BinaryReader br = new BinaryReader(client.GetStream()))
                    {
                        playerName = br.ReadString();
                    }
                    NewPlayerJoin(playerName, client);
                }
            }
        }

        public void GetServerState(ref Mogre.StringVector serverState)
        {
            serverState.Clear();
            serverState.Add("Current Server State:");
            serverState.Add(string.Format("Current Players Num: {0}", players.Count));
        }
    }
}
