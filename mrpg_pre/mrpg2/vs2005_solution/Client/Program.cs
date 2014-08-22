using System;
using System.Collections.Generic;
using System.Text;
using Tao.Sdl;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using Tao.OpenGl;
using System.Diagnostics;

namespace Client
{
    class Program
    {
        #region Fields

        static bool shuttingDown = false;
        static ClientState clientState = null;
        static int horizontalPixels;
        static int verticalPixels;
        static float aspectRatio = 0;
        static Avatar avatar;

        #endregion

        #region Properties

        public static float AspectRatio
        {
            get { return aspectRatio; }
        }

        public static bool ShuttingDown
        {
            get { return shuttingDown; }
            set { shuttingDown = value; }
        }

        public static ClientState ClientState
        {
            set { clientState = value; }
            get { return clientState; }
        }

        public static Avatar Avatar
        {
            set { avatar = value; }
            get { return avatar; }
        }

        #endregion

        #region Initialization

        private static void Init()
        {
            Configuration.Init();
            int sdlResult = Sdl.SDL_Init(Sdl.SDL_INIT_VIDEO);
            if (sdlResult == -1)
            {
                throw new Exception("SDL_Init failed.");
            }
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_DOUBLEBUFFER, 1);
            Sdl.SDL_WM_SetCaption(Configuration.Title, Configuration.Title);
            int sdlVideoModeFlags = Sdl.SDL_OPENGL | Sdl.SDL_GL_DOUBLEBUFFER;
            if (Configuration.FullScreen)
            {
                sdlVideoModeFlags |= Sdl.SDL_FULLSCREEN;
            }
            IntPtr screenPtr = Sdl.SDL_SetVideoMode(
                Configuration.HorizontalPixels, 
                Configuration.VerticalPixels, 
                0,
                sdlVideoModeFlags);
            if (screenPtr == IntPtr.Zero)
            {
                throw new Exception("SDL_SetVideoMode failed.");
            }
            IntPtr videoInfoPtr = Sdl.SDL_GetVideoInfo();
            Sdl.SDL_VideoInfo videoInfo = (Sdl.SDL_VideoInfo)
                Marshal.PtrToStructure(videoInfoPtr, typeof(Sdl.SDL_VideoInfo));
            horizontalPixels = videoInfo.current_w;
            verticalPixels = videoInfo.current_h;
            aspectRatio = (float)(videoInfo.current_w / (double)videoInfo.current_h);
            Gl.glViewport(0, 0, horizontalPixels, verticalPixels);
            Sdl.SDL_ShowCursor(Sdl.SDL_ENABLE);
            Sdl.SDL_EnableUNICODE(1);
            GraphicsSystem.Init();
            SoundSystem.Init();
            GuiSystem.Init();
            TextureSystem.Init();
            GuiTextureSystem.Init();
            ModelSystem.Init();
            clientState = new PreLoginState();
            clientState.Activate();
        }

        #endregion

        #region Events

        public delegate void MouseButtonUpHandler(ScreenCoordinate screenCoordinate);
        public static event MouseButtonUpHandler MouseButtonUpEvent;

        public delegate void KeyDownHandler(Sdl.SDL_keysym keysym);
        public static event KeyDownHandler KeyDownEvent;

        public delegate void KeyUpHandler(Sdl.SDL_keysym keysym);
        public static event KeyUpHandler KeyUpEvent;

        private static void ProcessEvents()
        {
            while (true)
            {
                Sdl.SDL_Event sdlEvent;
                int hasEvent = Sdl.SDL_PollEvent(out sdlEvent);
                if (hasEvent == Sdl.SDL_FALSE)
                {
                    return;
                }
                switch(sdlEvent.type)
                {
                    case Sdl.SDL_MOUSEBUTTONUP:
                        if (MouseButtonUpEvent != null)
                        {
                            short x = sdlEvent.button.x;
                            short y = sdlEvent.button.y;
                            ScreenCoordinate screenCoordinate = new ScreenCoordinate(x, verticalPixels - y);
                            MouseButtonUpEvent(screenCoordinate);
                        }
                        break;
                    case Sdl.SDL_KEYDOWN:
                        if (KeyDownEvent != null)
                        {
                            KeyDownEvent(sdlEvent.key.keysym);
                        }
                        break;
                    case Sdl.SDL_KEYUP:
                        if (KeyUpEvent != null)
                        {
                            KeyUpEvent(sdlEvent.key.keysym);
                        }
                        break;
                    case Sdl.SDL_QUIT:
                        shuttingDown = true;
                        break;
                }
            }
        }

        #endregion

        #region Main

        static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Log.Write();
            try
            {
                Init();
                Loop();
                Shutdown();
            }
            catch (Exception e)
            {
                Log.Write(e.Message + "\n" + e.StackTrace);
                Console.ReadLine();
            }
        }

        static void Shutdown()
        {
            CommunicationSystem.Disconnect();
            SoundSystem.Shutdown();
            Sdl.SDL_Quit();
        }

        private static void Loop()
        {
            Log.Write();
            DateTime previousDateTime = DateTime.Now;
            while (!shuttingDown)
            {
                DateTime currentDateTime = DateTime.Now;
                TimeSpan dt = currentDateTime - previousDateTime;
                previousDateTime = currentDateTime;
                ProcessEvents();
                if (dt != TimeSpan.Zero)
                {
                    SoundSystem.Update(dt);
                    clientState.Update(dt);
                    GuiSystem.Update(dt);
                    GraphicsSystem.Draw();
                }
                Thread.Sleep(0);
            }
        }

        #endregion
    }
}
