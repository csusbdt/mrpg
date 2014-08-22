using System;
using System.Collections.Generic;
using System.Text;
using Tao.Sdl;

namespace Client
{
    class GamePlayState : ClientState
    {
        #region Fields

        Button exitButton; // = new Button(new ScreenCoordinate(0, 0), new ScreenCoordinate(60, 60));
        //Dictionary<string, Entity> entityDictionary = new Dictionary<string, Entity>();
        List<IDisposable> disposables = new List<IDisposable>();

        bool isKeyDownW = false;
        bool isKeyDownA = false;
        bool isKeyDownS = false;
        bool isKeyDownD = false;

        Avatar avatar;

        #endregion

        #region Life Cycle

        public GamePlayState()
        {
            avatar = Program.Avatar;
        }

        public void Activate()
        {
            Log.Write(this);
            Program.KeyDownEvent += KeyDownEventHandler;
            Program.KeyUpEvent += KeyUpEventHandler;
            Camera.Position = Program.Avatar.Position;

            // Exit button
            ScreenCoordinate exitButtonLowerLeft = new ScreenCoordinate(10, 40);
            ScreenCoordinate exitButtonUpperRight = new ScreenCoordinate(130, 80);
            //PixelContainer exitButtonImage = TargaImageLoader.LoadImage("models/test.tga");
            //Texture exitButtonTexture = new Texture(exitButtonImage);
            //disposables.Add(exitButtonTexture);
            Texture exitButtonTexture = GuiTextureSystem.GetTextureById("exitButton");
            exitButton = new Button(
                exitButtonLowerLeft, 
                exitButtonUpperRight, 
                exitButtonTexture, 
                ExitButtonHandler);
            GuiSystem.AddButton(exitButton);
        }

        void TransitionToAvatarSelectionState()
        {
            Log.Write(this);
            Program.KeyDownEvent -= KeyDownEventHandler;
            Program.KeyUpEvent -= KeyUpEventHandler;
            GraphicsSystem.Entities.Clear();
            GuiSystem.Clear();
            Map.Unload();
            foreach (IDisposable disposable in disposables)
            {
                disposable.Dispose();
            }
            GC.Collect();
            ClientState avatarSelectionState = new AvatarSelectionState();
            avatarSelectionState.Activate();
            Program.ClientState = avatarSelectionState;
        }

        #endregion

        #region Gui Event Handlers

        void ExitButtonHandler()
        {
            Log.Write(this);
            CommunicationSystem.SendExitGamePlayMessage();
            TransitionToAvatarSelectionState();
        }

        public void KeyDownEventHandler(Sdl.SDL_keysym keysym)
        {
            switch (keysym.sym)
            {
                case Sdl.SDLK_w :
                    isKeyDownW = true;
                    break;
                case Sdl.SDLK_a :
                    isKeyDownA = true;
                    break;
                case Sdl.SDLK_s :
                    isKeyDownS = true;
                    break;
                case Sdl.SDLK_d :
                    isKeyDownD = true;
                    break;
            }
        }

        public void KeyUpEventHandler(Sdl.SDL_keysym keysym)
        {
            switch (keysym.sym)
            {
                case Sdl.SDLK_w:
                    isKeyDownW = false;
                    break;
                case Sdl.SDLK_a:
                    isKeyDownA = false;
                    break;
                case Sdl.SDLK_s:
                    isKeyDownS = false;
                    break;
                case Sdl.SDLK_d:
                    isKeyDownD = false;
                    break;
            }
        }

        #endregion

        #region Update Methods

        public void Update(TimeSpan dt)
        {
            ProcessReceivedMessages();
            ProcessUserInput(dt);
        }

        void ProcessReceivedMessages()
        {
            while (true)
            {
                Message message = CommunicationSystem.GetNextMessage();
                if (message == null)
                {
                    return;
                }

                if (message is SetMapMessage)
                {
                    SetMapMessage setMapMessage = (SetMapMessage)message;
                    LoadMap(setMapMessage.MapId);
                }
                else if (message is MoveAvatarMessage)
                {
                    MoveAvatarMessage moveAvatarMessage = (MoveAvatarMessage)message;
                    avatar.MoveTo(moveAvatarMessage.Position, moveAvatarMessage.Orientation);
                }
                else if (message is CreateModeledEntityMessage)
                {
                    CreateModeledEntityMessage createEntityMessage = (CreateModeledEntityMessage)message;
                    EntitySystem.CreateEntity(createEntityMessage);
                }
                else if (message is DeleteEntityMessage)
                {
                    DeleteEntityMessage deleteEntityMessage = (DeleteEntityMessage)message;
                    Entity entity = deleteEntityMessage.Entity;
                    GraphicsSystem.Entities.Remove(entity);
                    entityDictionary.Remove(entity.EntityId);
                }
                else if (message is MoveEntityMessage)
                {
                    MoveEntityMessage moveEntityMessage = (MoveEntityMessage)message;
                    String entityId = moveEntityMessage.EntityId;
                    Entity entity = entityDictionary[entityId];
                    entity.Position = moveEntityMessage.Position;
                    entity.Orientation = moveEntityMessage.Orientation;
                }
                else
                {
                    throw new Exception("Invalid message from server.");
                }
            }
        }

        void ProcessUserInput(TimeSpan dt)
        {
            Vec3f position = avatar.Position;
            if (isKeyDownW)
            {
                position.z -= avatar.Speed * (float)dt.TotalSeconds;
            }
            if (isKeyDownA)
            {
                position.x -= avatar.Speed * (float)dt.TotalSeconds;
            }
            if (isKeyDownD)
            {
                position.x += avatar.Speed * (float)dt.TotalSeconds;
            }
            if (isKeyDownS)
            {
                position.z += avatar.Speed * (float)dt.TotalSeconds;
            }
            avatar.Position = position;
            Camera.Position = position;
        }

        void LoadMap(string mapId)
        {
            Log.Write();
            GraphicsSystem.Entities.Clear();
            entityDictionary.Clear();
            foreach (IDisposable disposable in disposables)
            {
                disposable.Dispose();
            }
            disposables.Clear();
            Map.Load(mapId);
        }

        void UnloadMap()
        {
            Log.Write();
            GraphicsSystem.Entities.Clear();
            disposables.Clear();
            entityDictionary.Clear();
            Map.Unload();
        }

        #endregion
    }
}
