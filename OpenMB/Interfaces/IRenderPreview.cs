using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Interfaces
{
    public interface IRenderPreview
    {
        MaterialPtr RenderPreview(Entity entity);
    }
}
