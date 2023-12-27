using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RewardCoinGage
{

    public class Triangle
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        public Triangle(int resolution, float radius, float angle, float startAngle)
        {
            resolution = Mathf.Max(3, resolution);

            // Add the first vertex, center of the "circle"
            vertices.Add(Vector3.zero);

            // I now need to calculate the vertices positions
            float angleStep = angle / (resolution - 2);

            // Let's now setup the vertices
            for (int i = 0; i < resolution - 1; i++)
            {
                float thisAngle = startAngle + angleStep * i;
                float x = radius * Mathf.Cos(Mathf.Deg2Rad * thisAngle);
                float y = radius * Mathf.Sin(Mathf.Deg2Rad * thisAngle);

                Vector3 vertex = new Vector3(x, y);
                vertices.Add(vertex);
            }

            // Let's now add the triangles. 
            // We are always going to add the first vertex
            // How many triangles should we have ? resolution - 2 ?
            // Yeah I think that's it

            for (int i = 0; i < resolution - 2; i++)
            {
                triangles.Add(0);
                triangles.Add(i + 2);
                triangles.Add(i + 1);
            }
        }

        public Vector3[] GetVertices()
        {
            return vertices.ToArray();
        }

        public int[] GetTriangles()
        {
            return triangles.ToArray();
        }

        public Vector3 GetCenter()
        {
            Vector3 v = vertices[0];

            for (int i = 0; i < vertices.Count - 1; i++)
                v += vertices[i];

            v /= vertices.Count;
            return v;

            //return (vertices[0] + vertices[1] + vertices[vertices.Count - 1]) / 3;
        }

        public Vector3 GetCenterDirection()
        {
            Vector3 v = vertices[0];

            for (int i = 1; i < vertices.Count; i++)
                v += vertices[i];

            v /= vertices.Count;
            return v.normalized;
        }
    }

}