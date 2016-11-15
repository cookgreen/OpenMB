#region Using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Mogre;

#endregion

namespace RMOgre
{
    /// <summary>
    /// Classe permettant le chargement d'un scene Ogre
    /// </summary>
    public class DotSceneLoader
    {
        #region Membres

        private readonly List<LookTarget> lookTargets = new List<LookTarget>();
        private readonly List<TrackTarget> trackTargets = new List<TrackTarget>();

        /// <summary>
        /// SceneNode attachée
        /// </summary>
        protected SceneNode sceneNodeAttach;

        /// <summary>
        /// SceneManager
        /// </summary>
        protected SceneManager sceneManager;

        /// <summary>
        /// nom groupe
        /// </summary>
        protected string sGroupName;

        /// <summary>
        /// node
        /// </summary>
        protected string sPrependNode;

        private Camera camera;

        #endregion

        #region Structures

        /// <summary>
        /// Structure permettant de stocker les noeuds Look pour affectation a posteriori
        /// </summary>
        private struct LookTarget
        {
            private readonly SceneNode sourceNode;
            private readonly Camera sourceCamera;
            private string nodeName;
            private Node.TransformSpace relativeTo;
            private bool isPositionSet;
            private Vector3 position;
            private Vector3 localDirection;

            public Camera SourceCamera
            {
                get { return sourceCamera; }
            }

            public string NodeName
            {
                get { return nodeName; }
                set { nodeName = value; }
            }

            public Node.TransformSpace RelativeTo
            {
                get { return relativeTo; }
                set { relativeTo = value; }
            }

            public bool IsPositionSet
            {
                get { return isPositionSet; }
                set { isPositionSet = value; }
            }

            public Vector3 Position
            {
                get { return position; }
                set { position = value; }
            }

            public Vector3 LocalDirection
            {
                get { return localDirection; }
                set { localDirection = value; }
            }

            public SceneNode SourceNode
            {
                get { return sourceNode; }
            }

            /// <summary>
            ///  * Initializes the LookTarget for a scene node or camera.
            ///      * Either sourceNode or sourceCamera must be non-null
            /// </summary>
            /// <param name="_sourceNode">Noeud</param>
            /// <param name="_sourceCamera">Camera</param>
            public LookTarget(SceneNode _sourceNode, Camera _sourceCamera)
            {
                sourceNode = _sourceNode;
                sourceCamera = _sourceCamera;
                relativeTo = Node.TransformSpace.TS_LOCAL;
                isPositionSet = false;
                position = Vector3.ZERO;
                localDirection = Vector3.NEGATIVE_UNIT_Z;
                nodeName = null;
            }
        }

        /// <summary>
        /// Structure permettant de stocker les noeuds target pour affectation a posteriori
        /// </summary>
        private struct TrackTarget
        {
            private readonly SceneNode sourceNode;
            private readonly Camera sourceCamera;
            private string nodeName;
            private Vector3 offset;
            private Vector3 localDirection;

            public Camera SourceCamera
            {
                get { return sourceCamera; }
            }

            public SceneNode SourceNode
            {
                get { return sourceNode; }
            }

            public string NodeName
            {
                get { return nodeName; }
                set { nodeName = value; }
            }

            public Vector3 LocalDirection
            {
                get { return localDirection; }
                set { localDirection = value; }
            }

            public Vector3 Offset
            {
                get { return offset; }
                set { offset = value; }
            }

            /// <summary>
            /// Constructeur
            /// </summary>
            /// <param name="_sourceNode">Noeud</param>
            /// <param name="_sourceCamera">Camera</param>
            public TrackTarget(SceneNode _sourceNode, Camera _sourceCamera)
            {
                sourceNode = _sourceNode;
                sourceCamera = _sourceCamera;
                offset = Vector3.ZERO;
                localDirection = Vector3.NEGATIVE_UNIT_Z;
                nodeName = null;
            }
        };

        #endregion

        #region Accesseurs

        /// <summary>
        /// Fenêtre de rendu
        /// </summary>
        protected RenderWindow RenderWindow { get; set; }

        #endregion

        /// <summary>
        /// Declaration d'une scene
        /// </summary>
        /// <param name="_sSceneName">Fichier à charger dans les ressources</param>
        /// <param name="_sGroupName">Groupe de ressources associées</param>
        /// <param name="_sceneManager">Scene manager associé</param>
        public void ParseDotScene(string _sSceneName, string _sGroupName, SceneManager _sceneManager)
        {
            ParseDotScene(_sSceneName, _sGroupName, _sceneManager, null, "");
        }

        /// <summary>
        /// Declaration d'une scene
        /// </summary>
        /// <param name="_sSceneName">Fichier à charger dans les ressources</param>
        /// <param name="_sGroupName">Groupe de ressources associées</param>
        /// <param name="_sceneManager">Scene manager associé</param>
        /// <param name="_sceneNode">Noeud à laquelle la scene est rattachée</param>
        public void ParseDotScene(string _sSceneName, string _sGroupName, SceneManager _sceneManager, SceneNode _sceneNode)
        {
            ParseDotScene(_sSceneName, _sGroupName, _sceneManager, _sceneNode, "");
        }

        /// <summary>
        /// Declaration d'une scene
        /// </summary>
        /// <param name="_sSceneName">Fichier à charger dans les ressources</param>
        /// <param name="_sGroupName">Groupe de ressources associées</param>
        /// <param name="_sceneManager">Scene manager associé</param>
        /// <param name="_sceneNode">Noeud à laquelle la scene est rattachée</param>
        /// <param name="_sPrependNode">Préfixe du noeud</param>
        public void ParseDotScene(string _sSceneName, string _sGroupName, SceneManager _sceneManager, SceneNode _sceneNode, string _sPrependNode)
        {
            ParseDotScene(_sSceneName, _sGroupName, _sceneManager, _sceneNode, _sPrependNode, null);
        }

        /// <summary>
        /// Declaration d'une scene
        /// </summary>
        /// <param name="_sSceneName">Fichier à charger dans les ressources</param>
        /// <param name="_sGroupName">Groupe de ressources associées</param>
        /// <param name="_sceneManager">Scene manager associé</param>
        /// <param name="_sceneNodeAttach">Noeud à laquelle la scene est rattachée</param>
        /// <param name="_sPrependNode">Préfixe du noeud</param>
        /// <param name="_renderWindow">Objet de rendu</param>
        public void ParseDotScene(string _sSceneName, string _sGroupName, SceneManager _sceneManager, SceneNode _sceneNodeAttach, string _sPrependNode, RenderWindow _renderWindow)
        {
            RenderWindow = _renderWindow;

            // set up shared object values
            sGroupName = _sGroupName;
            sceneManager = _sceneManager;
            sPrependNode = _sPrependNode;
            //this.StaticObjects = new List<string>();
            //this.DynamicObjects = new List<string>();

            DataStreamPtr pStream = ResourceGroupManager.Singleton.OpenResource(_sSceneName, _sGroupName);

            string data = pStream.AsString;
            // Open the .scene File
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            pStream.Close();

            // Validate the File
            XmlElement xmlRoot = xmlDoc.DocumentElement;
            if (xmlRoot != null && xmlRoot.Name != "scene")
            {
                LogManager.Singleton.LogMessage("[DotSceneLoader] Error: Invalid .scene File. Missing <scene>");
                return;
            }

            // figure out where to attach any nodes we create
            sceneNodeAttach = _sceneNodeAttach ?? sceneManager.RootSceneNode;

            // Process the scene
            ProcessScene(xmlRoot);
        }

