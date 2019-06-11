using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Connector
{
    public class MBOgreManifest
    {
        private List<string> meshNames;
        private List<string> materialNames;
        private List<string> textureNames;

        public List<string> MeshNames
        {
            get
            {
                return meshNames;
            }

            set
            {
                meshNames = value;
            }
        }

        public List<string> MaterialNames
        {
            get
            {
                return materialNames;
            }

            set
            {
                materialNames = value;
            }
        }

        public List<string> TextureNames
        {
            get
            {
                return textureNames;
            }

            set
            {
                textureNames = value;
            }
        }
    }
}
