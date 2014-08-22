using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;
using Tao.FreeType;
using System.Runtime.InteropServices;

namespace Client
{
    public class Font
    {
        private int listBase;
        private int fontSize;
        private int[] textures;
        private int[] extentX;

        public Font(string font, int size)
        {
            // Save the size we need it later on when printing
            fontSize = size;
            // We begin by creating a library pointer
            System.IntPtr libptr;
            int ret = FT.FT_Init_FreeType(out libptr);
            if (ret != 0) return;
            //Once we have the library we create and load the font face
            FT_FaceRec face;
            System.IntPtr faceptr;
            int retb = FT.FT_New_Face(libptr, font, 0, out faceptr);
            if (retb != 0) return;

            face = (FT_FaceRec)Marshal.PtrToStructure(faceptr, typeof(FT_FaceRec));

            //Freetype measures the font size in 1/64th of pixels for accuracy 
            //so we need to request characters in size*64
            FT.FT_Set_Char_Size(faceptr, size << 6, size << 6, 96, 96);

            //Provide a reasonably accurate estimate for expected pixel sizes
            //when we later on create the bitmaps for the font
            FT.FT_Set_Pixel_Sizes(faceptr, (uint)size, (uint)size);

            // Once we have the face loaded and sized we generate opengl textures 
            // from the glyphs  for each printable character
            textures = new int[128];
            extentX = new int[128];
            listBase = Gl.glGenLists(128);
            Gl.glGenTextures(128, textures);
            for (int c = 0; c < 128; c++)
            {
                CompileCharacter(face, faceptr, c);
            }

            // Dispose of these as we don't need
            FT.FT_Done_Face(faceptr);
            FT.FT_Done_FreeType(libptr);
        }

        public void CompileCharacter(FT_FaceRec face, System.IntPtr faceptr, int c)
        {
            //We first convert the number index to a character index
            uint index = FT.FT_Get_Char_Index(faceptr, (uint)c);

            //Here we load the actual glyph for the character
            //int ret = FT.FT_Load_Glyph(faceptr, index, FT.FT_LOAD_DEFAULT);
            int ret = FT.FT_Load_Glyph(faceptr, index, FT.FT_LOAD_FORCE_AUTOHINT);
            if (ret != 0)
            {
                Console.Write("Load_Glyph failed for character " + c.ToString());
            }

            FT_GlyphSlotRec glyphrec = (FT_GlyphSlotRec)Marshal.PtrToStructure(face.glyph, typeof(FT_GlyphSlotRec));

            ret = FT.FT_Render_Glyph(ref glyphrec, FT_Render_Mode.FT_RENDER_MODE_NORMAL);

            if (ret != 0)
            {
                Console.Write("Render failed for character " + c.ToString());
            }
            int size = (glyphrec.bitmap.width * glyphrec.bitmap.rows);

            if (size <= 0)
            {
                //space is a special `blank` character
                extentX[c] = 0;
                if (c == 32)
                {
                    Gl.glNewList((uint)(listBase + c), Gl.GL_COMPILE);
                    Gl.glTranslatef(fontSize >> 1, 0, 0);
                    extentX[c] = fontSize >> 1;
                    Gl.glEndList();
                }
                return;
            }

            byte[] bmp = new byte[size];
            Marshal.Copy(glyphrec.bitmap.buffer, bmp, 0, bmp.Length);

            //Next we expand the bitmap into an opengl texture 	    	
            int width = NextPow2(glyphrec.bitmap.width);
            int height = NextPow2(glyphrec.bitmap.rows);
            byte[] expanded = new byte[2 * width * height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    expanded[2 * (i + j * width)] = expanded[2 * (i + j * width) + 1] =
                        (i >= glyphrec.bitmap.width || j >= glyphrec.bitmap.rows) ?
                        (byte)0 : bmp[i + glyphrec.bitmap.width * j];
                }
            }

            //Set up some texture parameters for opengl
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures[c]);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);

            //Create the texture
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, width, height,
                0, Gl.GL_LUMINANCE_ALPHA, Gl.GL_UNSIGNED_BYTE, expanded);
            expanded = null;
            bmp = null;

            //Create a display list and bind a texture to it
            Gl.glNewList((uint)(listBase + c), Gl.GL_COMPILE);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, textures[c]);

            //Account for freetype spacing rules
            Gl.glTranslatef(glyphrec.bitmap_left, 0, 0);
            Gl.glPushMatrix();
            Gl.glTranslatef(0, glyphrec.bitmap_top - glyphrec.bitmap.rows, 0);
            float x = (float)glyphrec.bitmap.width / (float)width;
            float y = (float)glyphrec.bitmap.rows / (float)height;

            //Draw the quad
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2d(0, 0); Gl.glVertex2f(0, glyphrec.bitmap.rows);
            Gl.glTexCoord2d(0, y); Gl.glVertex2f(0, 0);
            Gl.glTexCoord2d(x, y); Gl.glVertex2f(glyphrec.bitmap.width, 0);
            Gl.glTexCoord2d(x, 0); Gl.glVertex2f(glyphrec.bitmap.width, glyphrec.bitmap.rows);
            Gl.glEnd();

            Gl.glPopMatrix();

            //Advance for the next character			
            Gl.glTranslatef(glyphrec.bitmap.width, 0, 0);
            extentX[c] = glyphrec.bitmap_left + glyphrec.bitmap.width;
            Gl.glEndList();

        }

        //public void Dispose()
        //{
        //    Gl.glDeleteLists(listBase, 128);
        //    Gl.glDeleteTextures(128, textures);
        //    textures = null;
        //    extentX = null;
        //}

        internal int NextPow2(int a)
        {
            int rval = 1;
            while (rval < a) rval <<= 1;
            return rval;
        }

        internal void PushScm()
        {
            //Gl.glPushAttrib(Gl.GL_TRANSFORM_BIT);
            //int[] viewport = new int[4];
            //Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
            //Gl.glMatrixMode(Gl.GL_PROJECTION);
            //Gl.glPushMatrix();
            //Gl.glLoadIdentity();
            //Gl.glOrtho(viewport[0], viewport[2], viewport[1], viewport[3], 0, 1);
            //Gl.glPopAttrib();
            //viewport = null;
        }

        internal void PopPm()
        {
            //Gl.glPushAttrib(Gl.GL_TRANSFORM_BIT);
            //Gl.glMatrixMode(Gl.GL_PROJECTION);
            //Gl.glPopMatrix();
            //Gl.glPopAttrib();
        }

        public void Print(float x, float y, string what)
        {
            int font = listBase;
            //Prepare openGL for rendering the font characters
            //PushScm();
            Gl.glPushAttrib(Gl.GL_LIST_BIT | Gl.GL_CURRENT_BIT | Gl.GL_ENABLE_BIT | Gl.GL_TRANSFORM_BIT);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glListBase(font);
            float[] modelview_matrix = new float[16];
            Gl.glGetFloatv(Gl.GL_MODELVIEW_MATRIX, modelview_matrix);
            Gl.glPushMatrix();

            Gl.glLoadIdentity();
            Gl.glTranslatef(x, y, 0);
            Gl.glMultMatrixf(modelview_matrix);

            //Render
            byte[] textbytes = new byte[what.Length];
            for (int i = 0; i < what.Length; i++)
                textbytes[i] = (byte)what[i];
            Gl.glCallLists(what.Length, Gl.GL_UNSIGNED_BYTE, textbytes);
            textbytes = null;


            //Restore openGL state

            Gl.glPopMatrix();
            Gl.glPopAttrib();
            //PopPm();

        }
    }
}
