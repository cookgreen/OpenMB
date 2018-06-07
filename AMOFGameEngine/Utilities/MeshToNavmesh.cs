/* @Author: Aka Aralox
 * @Modify: Cook Green
 * --------------------------------------------------------------------------------------------------------------
 * Created by Pravin (Aka Aralox) <aralox@gmail.com>
 * Please email me if you have any questions, comments or suggestions about this class :)
 * 
 * See here for the discussion that helped me create this:
 * http://groups.google.com/group/critterai/browse_thread/thread/598d668fd9b3e401
 * A huge amount of thanks goes to Stephen A. Pratt for the hard work he put into creating CritterAI,
 * and for giving me so much help with this. Thanks also goes to Mikko Mononen for creating the wonderful Recast.
 * The goal of this class is to load an arbitarily created mesh, and create a NavmeshTileBuildData (and Navmesh) from it.
 * 
 * Dependencies:
 * - only cai-nav, cai-nav-rcn and cai-util (and System namespace) are 100% necessary dependencies.
 * - System.Windows.Forms is used in ReportError, change it to whatever you use.
 * - Mogre is used for the LoadNavmesh(DataStreamPtr dataPtr) and DataPtrToStream(DataStreamPtr dataPtr) which are pretty cool.
 * 
 * Important Notes:
 * - This will load only one Navmesh Tile, if you need multiple tiles, feel free to build upon this class.
 * - You must compile your project with /unsafe, for DataPtrToStream(). If you dont use Mogre, comment out this method.
 * - Max polygons per vertex is 3 (hardcoded here and there), so make sure exported mesh is triangulated.
 * - Call Customize before LoadNavmesh to put in your own custom values (especially the xz and y cell sizes)
 * - I know there are many many easy optimisations i can make to this code, but i wanted to keep it simple,
 *   as performance here is not an issue, future understandability is much more important.
 * - You can change the upper limits of verts and polys if your mesh has more than these.
 * */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using org.critterai.nav;
using Mogre;
using Math = System.Math;
using System.Runtime.InteropServices;

public static class MeshToNavmesh
{
    //Constants
    const float LOWER_LIMIT = -10000;       //Used when looking for the navmesh bounds
    const float UPPER_LIMIT = 10000;
    const int maxPolyVerts = 300000;
	const int maxPolys = 100000;            //a hundred thousand polygons should be a good upper limit
    const int maxVertsPerPoly = 3;

    //used in LoadBase() http://www.critterai.org/projects/cainav/doc/html/2764CED0.htm
    private static int tileX = 0;
    private static int tileZ = 0;
    private static int tileLayer = 0;
    private static uint tileUserId = 1;
    private static org.critterai.Vector3 boundsMin = new org.critterai.Vector3();      //lowest (x,y,z)       Note: CLR initializes to zero
    private static org.critterai.Vector3 boundsMax = new org.critterai.Vector3();       //highest (x,y,z)
    private static float xzCellSize = 0.30f;      //These two are important. If using the recast demo to generate your navmesh,
    private static float yCellSize = 0.20f;       //Use the same values here
    private static float walkableHeight = 2.0f;   //Redundant
    private static float walkableRadius = 2.0f;   //Redundant
    private static float walkableStep = 1.0f;     //Redundant
    private static bool bvTreeEnabled;

    //used in LoadPolys()
    private static ushort[] polyVerts;
    private static int vertCount;
    private static ushort[] polys;
    private static ushort[] polyFlags;
    private static byte[] polyAreas;
    private static int polyCount;

    //Navmesh objects
    private static NavmeshTileBuildData navData;
    private static Navmesh navmesh;

    //Convenience class for holding lists (Note: I could also have used a [jagged/normal] array, but more fun doing this)
    class MyVector3 <T>
    {
        public T x;
        public T y;
        public T z;

        public MyVector3(T a, T b, T c)
        {
            x = a;
            y = b;
            z = c;
        }