        /// <summary>
        /// Lecture d'un nombre a virgule
        /// </summary>
        /// <param name="_s">nombre format texte</param>
        /// <returns>float</returns>
        protected float ParseFloat(string _s)
        {
            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };
            return float.Parse(_s, provider);
        }

        /// <summary>
        /// récupération chaine
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sAttrib">attribut</param>
        /// <returns>string</returns>
        protected string GetAttrib(XmlElement _xmlElement, string _sAttrib)
        {
            return GetAttrib(_xmlElement, _sAttrib, "");
        }

        /// <summary>
        /// récupération chaine
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sAttrib">attribut</param>
        /// <param name="_sDefaultValue">valeur par défaut</param>
        /// <returns>string</returns>
        protected string GetAttrib(XmlElement _xmlElement, string _sAttrib, string _sDefaultValue)
        {
            if (!string.IsNullOrEmpty(_xmlElement.GetAttribute(_sAttrib))) return _xmlElement.GetAttribute(_sAttrib);
            return _sDefaultValue;
        }

        /// <summary>
        /// récupération bolléen
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sParameter">attribut</param>
        /// <returns>bool</returns>
        protected bool GetAttribBool(XmlElement _xmlElement, string _sParameter)
        {
            return GetAttribBool(_xmlElement, _sParameter, false);
        }

        /// <summary>
        /// récupération bolléen
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sAttrib">attribut</param>
        /// <param name="_bDefaultValue">valeur par défaut</param>
        /// <returns>bool</returns>
        protected bool GetAttribBool(XmlElement _xmlElement, string _sAttrib, bool _bDefaultValue)
        {
            if (string.IsNullOrEmpty(_xmlElement.GetAttribute(_sAttrib)))
                return _bDefaultValue;

            if (_xmlElement.GetAttribute(_sAttrib) == "true")
                return true;

            return false;
        }

        /// <summary>
        /// Récupération SceneNode
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sParameter">paramètres</param>
        /// <returns>SceneNode</returns>
        protected SceneNode GetAttribSceneNode(XmlElement _xmlElement, string _sParameter)
        {
            return GetAttribSceneNode(_xmlElement, _sParameter, null);
        }

        /// <summary>
        /// Récupération SceneNode
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sAttrib">attributs</param>
        /// <param name="_snDefaultValue">SceneNode par défaut</param>
        /// <returns>SceneNode</returns>
        protected SceneNode GetAttribSceneNode(XmlElement _xmlElement, string _sAttrib, SceneNode _snDefaultValue)
        {
            string sNodeName = _xmlElement.GetAttribute(_sAttrib);
            if (string.IsNullOrEmpty(sNodeName)) return _snDefaultValue;

            if (!sceneManager.HasSceneNode(sNodeName)) return _snDefaultValue;
            return sceneManager.GetSceneNode(sNodeName);
        }

        /// <summary>
        /// Lecture attribut nombre a virgule
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sParameter">attribut</param>
        /// <returns>float</returns>
        protected float GetAttribReal(XmlElement _xmlElement, string _sParameter)
        {
            return GetAttribReal(_xmlElement, _sParameter, 0.0f);
        }

        /// <summary>
        /// Lecture attribut nombre a virgule
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sAttrib">attribut</param>
        /// <param name="_fDefaultValue">valeur par défaut</param>
        /// <returns>float</returns>
        protected float GetAttribReal(XmlElement _xmlElement, string _sAttrib, float _fDefaultValue)
        {
            if (!string.IsNullOrEmpty(_xmlElement.GetAttribute(_sAttrib))) return ParseFloat(_xmlElement.GetAttribute(_sAttrib));
            return _fDefaultValue;
        }



        /// <summary>
        /// Lecture uint
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sParameter">attribut</param>
        /// <returns>uint</returns>
        protected uint GetAttribUInt(XmlElement _xmlElement, string _sParameter)
        {
            return GetAttribUInt(_xmlElement, _sParameter, 0);
        }

        /// <summary>
        /// Lecture uint
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sAttrib">attribut</param>
        /// <param name="_uiDefaultValue">valeur par défaut</param>
        /// <returns>uint</returns>
        protected uint GetAttribUInt(XmlElement _xmlElement, string _sAttrib, uint _uiDefaultValue)
        {
            if (!string.IsNullOrEmpty(_xmlElement.GetAttribute(_sAttrib))) return uint.Parse(_xmlElement.GetAttribute(_sAttrib));
            return _uiDefaultValue;
        }

        /// <summary>
        /// Récupération InterpolationMode
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sAttrib">attribut</param>
        /// <returns>InterpolationMode</returns>
        protected Animation.InterpolationMode GetAttribAnimationInterpolationMode(XmlElement _xmlElement, string _sAttrib)
        {
            return GetAttribAnimationInterpolationMode(_xmlElement, _sAttrib, Animation.InterpolationMode.IM_LINEAR);
        }

        /// <summary>
        ///  Récupération InterpolationMode
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sAttrib">attribut</param>
        /// <param name="_interpolationModeDefaultValue">InterpolationMode par défaut</param>
        /// <returns>InterpolationMode</returns>
        protected Animation.InterpolationMode GetAttribAnimationInterpolationMode(XmlElement _xmlElement, string _sAttrib, Animation.InterpolationMode _interpolationModeDefaultValue)
        {
            string sInterpolationModeText = GetAttrib(_xmlElement, _sAttrib);
            if (!string.IsNullOrEmpty(sInterpolationModeText))
            {
                sInterpolationModeText = sInterpolationModeText.ToLower();
                if (sInterpolationModeText == "linear")
                    return Animation.InterpolationMode.IM_LINEAR;
                if (sInterpolationModeText == "spline")
                    return Animation.InterpolationMode.IM_SPLINE;

                LogManager.Singleton.LogMessage("Invalid animation interpolation mode specified: " + sInterpolationModeText);
            }
            return _interpolationModeDefaultValue;
        }

        /// <summary>
        ///  Récupération RotationInterpolationMode
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sAttrib">attribut</param>
        /// <returns>RotationInterpolationMode</returns>
        protected Animation.RotationInterpolationMode GetAttribAnimationRotationInterpolationMode(XmlElement _xmlElement, string _sAttrib)
        {
            return GetAttribAnimationRotationInterpolationMode(_xmlElement, _sAttrib, Animation.RotationInterpolationMode.RIM_LINEAR);
        }

        /// <summary>
        ///  Récupération RotationInterpolationMode
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sAttrib">attribut</param>
        /// <param name="_rotationInterpolationModeModeDefaultValue">RotationInterpolationMode par défaut</param>
        /// <returns>RotationInterpolationMode</returns>
        protected Animation.RotationInterpolationMode GetAttribAnimationRotationInterpolationMode(XmlElement _xmlElement, string _sAttrib, Animation.RotationInterpolationMode _rotationInterpolationModeModeDefaultValue)
        {
            string sRotationInterpolationMode = GetAttrib(_xmlElement, _sAttrib);
            if (!string.IsNullOrEmpty(sRotationInterpolationMode))
            {
                sRotationInterpolationMode = sRotationInterpolationMode.ToLower();
                if (sRotationInterpolationMode == "linear")
                    return Animation.RotationInterpolationMode.RIM_LINEAR;
                if (sRotationInterpolationMode == "spherical")
                    return Animation.RotationInterpolationMode.RIM_SPHERICAL;

                LogManager.Singleton.LogMessage("Invalid animation rotation interpolation mode specified: " + sRotationInterpolationMode);
            }
            return _rotationInterpolationModeModeDefaultValue;
        }

