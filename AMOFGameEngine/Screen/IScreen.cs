using Mogre;
using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Screen
{
    public interface IScreen
    {
        bool IsVisible { get; }
        event Action OnScreenExit;
        string Name { get; }
        void Init(params object[] param);
        void Run();
        void Update(float timeSinceLastFrame);
        void Exit();

        void InjectMouseMove(MouseEvent arg);
        void InjectMousePressed(MouseEvent arg, MouseButtonID id);
        void InjectMouseReleased(MouseEvent arg, MouseButtonID id);
        void InjectKeyPressed(KeyEvent arg);
        void InjectKeyReleased(KeyEvent arg);
        void Show();
        void Hide();
        bool CheckEnterScreen(Vector2 mousePos);
    }
}
