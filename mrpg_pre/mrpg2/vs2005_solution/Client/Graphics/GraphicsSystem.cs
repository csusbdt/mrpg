using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;
using Tao.Sdl;

namespace Client
{
    class GraphicsSystem
    {
        private const float Z_NEAR = 0.1f;
        private const float Z_FAR = 2000;
        private static List<Entity> entities = new List<Entity>();
        private static List<GuiComponent> guiComponents = new List<GuiComponent>();

        private static float[] perspectiveProjectionMatrix = new float[16];
        private static float[] orthographicProjectionMatrix = new float[16];

        public static List<Entity> Entities
        {
            get { return entities; }
        }

        public static List<GuiComponent> GuiComponents
        {
            get { return guiComponents; }
        }

        public static void Init()
        {
            // Generate orthographic projection matrix.
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            int[] viewport = new int[4];
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
            Glu.gluOrtho2D(viewport[0], viewport[2], viewport[1], viewport[3]);
            Gl.glGetFloatv(Gl.GL_PROJECTION_MATRIX, orthographicProjectionMatrix);

            // Generate perspective projection matrix.
            //Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_FASTEST);
            //Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, Program.AspectRatio, Z_NEAR, Z_FAR);
            Gl.glGetFloatv(Gl.GL_PROJECTION_MATRIX, perspectiveProjectionMatrix);

            Gl.glClearColor(0, 0, 0, 1);
        }

        public static void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            DrawMap();
            DrawEntities();
            DrawGuiComponents();
            Sdl.SDL_GL_SwapBuffers();
        }

        private static void DrawMap()
        {
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadMatrixf(perspectiveProjectionMatrix);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glColor4f(1, 1, 1, 1);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Camera.Apply();
            Map.Draw();
        }

        private static void DrawEntities()
        {
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadMatrixf(perspectiveProjectionMatrix);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glColor4f(1, 1, 1, 1);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Camera.Apply();
            foreach (Entity entity in entities)
            {
                Gl.glPushMatrix();
                entity.Draw();
                Gl.glPopMatrix();
            }
        }

        //            Gl.glEnable(Gl.GL_BLEND);
        //            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
        private static void DrawGuiComponents()
        {
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadMatrixf(orthographicProjectionMatrix);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glColor4f(1, 1, 1, 1);
            foreach (GuiComponent guiComponent in guiComponents)
            {
                Gl.glPushMatrix();
                guiComponent.draw();
                Gl.glPopMatrix();
            }
        }
    }
}
