using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace Triangulo
{

    // We can now move around objects. However, how can we move our "camera", or modify our perspective?
    // In this tutorial, I'll show you how to setup a full projection/view/model (PVM) matrix.
    // In addition, we'll make the rectangle rotate over time.
    public class Window : GameWindow
    {

        private static readonly float x = 0.5f;
        private static readonly float y = 0.3f;
        private static readonly float z = 1.0f;


        private float[] _vertices =
        {

            //Cuadrado de la casa

            -x, -x,  x, 1.0f, 1.0f, 0.0f,
             x, -x,  x, 1.0f, 1.0f, 0.0f,
             x,  x,  x, 1.0f, 1.0f, 0.0f,
             x,  x,  x, 1.0f, 1.0f, 0.0f,
            -x,  x,  x, 1.0f, 1.0f, 0.0f,
            -x, -x,  x, 1.0f, 1.0f, 0.0f,

            -x,  x,  x, 1.0f, 0.5f, 0.0f,
            -x,  x, -x, 1.0f, 0.5f, 0.0f,
            -x, -x, -x, 1.0f, 0.5f, 0.0f,
            -x, -x, -x, 1.0f, 0.5f, 0.0f,
            -x, -x,  x, 1.0f, 0.5f, 0.0f,
            -x,  x,  x, 1.0f, 0.5f, 0.0f,


            //Triangulo de la casa

             -y,  z, x, 0.8f, 0.5f, 0.0f, // Punta
            -x,  x, x, 0.8f, 0.5f, 0.0f, // Izquierda
             x,  x, x, 0.8f, 0.5f, 0.0f, // Derecha

            -y,  z, x, 0.4f, 0.5f, 0.0f,
            -x,  x,  x, 0.5f, 0.4f, 0.0f,
            -x,  x, -x, 0.5f, 0.4f, 0.0f,


        };



        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;



        private Matrix4 _view;


        private Matrix4 _projection;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);


            GL.Enable(EnableCap.DepthTest);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);


            _shader = new Shader(@"D:\OpenGL projects\Casa\Casa\Shaders\shader.vert", @"D:\OpenGL projects\Casa\Casa\Shaders\shader.frag");
            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 3 * sizeof(float), 3 * sizeof(float));

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);

      
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f), Size.X / (float)Size.Y, 0.1f, 100.0f);

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);


   
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(_vertexArrayObject);


            _shader.Use();

            var model = Matrix4.Identity * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(30));


            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _view);
            _shader.SetMatrix4("projection", _projection);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}
