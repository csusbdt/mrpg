using System;
using System.Collections.Generic;
using System.Text;
using Client.Communication;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace CommunicationTest
{
    class ClientTest
    {
        // This is a helper method for the test code -- it turns 
        // CommunicationSystem.GetNextReceivedMessage() into a blocking call.
        static Message GetNextReceivedMessage()
        {
            Message message = null;
            while (message == null)
            {
                message = CommunicationSystem.GetNextReceivedMessage();
            }
            return message;
        }

        public static void Start()
        {
            // CommunicationSystem must be initilized before anything else.
            CommunicationSystem.Init();
            Debug.WriteLine("Client: Init completed.");

            // Make a TCP connection to the server.
            CommunicationSystem.Connect(IPAddress.Loopback, Program.ServerPort);
            Debug.WriteLine("Client: Connected");

            ProcessPreLoginState();
            ProcessAvatarSelectState();
            ProcessGamePlayState();
            ProcessGameStateExitAndLogout();

            Console.WriteLine("Client test done.");
        }

        static void ProcessPreLoginState()
        {
            // Send login message.
            CommunicationSystem.SendLoginMessage("x", "x");
            Debug.WriteLine("Client: Login sent.");
            CommunicationSystem.Update();

            // Read response.
            LoginSuccessMessage loginSuccessMessage = (LoginSuccessMessage)GetNextReceivedMessage();
            Debug.WriteLine("Client: Login success received.");
        }

        static void ProcessAvatarSelectState()
        {
            // Read avatar list message.
            AvatarListMessage avatarListMessage = (AvatarListMessage)GetNextReceivedMessage();
            Debug.WriteLine("Client: Avatar list received.");

            // Select first avatar in list.
            Avatar avatar = avatarListMessage.AvatarList[0];
            CommunicationSystem.SendAvatarSelectMessage(avatar.AvatarName);
            Debug.WriteLine("Client: Avatar select sent.");
            CommunicationSystem.Update();
        }

        static void ProcessGamePlayState()
        {
            SendMapSetupMessages();
            ProcessPlayerFireBallAttackOnNpc();

            // Test server sends a move entity message.
            MoveEntityMessage moveEntityMessage = (MoveEntityMessage)GetNextReceivedMessage();
            Debug.WriteLine("Client: Move entity received.");

            // The following interaction between client and server emulates the player
            // hitting the last created npc with a fire ball attack.

            // Invoke the first capability on the last created npc.
            string capability = "fire_ball";
            CommunicationSystem.SendInvokeCapabilityRequestMessage(capability, "3");
            Debug.WriteLine("Client: Invoke capability request sent.");
            CommunicationSystem.Update();

            // Test server accepts the invoke request by sending an invoke capability message.
            InvokeCapabilityMessage invokeCapabilityMessage = (InvokeCapabilityMessage)GetNextReceivedMessage();
            Debug.WriteLine("Client: Invoke capability received.");

            // Revoke a capability.
            CommunicationSystem.SendRevokeCapabilityRequestMessage("some_cap", "some_id");
            Debug.WriteLine("Client: Revoke capability request sent.");
            CommunicationSystem.Update();

            // Test server send a revoke capability message.
            RevokeCapabilityMessage revokeCapabilityMessage = (RevokeCapabilityMessage)GetNextReceivedMessage();
            Debug.WriteLine("Client: Revoke capability received.");
        }

        static void SendMapSetupMessages()
        {
            // Set map message is the first message server sends; this gives
            // client opportunity to clear graphics memory before loading new assets.
            SetMapMessage setMapMessage = (SetMapMessage)GetNextReceivedMessage();
            Debug.WriteLine("Client: Set map received.");

            // Test server sends 2 create entity messages followed by a create container message.
            CreateEntityMessage createEntityMessage = (CreateEntityMessage)GetNextReceivedMessage();
            createEntityMessage = (CreateEntityMessage)GetNextReceivedMessage();
            CreateContainerMessage createContainerMessage = (CreateContainerMessage)GetNextReceivedMessage();

            // Test server sends 2 create entity messages.
            createEntityMessage = (CreateEntityMessage)GetNextReceivedMessage();
            createEntityMessage = (CreateEntityMessage)GetNextReceivedMessage();

            // Test server sends create pc message next.
            CreatePcMessage createPcMessage = (CreatePcMessage)GetNextReceivedMessage();
            Debug.WriteLine("Client: Create PC received.");

            // Test server sends 2 create npc messages next.
            CreateNpcMessage createNpcMessage = (CreateNpcMessage)GetNextReceivedMessage();
            Debug.WriteLine("Client: Create NPC received.");
            createNpcMessage = (CreateNpcMessage)GetNextReceivedMessage();
            Debug.WriteLine("Client: Create NPC received.");
        }

        // Fire ball attack is an example of an instantanous effect.
        // An instantaneous effect can not be interrupted.
        static void ProcessPlayerFireBallAttackOnNpc()
        {
            // Player moves his pc.
            CommunicationSystem.SendMovePcMessage(0, 0, 0, 0, 0, 0);

            CommunicationSystem.SendInvokeCapabilityRequestMessage("fire_ball", "3");
            CommunicationSystem.Update();
            Debug.WriteLine("Client: Move PC sent.");
            Debug.WriteLine("Client: Invoke capability request sent.");

            // Test server accepts the invoke request by sending an invoke capability message.
            InvokeCapabilityMessage invokeCapabilityMessage = (InvokeCapabilityMessage)GetNextReceivedMessage();
            Debug.WriteLine("Client: Invoke capability received.");

            // Server adjusts the mana of the pc and health of the npc.
            SetManaMessage setManaMessage = (SetManaMessage)GetNextReceivedMessage();
            SetHealthMessage setHealthMessage = (SetHealthMessage)GetNextReceivedMessage();
        }

        // Player healing self is an example of a continuous effect.
        // A continuous effect can be interrupted.
        static void ProcessPlayerHealSelf()
        {
            string pcEntityId = "1";
            CommunicationSystem.SendInvokeCapabilityRequestMessage("heal", pcEntityId);
            CommunicationSystem.Update(); // flush message
            InvokeCapabilityMessage invokeCapabilityMessage = (InvokeCapabilityMessage)GetNextReceivedMessage();
            Debug.Assert(invokeCapabilityMessage.Capability.Equals("heal"));
            SetManaMessage setManaMessage = (SetManaMessage)GetNextReceivedMessage();
            SetHealthMessage setHealthMessage = (SetHealthMessage)GetNextReceivedMessage();
            setManaMessage = (SetManaMessage)GetNextReceivedMessage();
            setHealthMessage = (SetHealthMessage)GetNextReceivedMessage();
            RevokeCapabilityMessage revokeCapabilityMessage =
                (RevokeCapabilityMessage)GetNextReceivedMessage();
        }

        static void ProcessGameStateExitAndLogout()
        {
            // Exit game play.
            CommunicationSystem.SendExitGamePlayMessage();
            Debug.WriteLine("Client: Exit game play sent.");
            CommunicationSystem.Update();

            // Client transitions to avatar select state.

            // Logout and disconnect.
            CommunicationSystem.SendLogoutMessage();
            Debug.WriteLine("Client: Logout sent.");
            CommunicationSystem.Update();
            CommunicationSystem.Disconnect();
            Debug.WriteLine("Client: Disconnected.");
            CommunicationSystem.Update();
        }
    }
}
