using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game.Controller
{
    /// <summary>
    /// This base class can respond to mouse and keyboard control events to complete the corresponding operation
    /// </summary>
    public class GameObjectController : EngineRenderable
    {
        public GameObjectController(Camera camera) : base(camera)
        { 
        }
    }
}
