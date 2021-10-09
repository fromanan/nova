using UnityEngine;
using System.Linq;
using System.Text;
using NovaCore.Library.Files;

namespace Nova.Library.Utilities
{
    // Written for exporting uTerrains to OBJ files
    public static class MeshExporter
    {
        public static string UTERRAINS_CHUNK_PATH = 
            FileSystem.BuildPath(Application.dataPath, "Art", "Meshes", "uTerrains");

        public static string MeshToString(MeshFilter mf)
        {
            if (!(mf.sharedMesh is Mesh m))
            {
                Debug.LogWarning("Ignoring Saving of Uninitialized Mesh");
                return null;
            }

            if (m.vertexCount < 1)
            {
                Debug.LogWarning("Ignoring Empty Mesh");
                return null;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(MeshToString(m));

            if (mf.GetComponent<Renderer>() is Renderer renderer)
            {
                Material[] mats = renderer.sharedMaterials;

                for (int material = 0; material < m.subMeshCount; material++)
                {
                    sb.AppendLine($"\nusemtl {mats[material].name}\nusemap {mats[material].name}");

                    int[] triangles = m.GetTriangles(material);
                    for (int i = 0; i < triangles.Length; i += 3)
                    {
                        int a = triangles[i] + 1;
                        int b = triangles[i + 1] + 1;
                        int c = triangles[i + 2] + 1;
                        sb.AppendLine(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}", a, b, c));
                    }
                }
            }

            return sb.ToString();
        }

        public static string MeshToString(Mesh m)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"g {m.name}");
            m.vertices.ToList().ForEach(v => sb.AppendLine($"v {v.x} {v.y} {v.z}"));
            sb.AppendLine();
            m.normals.ToList().ForEach(v => sb.AppendLine($"vn {v.x} {v.y} {v.z}"));
            sb.AppendLine();
            m.uv.ToList().ForEach(v => sb.AppendLine($"vt {v.x} {v.y}"));
            return sb.ToString();
        }

        public static void MeshToFile(MeshFilter mf) => MeshToFile(mf, FileSystem.Guid());

        public static void MeshToFile(Mesh m) => MeshToFile(m, FileSystem.Guid());
        
        public static string MeshPath(string filename) => 
            FileSystem.BuildPath(UTERRAINS_CHUNK_PATH, $"{filename}.obj");

        public static void MeshToFile(MeshFilter mf, string filename) => SaveToFile(MeshToString(mf), filename);

        public static void MeshToFile(Mesh mesh, string filename) => SaveToFile(MeshToString(mesh), filename);

        private static void SaveToFile(string meshString, string filename)
        {
            if (string.IsNullOrEmpty(meshString)) return;
            
            Debug.Log("Started Save Operation");
            
            string filepath = MeshPath(filename);
            FileSystem.SaveToFile(meshString, filepath);

            Debug.Log($"Successfully Saved file to {filepath}");
        }

        public static Mesh CopyMesh(Mesh mesh)
        {
            Mesh newMesh = new Mesh
            {
                vertices = mesh.vertices,
                triangles = mesh.triangles,
                uv = mesh.uv,
                normals = mesh.normals,
                colors = mesh.colors,
                tangents = mesh.tangents
            };
            //AssetDatabase.CreateAsset(newMesh, mesh.name + " copy.asset");
            return newMesh;
        }
    }
}