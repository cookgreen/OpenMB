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
        public enum ServerError
        {
            ERR_SAME_USERNAME,
            ERR_CONNECTION_LOST,
            ERR_KICKED_BY_SERVER,
            ERR_BANNED_BY_SERVER
        }

        bool isStarted;
        private ServerMetaData metaData;
        private Dictionary<int, MpPlayer> players;
        private TcpListener listener;

        public event Action OnEscapePressed;
        public ServerMetaData MetaData { get { return metaData; } }
        public bool Started { get { return isStarted; } }
        public Dictionary<string, string> Options;
        public GameServer()
        {
            players = new Dictionary<int, MpPlayer>();
            metaData = null;
        }

        public void Init()
        {
            listener = new TcpListener(new IPAddress(new byte[]{127,0,0,1}),7458);

            GameManager.Instance.mKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(mKeyboard_KeyPressed);
            GameManager.Instance.mKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(mKeyboard_KeyReleased);
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
                MpPlayer p = new MpPlayer();
                p.Client = client;
                p.Position = new Mogre.Vector3();
                players.Add(players.Count, p);
                client.Close();

                return true;
            }
        }

        public void KickPlayer(int playerId)
        {
            MpPlayer targetPlayer = players.Where(o => o.Key == playerId).FirstOrDefault().Value;
            if (targetPlayer == null)
            {
                return;
            }
            targetPlayer.Client.Close();
        }

        public void Update()
        {
            TcpClient client = null;
            try
            {
                if (!listener.Pending())
                {
                    return;
                }
                client = listener.AcceptTcpClient();
                if (client == null)
                {
                    return;
                }
                if (!client.Connected)
                {
                    return;
                }
                string playerName;
                using (BinaryReader br = new BinaryReader(client.GetStream()))
                {
                    playerName = br.ReadString();
                }
                NewPlayerJoin(playerName, client);
            }
            catch(Exception ex)
            {
                if (client != null)
                {
                    client.Close();
                }
                Mogre.LogManager.Singleton.LogMessage(string.Format("[Engine Error]: {0}",ex.ToString()));
                return;
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