        public MyVector3()
        {
            x = default(T);
            y = default(T);
            z = default(T);
        }

        //Indexer
        public T this[int position]
        {
            get
            {
                if (position == 0)
                    return x;
                else if (position == 1)
                    return y;
                else if (position == 2)
                    return z;
                else
                    ReportError("Position only goes from 0 to 2, in MyVector3");
                return default(T);
            }

            set
            {
                if (position == 0)
                    x = value;
                else if (position == 1)
                    y = value;
                else if (position == 2)
                    z = value;
                else
                    System.Windows.Forms.MessageBox.Show("Position only goes from 0 to 2, in MyVector3", "Error in MeshToNavmesh");
            }
        }

    }

    //Remember, the index of the list is the vertex/polygon number
    private static List<MyVector3<float>> vertices = new List<MyVector3<float>>();
    private static List<MyVector3<ushort>> faces = new List<MyVector3<ushort>>();
    private static List<MyVector3<ushort>> neighborPolys = new List<MyVector3<ushort>>();

    //private static methods can be accessed by nested classes (like MyVector3)
    private static void ReportError(string error)
    {
        System.Windows.Forms.MessageBox.Show(error, "Error in MeshToNavmesh");
    }

    //See here: http://www.ogre3d.org/addonforums/viewtopic.php?f=8&t=29287&p=98241#p98241
    public static MemoryStream DataPtrToStream(DataStreamPtr dataPtr)
    {
        if (dataPtr.Size() != 0)
        {
            byte[] buffer = new byte[dataPtr.Size()];

            unsafe
            {
                //Get the pointer to the first element of our buffer of bytes (in C++, can just use 'buffer')
                fixed (byte* bufferPtr = &buffer[0])
                {
                    //Read buffer.Length amount of data into bufferPtr
                    dataPtr.Read(bufferPtr, (uint)buffer.Length);
                }
            }

            MemoryStream stream = new MemoryStream(buffer);
            
            return stream;

        }
        return null;
    }

    //Overload for mogre resource name string
    public static Navmesh MogreLoadNavmesh(string resourceFile)
    {
        return MogreLoadNavmesh(ResourceGroupManager.Singleton.OpenResource(resourceFile));
    }


    //Overload for mogre resources
    //Tip: Use DataStreamPtr dataPtr = ResourceGroupManager.Singleton.OpenResource("myNavmesh.obj");
    public static Navmesh MogreLoadNavmesh(DataStreamPtr dataPtr)
    {
        Stream stream = DataPtrToStream(dataPtr);

        if (stream == null)
        {
            ReportError("Error: Null stream from Data pointer");
            return null;
        }

        return LoadNavmesh(stream);
    }

    //Overload for direct file name
    public static Navmesh LoadNavmesh(string directFileName)
    {
        FileStream fileStream = new FileStream(directFileName, FileMode.Open);
        return LoadNavmesh(fileStream);
    }

    //Creates a Navmesh from the given Stream
    public static Navmesh LoadNavmesh(Stream fileStream)
    {
        ReadFromOBJ(fileStream);

        return GenerateNavmesh();
    }

