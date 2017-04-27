using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Mods.TestDemo
{
    class TestModMain
    {
        public void Initilize()
        {
            TestModMenuState.create<TestModMenuState>(GameManager.Singleton.m_pAppStateManager, "TestModMenuState");
        }
    }
}