        /// <summary>
        /// Lecture couleur
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <returns>ColourValue</returns>
        protected ColourValue ParseColour(XmlElement _xmlElement)
        {
            return new ColourValue(
              ParseFloat(_xmlElement.GetAttribute("r")),
              ParseFloat(_xmlElement.GetAttribute("g")),
              ParseFloat(_xmlElement.GetAttribute("b")),
              string.IsNullOrEmpty(_xmlElement.GetAttribute("a")) == false ? ParseFloat(_xmlElement.GetAttribute("a")) : 1
              );
        }

        /// <summary>
        /// Lecture quaternion
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <returns>Quaternion</returns>
        protected Quaternion ParseQuaternion(XmlElement _xmlElement)
        {
            Quaternion orientation = new Quaternion
            {
                x = ParseFloat(_xmlElement.GetAttribute("x")),
                y = ParseFloat(_xmlElement.GetAttribute("y")),
                z = ParseFloat(_xmlElement.GetAttribute("z")),
                w = ParseFloat(_xmlElement.GetAttribute("w"))
            };

            return orientation;
        }

        /// <summary>
        /// Lecture quaternion de rotation
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <returns>Quaternion</returns>
        protected Quaternion ParseRotation(XmlElement _xmlElement)
        {
            Quaternion orientation = new Quaternion
            {
                x = ParseFloat(_xmlElement.GetAttribute("qx")),
                y = ParseFloat(_xmlElement.GetAttribute("qy")),
                z = ParseFloat(_xmlElement.GetAttribute("qz")),
                w = ParseFloat(_xmlElement.GetAttribute("qw"))
            };

            return orientation;
        }

        /// <summary>
        /// Lecture d'un Vector3
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <returns>Vector3</returns>
        protected Vector3 ParseVector3(XmlElement _xmlElement)
        {
            return new Vector3(
              ParseFloat(_xmlElement.GetAttribute("x")),
              ParseFloat(_xmlElement.GetAttribute("y")),
              ParseFloat(_xmlElement.GetAttribute("z"))
              );
        }

        /// <summary>
        /// Chargement caméra
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sceneNodeParent">SceneNode parent</param>
        protected void ProcessCamera(XmlElement _xmlElement, SceneNode _sceneNodeParent)
        {
            #region Defaut

            // Process attributes
            string name = GetAttrib(_xmlElement, "name");

            // Create the camera
            camera = sceneManager.CreateCamera(name);

            if (_sceneNodeParent != null)
                _sceneNodeParent.AttachObject(camera);

            //camera.AspectRatio = GetAttribReal(_xmlElement, "aspectRatio", 4 / 3f);


            float pFov = GetAttribReal(_xmlElement, "fov", Mogre.Math.PI / 4);
            camera.FOVy = new Radian(pFov);

            string sValue = GetAttrib(_xmlElement, "projectionType", "perspective");
            if (sValue == "perspective") camera.ProjectionType = ProjectionType.PT_PERSPECTIVE;
            else if (sValue == "orthographic") camera.ProjectionType = ProjectionType.PT_ORTHOGRAPHIC;

            #endregion

            foreach (XmlElement xmlElementChild in _xmlElement.ChildNodes)
            {
                switch (xmlElementChild.Name)
                {
                    case "rotation":
                        camera.Rotate(ParseRotation(xmlElementChild));
                        break;
                    case "clipping":
                        float fNearDist = GetAttribReal(xmlElementChild, "nearPlaneDist");
                        // ReSharper disable CompareOfFloatsByEqualityOperator
                        if (fNearDist == 0)
                        {
                            // 3ds
                            fNearDist = GetAttribReal(xmlElementChild, "near");
                        }
                        camera.NearClipDistance = fNearDist;
                        // Blender
                        float fFarDist = GetAttribReal(xmlElementChild, "farPlaneDist");
                        if (fFarDist == 0)
                        {
                            // 3ds
                            fFarDist = GetAttribReal(xmlElementChild, "far");
                        }
                        // ReSharper restore CompareOfFloatsByEqualityOperator
                        camera.FarClipDistance = fFarDist;
                        break;
                    case "lookTarget":
                        LoadLookTarget(xmlElementChild, null, camera);
                        break;
                    case "trackTarget":
                        LoadTrackTarget(xmlElementChild, null, camera);
                        break;
                }
            }

            //xmlElement = (XmlElement)_xmlElement.SelectSingleNode("target");
            //if (xmlElement != null)
            //{
            //  bool bEnable = GetAttribBool(xmlElement, "enabled", true);
            //  Vector3 v3Offset = new Vector3(GetAttribReal(xmlElement, "offset_x", 0), GetAttribReal(xmlElement, "offset_y", 0), GetAttribReal(xmlElement, "offset_z", 0));
            //  SceneNode pNode = GetAttribSceneNode(xmlElement, "node_name", null);
            //  if (pNode != null) camera.SetAutoTracking(bEnable, pNode, v3Offset);
            //}

            //xmlElement = (XmlElement)_xmlElement.SelectSingleNode("lookat");
            //if (xmlElement != null)
            //{
            //  Vector3 v3Offset = new Vector3(GetAttribReal(xmlElement, "x", 0), GetAttribReal(xmlElement, "y", 0), GetAttribReal(xmlElement, "z", 0));
            //  camera.LookAt(v3Offset);
            //}
        }

        /// <summary>
        /// Chargement d'une entity
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sceneNodeParent">SceneNode parent</param>
        protected void ProcessEntity(XmlElement _xmlElement, SceneNode _sceneNodeParent)
        {
            Entity entity = GetEntity(_xmlElement);
            if (entity != null) _sceneNodeParent.AttachObject(entity);
        }

        /// <summary>
        /// Retourne l'entity défini dans le xmlElement
        /// </summary>
        /// <param name="_xmlElement"></param>
        /// <returns></returns>
        private Entity GetEntity(XmlElement _xmlElement)
        {
            Entity entity = null;
            // Process attributes
            string name = GetAttrib(_xmlElement, "name");
            string meshFile = GetAttrib(_xmlElement, "meshFile");

            bool bvisible = GetAttribBool(_xmlElement, "visible", true);
            bool bcastshadows = GetAttribBool(_xmlElement, "castShadows", true);
            float brenderingDistance = GetAttribReal(_xmlElement, "renderingDistance", 0);
            //bool bReceiveShadows = GetAttribBool(_xmlElement, "receiveShadows", true);

            // Create the entity
            try
            {
                MeshPtr mesh = MeshManager.Singleton.Load(meshFile, sGroupName);
                try
                {
                    ushort src, dest;
                    mesh.SuggestTangentVectorBuildParams(VertexElementSemantic.VES_TANGENT, out src, out dest);
                    mesh.BuildTangentVectors(VertexElementSemantic.VES_TANGENT, src, dest);
                }
                catch
                {
                }

                entity = sceneManager.CreateEntity(name, meshFile);
                entity.Visible = bvisible;
                entity.CastShadows = bcastshadows;
                //entity.ReceivesShadows = bReceiveShadows; //No setter
                entity.RenderingDistance = brenderingDistance;

                // Process subentities (?)
                ProcessSubEntities(_xmlElement, entity);
                ProcessBoneAttachments(_xmlElement, entity);
            }
            catch (Exception e)
            {
                LogManager.Singleton.LogMessage("[DotSceneLoader] Error loading an entity!" + e.Message);
            }
            return entity;
        }