    private static Navmesh GenerateNavmesh()
    {
        #region Generate Neighboring polygon data

        //Then generate neighboring polygon data, by parsing the face list
        MyVector3<bool> sharedVertex;   //For the current face, what vertices are shared with the other face?
        int sharedVertices;             //if goes to 2, edge is shared

        for (ushort q = 0; q < faces.Count; q++)
        {
            //Index of face and neighborPoly refer to the same polygon.
            neighborPolys.Add(new MyVector3<ushort>());
            neighborPolys[q].x = Navmesh.NullIndex;
            neighborPolys[q].y = Navmesh.NullIndex;
            neighborPolys[q].z = Navmesh.NullIndex;

            //Compare this face with every other face
            for (ushort w = 0; w < faces.Count; w++)
                if (w != q)
                {
                    sharedVertices = 0;
                    sharedVertex = new MyVector3<bool>();

                    //Go from left to right in the face MyVector3
                    for (int j = 0; j <= 2; j++)
                    {
                        //And compare each index with every other index 
                        for (int k = 0; k <= 2; k++)
                        {
                            if (faces[q][j] == faces[w][k])
                            {
                                //If we find a matching index, update stuff (only for the current face, dont bother with other face, can optimise but will be confusing)
                                sharedVertices++;
                                sharedVertex[j] = true; //could break out of the for loop now, as face will not list the same index twice
                            }
                        }
                    }
                    if (sharedVertices > 2) ReportError("error: more than 2 vertices shared between polys " + q + " and " + w);

                    //Check if these faces are sharing an edge
                    if (sharedVertices == 2)
                    {
                        //get the Leftmost Right-To-Left Pair in the neighborPolys MyVector3 
                        //options are: edge 0-1, 1-2, and 2-0, respectively indexing neighboringPolys. (i.e. if index 1 of neighboring polys is 45, that means that the current polygon and polygon 45 share the edge face[1] <-> face[2]
                        if (sharedVertex[0] == true)
                        {
                            if (sharedVertex[1] == true)
                                neighborPolys[q][0] = w;        //I.e. tell this face's MyVector3 of neighboring polygons that the edge made up by vertices at 0 and 1 is shared between polygon q and w
                            else
                                neighborPolys[q][2] = w;
                        }
                        else
                        {
                            neighborPolys[q][1] = w;
                        }

                    }

                } //End iterating through other faces

        } //End iterating through each face
        #endregion

        //Now, Load these into Critter AI and create a navmesh
        navData = new NavmeshTileBuildData(maxPolyVerts, maxPolys, maxVertsPerPoly, 0, 0, 0);

        #region LoadBase
        //Get the min and max bounds from the vertex positions
        float lowest;
        float highest;

        //Find the bounds of the mesh. iterate through the x, y and z axes
        for (int axis = 0; axis <= 2; axis++)
        {
            lowest = UPPER_LIMIT;    //set to inital values that they do not reach
            highest = LOWER_LIMIT;

            //iterate through every vertex to find highest and lowest value of this axis
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i][axis] < lowest)
                {
                    lowest = vertices[i][axis];
                }

