using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class AvatarSelectionState : ClientState
    {
        Dictionary<string, Avatar> avatarDictionary = new Dictionary<string, Avatar>();

        public AvatarSelectionState(Client client) : base(client)
        {
        }

        public override void Activate()
        {
            Log.Write();
            List<Avatar> avatarList = AvatarDao.FindListByUsername(client.Username);
            foreach (Avatar avatar in avatarList)
            {
                avatar.Position = new Vec3f(0, 0, 10);
                avatarDictionary.Add(avatar.AvatarId, avatar);
            }
            client.SendAvatarListMessage(avatarList);
        }

        public override void Update(TimeSpan dt)
        {
            Message message = client.GetNextMessage();
            if (message == null)
            {
                return;
            }
            Log.Write(this, message.GetType().ToString());
            if (message is LogoutMessage)
            {
                client.Dead = true;
                return;
            }
            else if (message is AvatarSelectMessage)
            {
                AvatarSelectMessage avatarSelectMessage = (AvatarSelectMessage) message;
                string avatarId = avatarSelectMessage.AvatarId;
                client.Avatar = avatarDictionary[avatarId];
                GamePlayState gamePlayState = new GamePlayState(client);
                client.ClientState = new GamePlayState(client);
                gamePlayState.Activate();
            } 
            else
            {
                //throw new Exception("Invalid message from client.");
                Log.Write("Spurious game play message received.");
            }
        }
    }
}
