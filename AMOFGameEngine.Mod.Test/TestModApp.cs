using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Mod.Common;

namespace AMOFGameEngine.Mod.Test
{
    public class TestModApp : ModApp
    {
        public override void Run()
        {
            TestModMenuState.create<TestModMenuState>(m_pModStateManager, "TestModMainMenu");

            m_pModStateManager.start(m_pModStateManager.findByName("TestModMainMenu"));
        }

        private ModStateManager m_pModStateManager;
    }
}
