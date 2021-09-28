using System;
using UnityEngine;
using System.IO;
using System.Text;
using NovaCore.Library.Files;

namespace Nova.Library.Utilities
{
    // Written for exporting uTerrains to OBJ files
    public static class ObjExporter
    {
        public static string UTERRAINS_CHUNK_PATH = 
            FileSystem.BuildPath(Application.dataPath, "Art", "Meshes", "uTerrains");

        public static string MeshToString(MeshFilter mf)
        {
            Mesh m = mf.sharedMesh;

            if (!m)
            {
                Debug.LogWarning("Ignoring Saving of Uninitialized Mesh");
                return null;
            }

            if (m.vertexCount < 1)
            {
                Debug.LogWarning("Ignoring Empty Mesh");
                return null;
            }

            Renderer renderer = mf.GetComponent<Renderer>();
            bool useMaterials = renderer;

            Material[] mats = renderer.sharedMaterials;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"g {mf.name}");
            foreach (Vector3 v in m.vertices)
            {
                sb.AppendLine($"v {v.x} {v.y} {v.z}");
            }

            sb.AppendLine();
            foreach (Vector3 v in m.normals)
            {
                sb.Append($"vn {v.x} {v.y} {v.z}\n");
            }

            sb.AppendLine();
            foreach (Vector3 v in m.uv)
            {
                sb.AppendLine($"vt {v.x} {v.y}");
            }

            if (!useMaterials) return sb.ToString();

            for (int material = 0; material < m.subMeshCount; material++)
            {
                sb.AppendLine();
                sb.AppendLine($"usemtl {mats[material].name}");
                sb.AppendLine($"usemap {mats[material].name}");

                int[] triangles = m.GetTriangles(material);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[i] + 1,
                        triangles[i + 1] + 1, triangles[i + 2] + 1));
                }
            }

            return sb.ToString();
        }

        public static string MeshToString(Mesh m)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("g ").Append(m.name).Append("\n");
            foreach (Vector3 v in m.vertices)
            {
                sb.Append($"v {v.x} {v.y} {v.z}\n");
            }

            sb.Append("\n");
            foreach (Vector3 v in m.normals)
            {
                sb.Append($"vn {v.x} {v.y} {v.z}\n");
            }

            sb.Append("\n");
            foreach (Vector3 v in m.uv)
            {
                sb.Append($"vt {v.x} {v.y}\n");
            }

            return sb.ToString();
        }

        public static void MeshToFile(MeshFilter mf)
        {
            MeshToFile(mf, Guid.NewGuid().ToString());
        }

        public static void MeshToFile(Mesh m)
        {
            MeshToFile(m, Guid.NewGuid().ToString());
        }
        
        public static string MeshPath(string filename) => 
            FileSystem.BuildPath(UTERRAINS_CHUNK_PATH, $"{filename}.obj");

        public static void MeshToFile(MeshFilter mf, string filename)
        {
            Debug.Log("Started Save Operation");

            VerifyPath();

            string location = MeshPath(filename);
            using (StreamWriter sw = new StreamWriter(location))
            {
                string meshData = MeshToString(mf);
                if (meshData == null) return;
                sw.Write(meshData);
            }

            Debug.Log($"Successfully Saved file to {location}");
        }

        public static void MeshToFile(Mesh mesh, string filename)
        {
            Debug.Log("Started Save Operation");

            VerifyPath();

            string location = MeshPath(filename);
            using (StreamWriter sw = new StreamWriter(location))
            {
                string meshData = MeshToString(mesh);
                if (meshData == null) return;
                sw.Write(meshData);
            }

            Debug.Log($"Successfully Saved file to {location}");
        }

        private static void VerifyPath()
        {
            if (!Directory.Exists(UTERRAINS_CHUNK_PATH))
            {
                Directory.CreateDirectory(UTERRAINS_CHUNK_PATH);
            }
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