using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;
using AMOFGameEngine.Utilities;

namespace AMOFGameEngine.Game
{
    public class ScenePlane : SceneProp
    {
        private string materialName;
        private string groupName;
        private float width;
        private float height;
        private int xsegments;
        private int ysegments;
        private bool normals;
        private ushort numTexCoordSets;
        private float uTile;
        private float vTile;
        private Vector3 rkNormal;
        private Vector3 upVector;
        private float fConstanst;
        public ScenePlane(int id, GameWorld world, Vector3 rkNormal, float fConstanst, string materialName, string groupName, 
            float width, float height, int xsegments, int ysegments, bool normals, ushort numTexCoordSets, float uTile, 
            float vTile, Vector3 upVector, Vector3 initPosition) : base(id, world, null, initPosition)
        {
            this.materialName = materialName;
            this.groupName = groupName;
            this.width = width;
            this.height = height;
            this.xsegments = xsegments;
            this.ysegments = ysegments;
            this.normals = normals;
            this.numTexCoordSets = numTexCoordSets;
            this.uTile = uTile;
            this.vTile = vTile;
            this.upVector = upVector;
            this.rkNormal = rkNormal;
            this.fConstanst = fConstanst;
            create();
        }

        protected override void create()
        {
            string name = Guid.NewGuid().ToString();
            MeshManager.Singleton.CreatePlane(name, groupName,
                       new Plane(rkNormal, fConstanst), width, height, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, upVector);
            entity = sceneManager.CreateEntity(Guid.NewGuid().ToString(), name);
            entity.SetMaterialName(materialName);
            entity.CastShadows = false;
            entNode = sceneManager.RootSceneNode.CreateChildSceneNode();
            entNode.AttachObject(entity);
            entNode.Position = position;
            ActorDesc actorDesc = new ActorDesc();
            actorDesc.Density = 4;
            actorDesc.Body = null;
            actorDesc.Shapes.Add(physics.CreateTriangleMesh(new
                StaticMeshData(entity.GetMesh())));
            Actor entityActor = physicsScene.CreateActor(actorDesc);
        }
    }
}