        /// <summary>
        /// Gère les subEntities
        /// </summary>
        /// <param name="_xmlElement"></param>
        /// <param name="_entity"></param>
        private void ProcessSubEntities(XmlElement _xmlElement, Entity _entity)
        {
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("subentities");
            if (xmlElement != null)
            {
                xmlElement = (XmlElement)xmlElement.FirstChild;
                while (xmlElement != null)
                {
                    string mat = GetAttrib(xmlElement, "materialName");
                    _entity.GetSubEntity(GetAttribUInt(xmlElement, "index")).SetMaterialName(mat);
                    xmlElement = (XmlElement)xmlElement.NextSibling;
                }
            }
        }

        /// <summary>
        /// Créer les attachements
        /// </summary>
        /// <param name="_xmlElement"></param>
        /// <param name="_entity">Entity auquel sont associés les elts à rattacher</param>
        private void ProcessBoneAttachments(XmlElement _xmlElement, Entity _entity)
        {
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("boneAttachments");
            if (xmlElement != null)
            {
                XmlElement xmlElementAttach = (XmlElement)xmlElement.FirstChild;
                while (xmlElementAttach != null)
                {
                    string sBone = GetAttrib(xmlElementAttach, "bone");
                    TagPoint tagPoint = _entity.AttachObjectToBone(sBone, GetEntity((XmlElement)xmlElementAttach.SelectSingleNode("entity")));
                    //Parse child elements
                    foreach (XmlElement xmlElementChild in xmlElementAttach.ChildNodes)
                    {
                        switch (xmlElementChild.Name)
                        {
                            case "position":
                                tagPoint.Position = ParseVector3(xmlElementChild);
                                break;
                            case "rotation":
                                tagPoint.Orientation = ParseRotation(xmlElementChild);
                                break;
                            case "scale":
                                tagPoint.SetScale(ParseVector3(xmlElementChild));
                                break;
                        }
                    }
                    xmlElementAttach = (XmlElement)xmlElementAttach.NextSibling;
                }
            }
        }

        /// <summary>
        /// Chargement des animations
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sceneNodeParent">SceneNode parent</param>
        protected void ProcessNodeAnimations(XmlElement _xmlElement, SceneNode _sceneNodeParent)
        {
            foreach (XmlElement xmlElementChild in _xmlElement.ChildNodes)
            {
                if (xmlElementChild.Name == "animation")
                    ProcessNodeAnimation(xmlElementChild, _sceneNodeParent);
            }
        }

        /// <summary>
        /// Chargement d'une animation
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sceneNodeParent">SceneNode parent</param>
        protected void ProcessNodeAnimation(XmlElement _xmlElement, SceneNode _sceneNodeParent)
        {
            //Get enabled and looping states
            bool bEnable = GetAttribBool(_xmlElement, "enable", false);
            bool bLooping = GetAttribBool(_xmlElement, "loop", false);

            //Animation name
            string sName = GetAttrib(_xmlElement, "name");

            //Get existing animation or create new one
            Animation animation;
            if (sceneManager.HasAnimation(sName))
                animation = sceneManager.GetAnimation(sName);
            else
            {
                //Length
                float fLength = GetAttribReal(_xmlElement, "length", 0);

                //Interpolation mode
                Animation.InterpolationMode interpolationMode = GetAttribAnimationInterpolationMode(_xmlElement, "interpolationMode");

                //Rotation interpolation mode
                Animation.RotationInterpolationMode rotationInterpolationMode = GetAttribAnimationRotationInterpolationMode(_xmlElement, "rotationInterpolationMode");

                //Notify the callback
                //if (this->callback != 0)
                //    this->callback->LoadingNodeAnimation(this, params);

                //Create animation
                animation = sceneManager.CreateAnimation(sName, fLength);
                animation.SetInterpolationMode(interpolationMode);
                animation.SetRotationInterpolationMode(rotationInterpolationMode);

                //Notify the callback
                //if (this->callback != 0)
                //    this->callback->CreatedNodeAnimation(this, node, animation);
            }

            //Create animation track for node
            NodeAnimationTrack animationTrack = animation.CreateNodeTrack((ushort)(animation.NumNodeTracks + 1), _sceneNodeParent);

            //Load animation keyframes
            foreach (XmlElement xmlElementChild in _xmlElement.ChildNodes)
            {
                if (xmlElementChild.Name == "keyframe")
                    ProcessNodeAnimationKeyFrame(xmlElementChild, animationTrack);
            }

            //Notify callback
            //if (this->callback != 0)
            //    this->callback->CreatedNodeAnimationTrack(this, node, animationTrack, params.enable, params.looping);

            //if ((this->loadOptions & NO_ANIMATION_STATES) == 0)
            //{
            //Create a new animation state to track the animation
            if (!sceneManager.HasAnimationState(sName))
            {
                //No animation state has been created for the animation yet
                AnimationState animationState = sceneManager.CreateAnimationState(sName);
                animationState.Enabled = bEnable;
                animationState.Loop = bLooping;

                //Notify callback
                //if (this->callback != 0)
                //    this->callback->CreatedNodeAnimationState(this, node, animationState);
            }
            //else if (this->callback != 0)
            //        this->callback->ReferencedNodeAnimationState(this, node, animationState);
            //}
        }

        /// <summary>
        /// Chargement KeyFrame d'une animation 
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_nodeAnimationTrack">NodeAnimationTrack</param>
        protected void ProcessNodeAnimationKeyFrame(XmlElement _xmlElement, NodeAnimationTrack _nodeAnimationTrack)
        {
            //Key time
            float fKeyTime = GetAttribReal(_xmlElement, "time", 0);

            //Create the key frame
            TransformKeyFrame keyFrame = _nodeAnimationTrack.CreateNodeKeyFrame(fKeyTime);

            //Parse child elements
            foreach (XmlElement xmlElementChild in _xmlElement.ChildNodes)
            {
                switch (xmlElementChild.Name)
                {
                    case "translation":
                        keyFrame.Translate = ParseVector3(xmlElementChild);
                        break;
                    case "rotation":
                        keyFrame.Rotation = ParseRotation(xmlElementChild);
                        break;
                    case "scale":
                        keyFrame.Scale = ParseVector3(xmlElementChild);
                        break;
                }
            }
        }

        /// <summary>
        /// Chargement de l'environement
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        protected void ProcessEnvironment(XmlElement _xmlElement)
        {
            // Process fog (?)
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("fog");
            if (xmlElement != null) ProcessFog(xmlElement);

            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("skyBox");
            if (xmlElement != null) ProcessSkyBox(xmlElement);

            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("skyDome");
            if (xmlElement != null) ProcessSkyDome(xmlElement);

            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("skyPlane");
            if (xmlElement != null) ProcessSkyPlane(xmlElement);

            // Process colourAmbient (?)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("colourAmbient");
            if (xmlElement != null) sceneManager.AmbientLight = ParseColour(xmlElement);

            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("shadows");
            if (xmlElement != null) ProcessShadows(xmlElement);


            if (RenderWindow != null)
            {
                Viewport viewport = RenderWindow.NumViewports == 0 ? RenderWindow.AddViewport(camera) : RenderWindow.GetViewport(0);
                viewport.Camera = camera;
                camera.AspectRatio = (float)viewport.ActualWidth / viewport.ActualHeight;
                xmlElement = (XmlElement)_xmlElement.SelectSingleNode("colourBackground");
                if (xmlElement != null) viewport.BackgroundColour = ParseColour(xmlElement);
            }
        }

