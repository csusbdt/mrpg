using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class GamePlayState : ClientState
    {
        Pc pc;

        public GamePlayState(Client client) : base(client)
        {
        }

        public override void Activate()
        {
            Log.Write();
            pc = Pc.CreatePc(client);
        }

        void ExitGamePlayState()
        {
            Pc.DestroyPc(pc);
            AvatarSelectionState avatarSelectionState = new AvatarSelectionState(client);
            client.ClientState = avatarSelectionState;
            avatarSelectionState.Activate();
        }

        public override void Update(TimeSpan dt)
        {
            Message message = client.GetNextMessage();
            if (message == null)
            {
                return;
            }
            Log.Write(this, message.GetType().ToString());
            if (message is ExitGamePlayMessage)
            {
                ExitGamePlayState();
            }
            else
            {
                throw new Exception("Invalid message from client.");
            }
        }
    }
}
