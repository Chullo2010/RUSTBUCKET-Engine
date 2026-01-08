using OpenTK.Graphics.OpenGL4;
using System.IO;

namespace Tiny3DEngine
{
    public static class ShaderLoader
    {
        public static int Load(string vertPath, string fragPath)
        {
            string vertSrc = File.ReadAllText(vertPath);
            string fragSrc = File.ReadAllText(fragPath);

            int vert = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vert, vertSrc);
            GL.CompileShader(vert);
            GL.GetShader(vert, ShaderParameter.CompileStatus, out int success);
            if (success == 0) throw new System.Exception(GL.GetShaderInfoLog(vert));

            int frag = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(frag, fragSrc);
            GL.CompileShader(frag);
            GL.GetShader(frag, ShaderParameter.CompileStatus, out success);
            if (success == 0) throw new System.Exception(GL.GetShaderInfoLog(frag));

            int program = GL.CreateProgram();
            GL.AttachShader(program, vert);
            GL.AttachShader(program, frag);
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out success);
            if (success == 0) throw new System.Exception(GL.GetProgramInfoLog(program));

            GL.DeleteShader(vert);
            GL.DeleteShader(frag);

            return program;
        }
    }
}