        /// <summary>
        /// Chargement des ombres
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        protected void ProcessShadows(XmlElement _xmlElement)
        {
            string sTechnique = GetAttrib(_xmlElement, "technique", "none");
            switch (sTechnique)
            {
                case "stencilAdditive":
                    sceneManager.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;
                    break;
                case "stencilModulative":
                    sceneManager.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_MODULATIVE;
                    break;
                case "textureAdditive":
                    sceneManager.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_ADDITIVE;
                    break;
                case "textureAdditiveIntegrated":
                    sceneManager.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_ADDITIVE_INTEGRATED;
                    break;
                case "textureModulative":
                    sceneManager.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_MODULATIVE;
                    break;
                case "textureModulativeIntegrated":
                    sceneManager.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_MODULATIVE_INTEGRATED;
                    break;
                default:
                    sceneManager.ShadowTechnique = ShadowTechnique.SHADOWTYPE_NONE;
                    break;
            }
            sceneManager.ShadowFarDistance = GetAttribReal(_xmlElement, "farDistance", 0);
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("colourShadow");
            if (xmlElement != null) sceneManager.ShadowColour = ParseColour(xmlElement);
            //      <shadows technique="stencilModulative" farDistance="0">
            //    <colourShadow r="0" g="0" b="0" />
            //</shadows>
        }

        /// <summary>
        /// Chargement ciel en "boite"
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        protected void ProcessSkyBox(XmlElement _xmlElement)
        {
            // Process attributes
            string material = GetAttrib(_xmlElement, "material");
            float distance = GetAttribReal(_xmlElement, "distance", 5000);
            bool drawFirst = GetAttribBool(_xmlElement, "drawFirst", true);

            // Process rotation (?)
            Quaternion rotation = Quaternion.IDENTITY;
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("rotation");
            if (xmlElement != null)
                rotation = ParseQuaternion(xmlElement);

            // Setup the sky box
            sceneManager.SetSkyBox(true, material, distance, drawFirst, rotation, sGroupName);
        }

        /// <summary>
        /// Chargement ciel en "dome"
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        protected void ProcessSkyDome(XmlElement _xmlElement)
        {
            // Process attributes
            string material = GetAttrib(_xmlElement, "material");
            float curvature = GetAttribReal(_xmlElement, "curvature", 10);
            float tiling = GetAttribReal(_xmlElement, "tiling", 8);
            float distance = GetAttribReal(_xmlElement, "distance", 4000);
            bool drawFirst = GetAttribBool(_xmlElement, "drawFirst", true);

            // Process rotation (?)
            Quaternion rotation = Quaternion.IDENTITY;
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("rotation");
            if (xmlElement != null)
                rotation = ParseQuaternion(xmlElement);

            // Setup the sky dome
            sceneManager.SetSkyDome(true, material, curvature, tiling, distance, drawFirst, rotation, 16, 16, -1, sGroupName);
        }

        /// <summary>
        /// Chargement plan de ciel
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        protected void ProcessSkyPlane(XmlElement _xmlElement)
        {
            // Process attributes
            string material = GetAttrib(_xmlElement, "material");
            float planeX = GetAttribReal(_xmlElement, "planeX", 0);
            float planeY = GetAttribReal(_xmlElement, "planeY", -1);
            float planeZ = GetAttribReal(_xmlElement, "planeZ", 0);
            float planeD = GetAttribReal(_xmlElement, "planeD", 5000);
            float scale = GetAttribReal(_xmlElement, "scale", 1000);
            float bow = GetAttribReal(_xmlElement, "bow", 0);
            float tiling = GetAttribReal(_xmlElement, "tiling", 10);
            bool drawFirst = GetAttribBool(_xmlElement, "drawFirst", true);

            // Setup the sky plane
            Plane plane;
            plane.normal = new Vector3(planeX, planeY, planeZ);
            plane.d = planeD;
            sceneManager.SetSkyPlane(true, plane, material, scale, tiling, drawFirst, bow, 1, 1, sGroupName);
        }

        /// <summary>
        /// Chargement brouillard
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        protected void ProcessFog(XmlElement _xmlElement)
        {
            // Process attributes
            float linearStart = GetAttribReal(_xmlElement, "linearStart", 0.0f);
            float linearEnd = GetAttribReal(_xmlElement, "linearEnd", 1.0f);

            FogMode mode = FogMode.FOG_NONE;
            string sMode = GetAttrib(_xmlElement, "mode");
            // only linear atm
            if (sMode == "none")
                mode = FogMode.FOG_NONE;
            else if (sMode == "exp")
                mode = FogMode.FOG_EXP;
            else if (sMode == "exp2")
                mode = FogMode.FOG_EXP2;
            else if (sMode == "linear")
                mode = FogMode.FOG_LINEAR;

            // Process colourDiffuse (?)
            ColourValue colourDiffuse = ColourValue.White;
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("colourDiffuse");
            if (xmlElement != null)
                colourDiffuse = ParseColour(xmlElement);

            // Setup the fog
            sceneManager.SetFog(mode, colourDiffuse, 0.001f, linearStart, linearEnd);
        }

        /// <summary>
        /// Chargement d'une lumière
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sceneNodeParent">SceneNode parent</param>
        protected void ProcessLight(XmlElement _xmlElement, SceneNode _sceneNodeParent)
        {
            // Process attributes
            string name = GetAttrib(_xmlElement, "name");

            // Create the light
            Light light = sceneManager.CreateLight(name);
            if (_sceneNodeParent != null)
                _sceneNodeParent.AttachObject(light);

            string sValue = GetAttrib(_xmlElement, "type");
            if (sValue == "point")
                light.Type = Light.LightTypes.LT_POINT;
            else if (sValue == "directional")
                light.Type = Light.LightTypes.LT_DIRECTIONAL;
            else if (sValue == "spot")
                light.Type = Light.LightTypes.LT_SPOTLIGHT;

            // only set if Lamp is Spotlight (Blender)
            bool castShadow = true;
            if (_xmlElement.HasAttribute("castShadow"))
            {
                castShadow = GetAttribBool(_xmlElement, "castShadow", true);
            }
            else if (_xmlElement.HasAttribute("castShadows"))
            {
                castShadow = GetAttribBool(_xmlElement, "castShadows", true);
            }

            light.CastShadows = castShadow;

            // Process normal (?)
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("normal");
            if (xmlElement != null)
                light.Direction = ParseVector3(xmlElement);      // Process normal (?)

            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("position");
            if (xmlElement != null)
                light.Position = ParseVector3(xmlElement);

            // Process colourDiffuse (?)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("colourDiffuse");
            if (xmlElement != null)
                light.DiffuseColour = ParseColour(xmlElement);

            // Process colourSpecular (?)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("colourSpecular");
            if (xmlElement != null)
                light.SpecularColour = ParseColour(xmlElement);

            // Process lightRange (?)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("lightRange");
            if (xmlElement != null)
                ProcessLightRange(xmlElement, light);

            // Process lightAttenuation (?)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("lightAttenuation");
            if (xmlElement != null)
                ProcessLightAttenuation(xmlElement, light);
        }

        /// <summary>
        /// Chargement aténuation d'une lumière
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_light">Light</param>
        protected void ProcessLightAttenuation(XmlElement _xmlElement, Light _light)
        {
            // Process attributes
            float range = GetAttribReal(_xmlElement, "range");
            float constant = GetAttribReal(_xmlElement, "constant");
            float linear = GetAttribReal(_xmlElement, "linear");
            float quadratic = GetAttribReal(_xmlElement, "quadratic");

            // Setup the light attenuation
            _light.SetAttenuation(range, constant, linear, quadratic);
        }

