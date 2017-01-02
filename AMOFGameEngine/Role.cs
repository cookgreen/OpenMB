using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine
{
    class Role
    {
        private string mRoleName;
        private string mRoleMeshName;
        private AnimationState[] mAnims;
        private Camera mRoleCamera;
        private bool bControned;
        private Entity mRoleEnt;
        private SceneNode mRoleNode;

        public Role(string roleName,string roleMeshName,Camera camera)
        {
            mRoleName=roleName;
            mRoleMeshName=roleMeshName;
            mRoleCamera=camera;

            SetupRole(mRoleName, mRoleMeshName, mRoleCamera.SceneManager);
        }
        void SetupRole(string roleName, string roleMeshName, SceneManager scm)
        {
            mRoleNode = scm.RootSceneNode.CreateChildSceneNode();
            mRoleEnt = scm.CreateEntity(roleName, roleMeshName);
            mRoleNode.AttachObject(mRoleEnt);
        }
        void SetupCamera(Camera camera)
        {

        }
    }
}