                if (vertices[i][axis] > highest)
                {
                    highest = vertices[i][axis];
                }
            }

            if (axis == 0)//x
            {
                boundsMin.x = lowest;
                boundsMax.x = highest;
            }
            else if (axis == 1)
            {
                boundsMin.y = lowest;
                boundsMax.y = highest;
            }
            else if (axis == 2)
            {
                boundsMin.z = lowest;
                boundsMax.z = highest;
            }

        }

        bool sucess;
        sucess = navData.LoadBase(tileX, tileZ, tileLayer, tileUserId, boundsMin, boundsMax, xzCellSize, yCellSize, walkableHeight, walkableRadius, walkableStep, bvTreeEnabled);
        if (!sucess)
            ReportError("Error, LoadBase returned false");

        #endregion

        #region LoadPolys
        vertCount = vertices.Count;
        polyCount = faces.Count;

        //Convert vertices from world space to grid space 
        polyVerts = new ushort[vertCount * 3];

        for (int i = 0; i < vertCount; i++)
        {
            polyVerts[3 * i + 0] = (ushort)Math.Round((vertices[i].x - boundsMin.x) / xzCellSize);
            polyVerts[3 * i + 1] = (ushort)Math.Round((vertices[i].y - boundsMin.y) / yCellSize);
            polyVerts[3 * i + 2] = (ushort)Math.Round((vertices[i].z - boundsMin.z) / xzCellSize);
        }

        //build polys array (http://www.critterai.org/projects/cainav/doc/html/B8C2F0F4.htm)
        polys = new ushort[6 * polyCount];
        int ind = 0;
        int faceNo = 0;

        while (faceNo < polyCount)
        {
            polys[ind + 0] = faces[faceNo].x;
            polys[ind + 1] = faces[faceNo].y;
            polys[ind + 2] = faces[faceNo].z;

            polys[ind + 3] = neighborPolys[faceNo].x;
            polys[ind + 4] = neighborPolys[faceNo].y;
            polys[ind + 5] = neighborPolys[faceNo].z;

            ind += 6;
            faceNo++;
        }

        //Fill polyflags array with default flags
        polyFlags = new ushort[polyCount];
        for (int i = 0; i < polyCount; i++)
            polyFlags[i] = 1;               //custom user flag

        //Fill polyAreas array
        polyAreas = new byte[polyCount];
        for (int i = 0; i < polyCount; i++)
            polyAreas[i] = 1;

        sucess = navData.LoadPolys(polyVerts, vertCount, polys, polyFlags, polyAreas, polyCount);

        if (!sucess)
            ReportError("Error, LoadPolys returned false");

        #endregion

        //Build the Navmesh using the navData
        NavStatus status = Navmesh.Create(navData, out navmesh);

        if (status != NavStatus.Sucess)
            ReportError("Navmesh build status was " + status.ToString());

        return navmesh;
    }

    //Loads the vertex and poly data into our Lists
    private static void ReadFromOBJ(Stream fileStream)
    {
        StreamReader reader = new StreamReader(fileStream);
        string line;

        Regex floatNumber = new Regex(@"[\d\-][\d\.]*");
        Regex ushortNumber = new Regex(@"\d+");
        MatchCollection matchList;

        while ((line = reader.ReadLine()) != null)
        {
            //Read vertices
            if (line.Substring(0, 2) == "v ")
            {
                matchList = floatNumber.Matches(line);
                float x = Convert.ToSingle(matchList[0].ToString());
                float y = Convert.ToSingle(matchList[1].ToString());
                float z = Convert.ToSingle(matchList[2].ToString());
                vertices.Add(new MyVector3<float>(x, y, z));
            }

            //Read faces
            else if (line.Substring(0, 2) == "f ")
            {
                //Error here where invalid indices were given. This is because the OBJ file started indexing the verts from 1 instead of 0.
                matchList = ushortNumber.Matches(line);
                int v1 = -1 + Convert.ToUInt16(matchList[0].ToString());
                int v2 = -1 + Convert.ToUInt16(matchList[1].ToString());
                int v3 = -1 + Convert.ToUInt16(matchList[2].ToString());
                faces.Add(new MyVector3<ushort>((ushort)v1, (ushort)v2, (ushort)v3));
            }
        }
    }

    /**************** New Functions Begin ***************/
    /// <summary>
    /// Generate navmesh by entity
    /// </summary>
    /// <param name="ent">Ogre Entity</param>
    /// <returns>Navmesh</returns>

    public static Navmesh LoadNavmesh(Entity ent)
    {
        bool addedSharedVertex = false;
        vertices.Clear();
        faces.Clear();
        MeshPtr mesh = ent.GetMesh();
        Mesh.SubMeshIterator subIterator = mesh.GetSubMeshIterator();

        uint vertexNum = 0;
        uint vertexOffset = mesh.sharedVertexData.vertexStart;
        MyVector3<float>[] verticeArray = new MyVector3<float>[vertexNum];
        VertexElement posElem = mesh.sharedVertexData.vertexDeclaration.FindElementBySemantic(VertexElementSemantic.VES_POSITION);
        HardwareVertexBufferSharedPtr vertexBuffer = mesh.sharedVertexData.vertexBufferBinding.GetBuffer(posElem.Source);

        while (subIterator.MoveNext())
        {
            SubMesh subMesh = subIterator.Current;

            VertexData vertexData = subMesh.useSharedVertices ? mesh.sharedVertexData : subMesh.vertexData;

            HardwareIndexBufferSharedPtr indexBuffer = subMesh.indexData.indexBuffer;
            HardwareIndexBuffer.IndexType indexType = indexBuffer.Type;
            uint indexCount = subMesh.indexData.indexCount;

            uint trisNum = indexCount / 3;

            uint[] indcies = new uint[indexCount];
            uint indexOffset = subMesh.indexData.indexStart;

            if (subMesh.useSharedVertices)
            {
                if (!addedSharedVertex)
                {
                    vertexNum += mesh.sharedVertexData.vertexCount;
                    addedSharedVertex = true;
                }
            }
            else
            {
                vertexNum += subMesh.vertexData.vertexCount;
            }

            unsafe
            {
                uint* pLong = (uint*)(indexBuffer.Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY));
                ushort* pShort = (ushort*)pLong;
                for (int i = 0; i < indexCount; i++)
                {
                    if (indexType == HardwareIndexBuffer.IndexType.IT_32BIT)
                    {
                        indcies[indexOffset] = pLong[i] + vertexNum;
                    }
                    else
                    {
                        indcies[indexOffset] = pShort[i] + vertexNum;
                    }
                    indexOffset++;
                }
            }

            int indexLength = indcies.Length / 3;
            for (int i = 0; i < indexLength; i++)
            {
                faces.Add(new MyVector3<ushort>(
                        (ushort)indcies[i * 3 + 0],
                        (ushort)indcies[i * 3 + 1],
                        (ushort)indcies[i * 3 + 2]
                    ));
            }

            indexBuffer.Unlock();

            if (subMesh.vertexData != null)
            {
                vertexNum = subMesh.vertexData.vertexCount;
                vertexOffset = subMesh.vertexData.vertexStart;
                verticeArray = new MyVector3<float>[vertexNum];
                posElem = subMesh.vertexData.vertexDeclaration.FindElementBySemantic(VertexElementSemantic.VES_POSITION);
                vertexBuffer = subMesh.vertexData.vertexBufferBinding.GetBuffer(posElem.Source);
                unsafe
                {
                    byte* vertexMemory = (byte*)vertexBuffer.Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);
                    float* pVertexBuffer;
                    for (int i = 0; i < vertexNum; i++)
                    {
                        posElem.BaseVertexPointerToElement(vertexMemory, &pVertexBuffer);
                        verticeArray[vertexOffset] = (new MyVector3<float>(
                            pVertexBuffer[0],
                            pVertexBuffer[1],
                            pVertexBuffer[2]
                        ));
                        vertexMemory += vertexBuffer.VertexSize;
                        vertexOffset++;
                    }
                }
                for (int i = 0; i < verticeArray.Length; i++)
                {
                    vertices.Add(verticeArray[i]);
                }
                vertexBuffer.Unlock();
            }
        }

        vertexNum = mesh.sharedVertexData.vertexCount;
        vertexOffset = mesh.sharedVertexData.vertexStart;
        verticeArray = new MyVector3<float>[vertexNum];
        posElem = mesh.sharedVertexData.vertexDeclaration.FindElementBySemantic(VertexElementSemantic.VES_POSITION);
        vertexBuffer = mesh.sharedVertexData.vertexBufferBinding.GetBuffer(posElem.Source);

        unsafe 
        {
            byte* vertexMemory = (byte*)vertexBuffer.Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);
            float* pVertexBuffer;
            for (int i = 0; i < vertexNum; i++)
            {
                posElem.BaseVertexPointerToElement(vertexMemory, &pVertexBuffer);
                verticeArray[vertexOffset]=(new MyVector3<float>(
                    pVertexBuffer[0],
                    pVertexBuffer[1],
                    pVertexBuffer[2]
                ));
                vertexMemory += vertexBuffer.VertexSize;
                vertexOffset++;
            }
        }
        for (int i = 0; i < verticeArray.Length; i++)
        {
            vertices.Add(verticeArray[i]);
        }
        vertexBuffer.Unlock();
        
        return GenerateNavmesh();
    }

    /// <summary>
    /// Generate navmesh by TerrainGroup
    /// </summary>
    /// <param name="group">Terrain</param>
    /// <returns>Navmesh</returns>
    public static Navmesh LoadNavmesh(TerrainGroup group)
    {
        TerrainGroup.TerrainIterator terrainIterator = group.GetTerrainIterator();
        while (terrainIterator.MoveNext())
        {
            TerrainGroup.TerrainSlot slot = terrainIterator.Current;
            Terrain terrain = slot.instance;
        }

        return GenerateNavmesh();
    }
    /**************** New Functions End ***************/

    //used for customization of data
    public static void Customize(
        float xzCellSize,
        float yCellSize,
        bool bvTreeEnabled = false,
        int tileX = 0,
        int tileZ = 0,
        int tileLayer = 0,
        uint tileUserId = 1,
        float walkableHeight = 2.0f,
        float walkableRadius = 2.0f,
        float walkableStep = 1.0f)
    {
        MeshToNavmesh.xzCellSize = xzCellSize;
        MeshToNavmesh.yCellSize = yCellSize;
        MeshToNavmesh.bvTreeEnabled = bvTreeEnabled;
        MeshToNavmesh.tileX = tileX;
        MeshToNavmesh.tileZ = tileZ;
        MeshToNavmesh.tileLayer = tileLayer;
        MeshToNavmesh.tileUserId = tileUserId;
        MeshToNavmesh.walkableHeight = walkableHeight;
        MeshToNavmesh.walkableRadius = walkableRadius;
        MeshToNavmesh.walkableStep = walkableStep;
    }


    #region Output Methods (To write data to a file)

    public static void OutputSharedPolys()
    {
        string filename = "NeighborPolys.txt";
        string line;
        if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);
        line = "Vertex count: " + vertices.Count + '\n' + "Polygon count: " + faces.Count + '\n';
        File.AppendAllText(filename, line);

        for (int i = 0; i < neighborPolys.Count; i++)
        {
            //write to file
            line = "Neighboring polys of poly no. " + i + ":\n\tedge 0-1: " + neighborPolys[i].x + "\n\tedge 1-2: " + neighborPolys[i].y + "\n\tedge 2-0: " + neighborPolys[i].z + "\n";
            System.IO.File.AppendAllText(filename, line);
            Console.WriteLine(line);
        }

    }

    public static void OutputVertices()
    {
        NavmeshTile tile = navmesh.GetTile(0);

        float[] headerVerts = new float[tile.GetHeader().vertCount * 3];
        org.critterai.Vector3[] buffer = new org.critterai.Vector3[tile.GetHeader().vertCount * 3];
        tile.GetVerts(buffer);

        string filename = "Vertices.txt";
        if (File.Exists(filename)) File.Delete(filename);
        File.AppendAllText(filename, "Left List is Raw (vertcount = " + vertCount + 
                           "), Right List is From Header (vertcount = " + tile.GetHeader().vertCount+") (you might see data loss, as converted in/out of grid space.) \n\n");

        for (int i = 0; i < vertCount; i++)
        {
            File.AppendAllText(filename, "Vertex " + i + ": x: " + vertices[i].x + " y: " + vertices[i].y + " z: " + vertices[i].z + "\t\t|");
            File.AppendAllText(filename, "Vertex " + i + ": x: " + buffer[3 * i + 0].x + " y: " + headerVerts[3 * i + 1] + " z: " + headerVerts[3 * i + 2] + "\n");
        }

    }

    public static void OutputFaces()
    {
        string filename = "Faces.txt";
        if (File.Exists(filename)) File.Delete(filename);

        for (int i = 0; i < faces.Count; i++)
        {
            File.AppendAllText(filename, "Face " + i + ": v1: " + faces[i].x + " v2: " + faces[i].y + " v3: " + faces[i].z + "\n");
        }
    }

    #endregion
}