        /// <summary>
        /// Chargement portée d'une lumière
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_light">Light</param>
        protected void ProcessLightRange(XmlElement _xmlElement, Light _light)
        {
            // Process attributes
            float inner = GetAttribReal(_xmlElement, "inner");
            float outer = GetAttribReal(_xmlElement, "outer");
            float falloff = GetAttribReal(_xmlElement, "falloff", 1.0f);

            // Setup the light range
            _light.SetSpotlightRange(new Radian((Degree)inner), new Radian((Degree)outer), falloff);
        }

        /// <summary>
        /// Chargement d'une node
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sceneNodeParent">SceneNode parent</param>
        protected void ProcessNode(XmlElement _xmlElement, SceneNode _sceneNodeParent)
        {
            // Construct the node's name
            string name = sPrependNode + GetAttrib(_xmlElement, "name");

            bool bIsInitialStateSet = false;

            // Create the scene node
            SceneNode sceneNode;
            if (name.Length == 0)
            {
                // Let Ogre choose the name
                sceneNode = _sceneNodeParent != null ? _sceneNodeParent.CreateChildSceneNode() : sceneNodeAttach.CreateChildSceneNode();
            }
            else
            {
                // Provide the name
                sceneNode = _sceneNodeParent != null ? _sceneNodeParent.CreateChildSceneNode(name) : sceneNodeAttach.CreateChildSceneNode(name);
            }

            // Process other attributes

            // Process position (?)
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("position");
            if (xmlElement != null)
            {
                sceneNode.Position = ParseVector3(xmlElement);
                sceneNode.SetInitialState();
            }

            // Process quaternion (?)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("quaternion");
            if (xmlElement != null)
            {
                sceneNode.Orientation = ParseQuaternion(xmlElement);
                sceneNode.SetInitialState();
            }

            // Process rotation (?)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("rotation");
            if (xmlElement != null)
            {
                sceneNode.Orientation = ParseRotation(xmlElement);
                sceneNode.SetInitialState();
            }

            // Process scale (?)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("scale");
            if (xmlElement != null)
            {
                sceneNode.SetScale(ParseVector3(xmlElement));
                sceneNode.SetInitialState();
            }

            // Process entity (*)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("entity");
            if (xmlElement != null) ProcessEntity(xmlElement, sceneNode);


            // Process light (*)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("light");
            if (xmlElement != null) ProcessLight(xmlElement, sceneNode);

            // Process plane (*)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("plane");
            if (xmlElement != null)
            {
                ProcessPlane(xmlElement, sceneNode);
                //xmlElement = (XmlElement)xmlElement.NextSibling;
            }

            // Process plane (*)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("billboardset");
            if (xmlElement != null)
            {
                ProcessBillboardSet(xmlElement, sceneNode);
                //xmlElement = (XmlElement)xmlElement.NextSibling;
            }
            // Process camera (*)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("camera");
            if (xmlElement != null) ProcessCamera(xmlElement, sceneNode);

            // Process particleSystem (*)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("particleSystem");
            if (xmlElement != null) ProcessParticleSystem(xmlElement, sceneNode);

            // Process terrain (*)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("terrain");
            if (xmlElement != null) ProcessTerrain(xmlElement, sceneNode);

            // Process userDataReference (?)
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("userData");
            if (xmlElement != null) ProcessUserDataReference(xmlElement, sceneNode);

            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("lookTarget");
            if (xmlElement != null) LoadLookTarget(xmlElement, sceneNode, null);

            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("trackTarget");
            if (xmlElement != null) LoadTrackTarget(xmlElement, sceneNode, null);

            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("animations");
            if (xmlElement != null)
            {
                ProcessNodeAnimations(xmlElement, sceneNode);
                SetIdentityInitialState(sceneNode);
                bIsInitialStateSet = true;
            }

            // Process childnodes
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("node");
            while (xmlElement != null)
            {
                ProcessNode(xmlElement, sceneNode);
                xmlElement = (XmlElement)xmlElement.NextSibling;
            }

            //Set the initial state if it hasn't already been set
            if (!bIsInitialStateSet)
                sceneNode.SetInitialState();
        }

        /// <summary>
        /// Chargement terrain
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sceneNodeParent">SceneNode parent</param>
        protected void ProcessTerrain(XmlElement _xmlElement, SceneNode _sceneNodeParent)
        {
            float fWorldSize = GetAttribReal(_xmlElement, "worldSize");
            ushort usMapSize = ushort.Parse(_xmlElement.GetAttribute("mapSize"));
            //bool colourmapEnabled = GetAttribBool(_xmlElement, "colourmapEnabled");
            //int colourMapTextureSize = int.Parse(_xmlElement.GetAttribute("colourMapTextureSize"));
            //int compositeMapDistance = int.Parse(_xmlElement.GetAttribute("tuningCompositeMapDistance"));
            //int maxPixelError = int.Parse(_xmlElement.GetAttribute("tuningMaxPixelError"));

            //Vector3 lightdir = new Vector3(0f, -0.3f, 0.75f);
            //lightdir.Normalise();
            //Light l = mSceneMgr.CreateLight("tstLight");
            //l.Type = Light.LightTypes.LT_DIRECTIONAL;
            //l.Direction = lightdir;
            //l.DiffuseColour = new ColourValue(1.0f, 1.0f, 1.0f);
            //l.SpecularColour = new ColourValue(0.4f, 0.4f, 0.4f);
            //if (mSceneMgr.AmbientLight == null) mSceneMgr.AmbientLight=new ColourValue(0.6f, 0.6f, 0.6f);

            //TerrainGlobalOptions mTerrainGlobalOptions = new TerrainGlobalOptions
            //                                               {
            //                                                 MaxPixelError = maxPixelError,
            //                                                 CompositeMapDistance = compositeMapDistance,
            //                                                 LightMapDirection = lightdir,
            //                                                 CompositeMapAmbient = mSceneMgr.AmbientLight,
            //                                                 CompositeMapDiffuse = l.DiffuseColour
            //                                               };

            TerrainGroup mTerrainGroup = new TerrainGroup(sceneManager, Terrain.Alignment.ALIGN_X_Z, usMapSize, fWorldSize)
            {
                Origin = Vector3.ZERO,
                ResourceGroup = sGroupName
            };


            // Process terrain pages (*)
            XmlElement pElement = (XmlElement)_xmlElement.SelectSingleNode("terrainPages");
            if (pElement != null)
            {
                // ReSharper disable PossibleNullReferenceException
                foreach (XmlElement xmlElement in pElement.SelectNodes("terrainPage"))
                // ReSharper restore PossibleNullReferenceException
                {
                    ProcessTerrainPage(xmlElement, _sceneNodeParent, mTerrainGroup);
                }
            }
            mTerrainGroup.LoadAllTerrains(true);
            mTerrainGroup.FreeTemporaryResources();
            //mTerrain->setPosition(mTerrainPosition);
        }

        /// <summary>
        /// Chargement terrain
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sceneNodeParent">SceneNode parent</param>
        /// <param name="_mTerrainGroup">TerrainGroup</param>
        protected void ProcessTerrainPage(XmlElement _xmlElement, SceneNode _sceneNodeParent, TerrainGroup _mTerrainGroup)
        {
            string sName = GetAttrib(_xmlElement, "name");
            int iPageX = int.Parse(_xmlElement.GetAttribute("pageX"));
            int iPageY = int.Parse(_xmlElement.GetAttribute("pageY"));

            if (ResourceGroupManager.Singleton.ResourceExists(_mTerrainGroup.ResourceGroup, sName))
            {
                _mTerrainGroup.DefineTerrain(iPageX, iPageY, sName);
            }
        }

