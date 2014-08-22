using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class AvatarSelectionState : ClientState
    {
        #region Fields

        List<Avatar> avatarList;
        List<Button> avatarButtons = new List<Button>();

        delegate void UpdateMethod(TimeSpan dt);
        UpdateMethod updateMethod;
        List<IDisposable> disposables = new List<IDisposable>();

        // gui components
        Button logoutButton;
        Button selectAvatarButton;

        #endregion

        #region Life Cycle

        public void Activate()
        {
            Log.Write(this);
            Program.Avatar = null;
            updateMethod = WaitingForAvatarList;
            //            Camera.Position = new Vec3f(0, 0, 10);

            // logout button
            //PixelContainer logoutButtonPixelContainer = TargaImageLoader.LoadImage("gui/loginButtonTexture.tga");
            //Texture logoutButtonTexture = new Texture(logoutButtonPixelContainer);
            //disposables.Add(logoutButtonTexture);
            Texture logoutButtonTexture = GuiTextureSystem.GetTextureById("logoutButton");
            ScreenCoordinate logoutButtonLowerLeft = new ScreenCoordinate(10, 40);
            ScreenCoordinate logoutButtonUpperRight = new ScreenCoordinate(130, 80);
            logoutButton = new Button(
                logoutButtonLowerLeft,
                logoutButtonUpperRight,
                logoutButtonTexture,
                LogoutButtonHandler);
            GuiSystem.AddButton(logoutButton);
            logoutButton.Enabled = true;

            // avatar selection button
            //PixelContainer selectAvatarButtonPixelContainer = TargaImageLoader.LoadImage("gui/passwordTextTexture.tga");
            //Texture selectAvatarButtonTexture = new Texture(selectAvatarButtonPixelContainer);
            //disposables.Add(selectAvatarButtonTexture);
            Texture selectAvatarButtonTexture = GuiTextureSystem.GetTextureById("okButton");
            ScreenCoordinate selectAvatarButtonLowerLeft = new ScreenCoordinate(300, 240);
            ScreenCoordinate selectAvatarButtonUpperRight = new ScreenCoordinate(420, 280);
            selectAvatarButton = new Button(
                selectAvatarButtonLowerLeft,
                selectAvatarButtonUpperRight,
                selectAvatarButtonTexture, 
                SelectAvatarButtonHandler);
            GuiSystem.AddButton(selectAvatarButton);
            selectAvatarButton.Enabled = false;
        }

        public void Update(TimeSpan dt)
        {
            updateMethod(dt);
        }

        void TransitionToNextState(ClientState nextState)
        {
            Log.Write(this);
            GuiSystem.Clear();
            foreach (IDisposable disposable in disposables)
            {
                disposable.Dispose();
            }
            GC.Collect();
            nextState.Activate();
            Program.ClientState = nextState;
        }

        #endregion

        #region Gui Event Handlers

        void LogoutButtonHandler()
        {
            Log.Write(this);
            CommunicationSystem.SendLogoutMessage();
            CommunicationSystem.Disconnect();
            TransitionToNextState(new PreLoginState());
        }

        void AvatarButtonHandler()
        {
            Log.Write(this);
            //CommunicationSystem.SendLogoutMessage();
            //CommunicationSystem.Disconnect();
            //TransitionToNextState(new PreLoginState());
        }
        void SelectAvatarButtonHandler()
        {
            Log.Write(this);
            //Avatar avatar = avatarList[0];
            //Program.Avatar = avatar;
            if (Program.Avatar != null)
            {
                CommunicationSystem.SendAvatarSelectMessage(Program.Avatar);
                TransitionToNextState(new GamePlayState());
            }
        }

        #endregion

        #region Update Methods

        private void WaitingForAvatarList(TimeSpan dt)
        {
            Message message = CommunicationSystem.GetNextMessage();
            if (message == null)
            {
                return;
            }
            // Drop any spurious game play messages.
            if (message.IsGamePlayMessage)
            {
                return;
            }
            Log.Write(this, message.GetType().ToString());
            AvatarListMessage avatarListMessage = (AvatarListMessage)message;
            if (avatarListMessage == null)
            {
                throw new Exception("Invalid server response.");
            }
            avatarList = avatarListMessage.AvatarList;
            avatarButtons.Clear();
            int y = 0;
            foreach (Avatar avatar in avatarList)
            {
                
                y += 50;
                String textureID = "logoutButton";
                switch (avatar.AvatarClass)
                {
                    case "mage":                        
                        textureID = "mageIcon";
                        break;

                        
                }

                Texture avatarButtonTexture = GuiTextureSystem.GetTextureById(textureID);
                ScreenCoordinate avatarButtonLowerLeft = new ScreenCoordinate(110, 40+y);
                ScreenCoordinate avatarButtonUpperRight = new ScreenCoordinate(160, 90+y);

                AvatarSelectButton avatarButton = new AvatarSelectButton(
                avatarButtonLowerLeft,
                avatarButtonUpperRight,
                avatarButtonTexture,
                AvatarButtonHandler, avatar);

                ScreenCoordinate avatarIdLowerLeft = new ScreenCoordinate(avatarButtonLowerLeft.x + 70, avatarButtonLowerLeft.y);
                ScreenCoordinate avatarIdUpperRight = new ScreenCoordinate(avatarButtonUpperRight.x + 70 + 100, avatarButtonUpperRight.y);
                Text avatarIdText = new Text(avatarIdLowerLeft, avatarIdUpperRight, null, avatar.AvatarId);
                GuiSystem.AddText(avatarIdText);
                GuiSystem.AddButton(avatarButton);
                
            }

            selectAvatarButton.Enabled = true;
            updateMethod = WaitingForUserInput;
        }

        private void WaitingForUserInput(TimeSpan dt)
        {
        }

        #endregion
    }
}
