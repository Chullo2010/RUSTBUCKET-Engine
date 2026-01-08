using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace Tiny3DEngine
{
    public static class SphereBuilder
    {
        public static void CreateSphere(out int vao, out int indexCount, float radius = 1f, int slices = 32, int stacks = 16)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<uint> indices = new List<uint>();

            for (int stack = 0; stack <= stacks; stack++)
            {
                float phi = MathF.PI / 2 - stack * MathF.PI / stacks;
                float y = radius * MathF.Sin(phi);
                float r = radius * MathF.Cos(phi);

                for (int slice = 0; slice <= slices; slice++)
                {
                    float theta = slice * 2 * MathF.PI / slices;
                    float x = r * MathF.Cos(theta);
                    float z = r * MathF.Sin(theta);
                    vertices.Add(new Vector3(x, y, z));
                    normals.Add(Vector3.Normalize(new Vector3(x, y, z)));
                }
            }

            for (int stack = 0; stack < stacks; stack++)
            {
                for (int slice = 0; slice < slices; slice++)
                {
                    uint first = (uint)(stack * (slices + 1) + slice);
                    uint second = first + (uint)(slices + 1);

                    indices.Add(first);
                    indices.Add(second);
                    indices.Add(first + 1);

                    indices.Add(second);
                    indices.Add(second + 1);
                    indices.Add(first + 1);
                }
            }

            vao = GL.GenVertexArray();
            int vbo = GL.GenBuffer();
            int nbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, nbo);
            GL.BufferData(BufferTarget.ArrayBuffer, normals.Count * Vector3.SizeInBytes, normals.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
            indexCount = indices.Count;
        }
    }
}