        /// <summary>
        /// Chargement d'un plan
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sceneNodeParent">SceneNode parent</param>
        protected void ProcessPlane(XmlElement _xmlElement, SceneNode _sceneNodeParent)
        {
            string sName = GetAttrib(_xmlElement, "name");
            //float distance = GetAttribReal(_xmlElement, "distance");
            float fWidth = GetAttribReal(_xmlElement, "width");
            float fHeight = GetAttribReal(_xmlElement, "height");

            int ixSegments = (int)GetAttribReal(_xmlElement, "xSegments");
            int iySegments = (int)GetAttribReal(_xmlElement, "ySegments");
            int iNumTexCoordSets = (int)GetAttribReal(_xmlElement, "numTexCoordSets");
            float fuTile = GetAttribReal(_xmlElement, "uTile");
            float fvTile = GetAttribReal(_xmlElement, "vTile");
            string sMaterial = GetAttrib(_xmlElement, "material");
            bool bNormals = GetAttribBool(_xmlElement, "normals");
            //bool movablePlane = GetAttribBool(_xmlElement, "movablePlane");
            bool bCastShadows = GetAttribBool(_xmlElement, "castShadows");
            //bool receiveShadows = GetAttribBool(_xmlElement, "receiveShadows");

            Vector3 v3Normal = Vector3.ZERO;
            XmlElement pElement = (XmlElement)_xmlElement.SelectSingleNode("normal");
            if (pElement != null)
                v3Normal = ParseVector3(pElement);

            Vector3 v3UpVector = Vector3.UNIT_Y;
            pElement = (XmlElement)_xmlElement.SelectSingleNode("upVector");
            if (pElement != null)
                v3UpVector = ParseVector3(pElement);

            Plane plane = new Plane(v3Normal, v3UpVector);

            try
            {
                MeshManager.Singleton.CreatePlane(sName, sGroupName, plane, fWidth, fHeight, ixSegments, iySegments, bNormals, (ushort)iNumTexCoordSets, fuTile, fvTile, v3UpVector);

                Entity entity = sceneManager.CreateEntity(sName, sName);
                entity.CastShadows = bCastShadows;

                if (!string.IsNullOrWhiteSpace(sMaterial)) entity.SetMaterialName(sMaterial);
                _sceneNodeParent.AttachObject(entity);
            }
            catch (Exception e)
            {
                LogManager.Singleton.LogMessage("[DotSceneLoader] Error loading a plane!" + e.Message);
            }
        }

        /// <summary>
        /// Chargement d'une node 
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        protected void ProcessNodes(XmlElement _xmlElement)
        {
            // Process node (*)
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("node");
            while (xmlElement != null)
            {
                ProcessNode(xmlElement, null);
                XmlNode nextNode = xmlElement.NextSibling;
                xmlElement = nextNode as XmlElement;
                while (xmlElement == null && nextNode != null)
                {
                    nextNode = nextNode.NextSibling;
                    xmlElement = nextNode as XmlElement;
                }
            }
        }

        //    <!ELEMENT billboardSet (billboard*)>
        //<!ATTLIST billboardSet
        //    name        CDATA    #REQUIRED   
        //    material    CDATA    #REQUIRED
        //    id            ID        #IMPLIED
        //    width        CDATA    #DEFAULT    "10"
        //    height        CDATA    #DEFAULT    "10"
        //    type        (orientedCommon | orientedSelf | point) "point"
        //    origin        (bottomLeft | bottomCenter | bottomRight | left | center | right | topLeft | topCenter | topRight) "center"
        //>

        /// <summary>
        /// Chargement d'un billboardset
        /// </summary>
        /// <param name="_xmlElement">xml</param>
        /// <param name="_sceneNodeParent">node auquel le billboardset est rattaché</param>
        protected void ProcessBillboardSet(XmlElement _xmlElement, SceneNode _sceneNodeParent)
        {
            string sName = GetAttrib(_xmlElement, "name");
            string sMaterial = GetAttrib(_xmlElement, "material");

            BillboardSet billboardSet = sceneManager.CreateBillboardSet(sName);
            billboardSet.DefaultHeight = GetAttribReal(_xmlElement, "height", 10);
            billboardSet.DefaultWidth = GetAttribReal(_xmlElement, "width", 10);

            switch (GetAttrib(_xmlElement, "type", "point"))
            {
                case "orientedCommon":
                    billboardSet.BillboardType = BillboardType.BBT_ORIENTED_COMMON;
                    break;
                case "orientedSelf":
                    billboardSet.BillboardType = BillboardType.BBT_ORIENTED_SELF;
                    break;
                case "point":
                    billboardSet.BillboardType = BillboardType.BBT_POINT;
                    break;
                case "perpendicularCommon":
                    billboardSet.BillboardType = BillboardType.BBT_PERPENDICULAR_COMMON;
                    break;
                case "perpendicularSelf":
                    billboardSet.BillboardType = BillboardType.BBT_PERPENDICULAR_SELF;
                    break;

                default:
                    billboardSet.BillboardType = BillboardType.BBT_ORIENTED_COMMON;
                    LogManager.Singleton.LogMessage("[DotSceneLoader] Error loading a billboardSet! Unknown type");
                    break;
            }


            billboardSet.SetMaterialName(sMaterial);
            switch (GetAttrib(_xmlElement, "origin", "center"))
            {
                case "bottomLeft":
                    billboardSet.BillboardOrigin = BillboardOrigin.BBO_BOTTOM_LEFT;
                    break;
                case "bottomCenter":
                    billboardSet.BillboardOrigin = BillboardOrigin.BBO_BOTTOM_CENTER;
                    break;
                case "bottomRight":
                    billboardSet.BillboardOrigin = BillboardOrigin.BBO_BOTTOM_RIGHT;
                    break;
                case "left":
                    billboardSet.BillboardOrigin = BillboardOrigin.BBO_CENTER_LEFT;
                    break;
                case "center":
                    billboardSet.BillboardOrigin = BillboardOrigin.BBO_TOP_CENTER;
                    break;
                case "right":
                    billboardSet.BillboardOrigin = BillboardOrigin.BBO_CENTER_RIGHT;
                    break;
                case "topLeft":
                    billboardSet.BillboardOrigin = BillboardOrigin.BBO_TOP_LEFT;
                    break;
                case "topCenter":
                    billboardSet.BillboardOrigin = BillboardOrigin.BBO_TOP_CENTER;
                    break;
                case "topRight":
                    billboardSet.BillboardOrigin = BillboardOrigin.BBO_TOP_RIGHT;
                    break;
                default:
                    billboardSet.BillboardOrigin = BillboardOrigin.BBO_BOTTOM_CENTER;
                    LogManager.Singleton.LogMessage("[DotSceneLoader] Error loading a billboardSet! Unknown origin");
                    break;
            }

            _sceneNodeParent.AttachObject(billboardSet);

            // Process childnodes
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("billboard");
            while (xmlElement != null)
            {
                ProcessBillboard(xmlElement, billboardSet);
                xmlElement = (XmlElement)xmlElement.NextSibling;
            }

        }

        /// <summary>
        /// Création d'un billboard
        /// </summary>
        /// <param name="_xmlElement">xml</param>
        /// <param name="_billboardSet">billboardset auquel le billboard est rattaché</param>
        protected void ProcessBillboard(XmlElement _xmlElement, BillboardSet _billboardSet)
        {
            Vector3 v3Position = new Vector3();
            XmlElement xmlElement = (XmlElement)_xmlElement.SelectSingleNode("position");
            if (xmlElement != null)
            {
                v3Position = ParseVector3(xmlElement);
            }
            Billboard billboard = _billboardSet.CreateBillboard(v3Position);
            float fRotation = 0;
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("rotation");
            if (xmlElement != null)
            {
                fRotation = GetAttribReal(_xmlElement, "angle", 0);
            }
            billboard.Rotation = fRotation;

            ColourValue colourValue = ColourValue.White;
            xmlElement = (XmlElement)_xmlElement.SelectSingleNode("colourDiffuse");
            if (xmlElement != null)
            {
                colourValue = ParseColour(xmlElement);
            }
            billboard.Colour = colourValue;
            if (_xmlElement.HasAttribute("width") && _xmlElement.HasAttribute("height")) billboard.SetDimensions(GetAttribReal(_xmlElement, "width"), GetAttribReal(_xmlElement, "height"));
        }


        /// <summary>
        /// Chargement scène
        /// </summary>
        /// <param name="_xmlRoot">XmlElement</param>
        protected void ProcessScene(XmlElement _xmlRoot)
        {
            // Process the scene parameters
            string version = GetAttrib(_xmlRoot, "formatVersion", "unknown");

            string message = "[DotSceneLoader] Parsing dotScene file with version " + version;

            LogManager.Singleton.LogMessage(message);

            // Process nodes (?)
            XmlElement xmlElement = (XmlElement)_xmlRoot.SelectSingleNode("nodes");
            if (xmlElement != null)
                ProcessNodes(xmlElement);

            FinishLoadingLookAndTrackTargets();

            // Process environment (?)
            xmlElement = (XmlElement)_xmlRoot.SelectSingleNode("environment");
            if (xmlElement != null) ProcessEnvironment(xmlElement);
            // Process externals (?)
            //         pElement = (XmlElement)XMLRoot.SelectSingleNode("externals");
            //         if (pElement != null)
            //            ProcessExternals(pElement);
        }

        /// <summary>
        /// Chargement référence de donnée utilisateur
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sceneNode">SceneNode</param>
        protected void ProcessUserDataReference(XmlElement _xmlElement, SceneNode _sceneNode)
        {
            // TODO
        }

        /// <summary>
        /// Chargement système de particule
        /// </summary>
        /// <param name="_xmlElement">XmlElement</param>
        /// <param name="_sceneNode">SceneNode</param>
        protected void ProcessParticleSystem(XmlElement _xmlElement, SceneNode _sceneNode)
        {
            // Process attributes
            string sName = GetAttrib(_xmlElement, "name");
            string sFile = GetAttrib(_xmlElement, "file");

            // Create the particle system
            try
            {
                ParticleSystem particleSystem = sceneManager.CreateParticleSystem(sName, sFile);
                _sceneNode.AttachObject(particleSystem);
            }
            catch (Exception e)
            {
                LogManager.Singleton.LogMessage("[DotSceneLoader] Error loading an Particle system!" + e.Message);
            }
        }

        private void LoadLookTarget(XmlElement _xmlElement, SceneNode _sceneNode, Camera _camera)
        {
            LookTarget lookTarget = new LookTarget(_sceneNode, _camera)
            {
                NodeName = GetAttrib(_xmlElement, "nodeName")
            };

            string relativeTo = GetAttrib(_xmlElement, "relativeTo");
            if (!string.IsNullOrEmpty(relativeTo))
                lookTarget.RelativeTo = (Node.TransformSpace)Enum.Parse(typeof(Node.TransformSpace), relativeTo);


            foreach (XmlElement childElement in _xmlElement.ChildNodes)
            {
                switch (childElement.Name)
                {
                    case "position":
                        lookTarget.Position = ParseVector3(childElement);
                        lookTarget.IsPositionSet = true;
                        break;
                    case "localDirection":
                        lookTarget.LocalDirection = ParseVector3(childElement);
                        break;
                }
            }

            lookTargets.Add(lookTarget);
        }

        private void LoadTrackTarget(XmlElement _xmlElement, SceneNode _sceneNode, Camera _camera)
        {
            TrackTarget trackTarget = new TrackTarget(_sceneNode, _camera)
            {
                NodeName = GetAttrib(_xmlElement, "nodeName")
            };

            foreach (XmlElement xmlElement in _xmlElement.ChildNodes)
            {
                switch (xmlElement.Name)
                {
                    case "offset":
                        trackTarget.Offset = ParseVector3(xmlElement);
                        break;
                    case "localDirection":
                        trackTarget.LocalDirection = ParseVector3(xmlElement);
                        break;
                }
            }

            trackTargets.Add(trackTarget);
        }


        private void FinishLoadingLookAndTrackTargets()
        {
            // ReSharper disable ForCanBeConvertedToForeach
            for (int i = 0; i < lookTargets.Count; i++)
            // ReSharper restore ForCanBeConvertedToForeach
            {
                LookTarget lookTarget = lookTargets[i];
                SceneNode lookTargetNode = string.IsNullOrEmpty(lookTarget.NodeName) ? null : sceneManager.GetSceneNode(lookTarget.NodeName);
                Vector3 position = new Vector3();
                if (lookTarget.IsPositionSet) position = lookTarget.Position;
                else
                {
                    lookTarget.RelativeTo = Node.TransformSpace.TS_WORLD;
                    if (lookTargetNode != null) position = lookTargetNode._getDerivedPosition();
                }
                if (lookTarget.SourceNode != null)
                    lookTarget.SourceNode.LookAt(position, lookTarget.RelativeTo, lookTarget.LocalDirection);
                else if (lookTarget.SourceCamera != null)
                    lookTarget.SourceCamera.LookAt(position);
            }
            lookTargets.Clear();

            foreach (TrackTarget trackTarget in trackTargets)
            {
                SceneNode trackTargetNode = sceneManager.GetSceneNode(trackTarget.NodeName);
                if (trackTarget.SourceNode != null)
                    trackTarget.SourceNode.SetAutoTracking(true, trackTargetNode, trackTarget.LocalDirection, trackTarget.Offset);
                else
                {
                    if (trackTarget.SourceCamera != null) trackTarget.SourceCamera.SetAutoTracking(true, trackTargetNode, trackTarget.Offset);
                }
            }
            trackTargets.Clear();
        }

        private void SetIdentityInitialState(SceneNode _node)
        {
            //Get the current state
            Vector3 position = _node.Position;
            Quaternion orientation = _node.Orientation;
            Vector3 scale = _node.GetScale();

            //Set the initial state to be at identity
            _node.SetPosition(Vector3.ZERO.x, Vector3.ZERO.y, Vector3.ZERO.z);
            _node.SetOrientation(Quaternion.IDENTITY.w, Quaternion.IDENTITY.x, Quaternion.IDENTITY.y, Quaternion.IDENTITY.z);
            _node.SetScale(Vector3.UNIT_SCALE);
            _node.SetInitialState();

            //Set the current state so the node is in the correct position if the node has
            //animations that are initially disabled
            _node.SetPosition(position.x, position.y, position.z);
            _node.SetOrientation(orientation.w, orientation.x, orientation.y, orientation.z);
            _node.SetScale(scale);
        }
    }
}