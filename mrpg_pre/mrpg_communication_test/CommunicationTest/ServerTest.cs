using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Server.Communication;
using System.Threading;

namespace CommunicationTest
{
    class ServerTest
    {
        //static bool shutdown = false;
        static CommunicationChannel channel = null;

        // When a client connects to the server, the communication system will invoke
        // registered callback methods that handle new communication channel events.
        public static void CreateCommunicationChannelEventHandler(CommunicationChannel communicationChannel)
        {
            ServerTest.channel = communicationChannel;
        }

        // This is a helper method for the test code -- it turns 
        // channel.GetNextReceivedMessage() into a blocking call.
        static Message GetNextReceivedMessage()
        {
            Message message = null;
            while (message == null)
            {
                message = channel.GetNextReceivedMessage();
            }
            return message;
        }

        // This test involves a single connecting client.
        public static void Start()
        {
            // Must call Init on Communication System before anything else.
            CommunicationSystem.Init(
                Program.ServerPort, 
                Program.MaximumNumberOfCommunicationChannels, 
                CreateCommunicationChannelEventHandler);
            Debug.WriteLine("Server: Init completed.");

            // Loop until test client connects to server.
            while (channel == null)
            {
                // Note: should call update in game loop to process incoming connection requests.
                CommunicationSystem.Update();
            }
            Debug.WriteLine("Server: TCP connection accepted.");

            ProcessPreLoginState();
            ProcessAvatarSelectState();
            ProcessGamePlayState();
            ProcessGameStateExitAndLogout();

            // Give client time to disconnect.
            Thread.Sleep(1000);

            // Terminate server instance.
            CommunicationSystem.Shutdown();
            Debug.WriteLine("Server done.");
            CommunicationSystem.Update();
            Debug.WriteLine("All tests passed; press enter to terminate.");
            Console.ReadKey();
        }

        static void ProcessPreLoginState()
        {
            // The first message will be a login message.
            LoginMessage loginMessage = (LoginMessage)GetNextReceivedMessage();
            Debug.WriteLine("Server: Login received.");

            // Send success message.
            channel.SendLoginSuccessMessage();
            Debug.WriteLine("Server: Login success sent.");
        }

        static void ProcessAvatarSelectState()
        {
            // Send avatar list.
            List<Avatar> avatars = new List<Avatar>();
            Avatar avatar = new Avatar("rovon", "priest", 7);
            avatars.Add(avatar);
            channel.SendAvatarListMessage(avatars);
            Debug.WriteLine("Server: Avatar list sent.");
            CommunicationSystem.Update();

            // Get avatar select message.
            AvatarSelectMessage avatarSelectMessage = (AvatarSelectMessage)GetNextReceivedMessage();
            Debug.WriteLine("Server: Avatar select received.");
        }

        static void ProcessGamePlayState()
        {
            SendMapSetupMessages();
            ProcessPlayerFireBallAttackOnNpc();

            // Send move entity message.
            channel.SendMoveEntityMessage("2", 0, 0, 0, 0, 0, 0);
            Debug.WriteLine("Server: Move entity sent.");
            CommunicationSystem.Update();

            // Test client sends invoke capability request message.
            InvokeCapabilityRequestMessage invokeCapabilityRequestMessage =
                (InvokeCapabilityRequestMessage)GetNextReceivedMessage();
            Debug.WriteLine("Server: Invoke capability received.");

            // Accept the invoke capability request message.
            channel.SendInvokeCapabilityMessage(
                "1",
                invokeCapabilityRequestMessage.Capability,
                invokeCapabilityRequestMessage.TargetId);
            Debug.WriteLine("Server: invoke capability sent.");
            CommunicationSystem.Update();

            // Test client sends a revoke capability request message.
            RevokeCapabilityRequestMessage revokeCapabilityRequestMessage =
                (RevokeCapabilityRequestMessage)GetNextReceivedMessage();
            Debug.WriteLine("Server: Revoke capability received.");

            // Interrupt an invoked capability.
            channel.SendRevokeCapabilityMessage("1", "weakness", "2");
            Debug.WriteLine("Server: revoke capability sent.");
            CommunicationSystem.Update();
        }

        static void SendMapSetupMessages()
        {
            // Send set map message first.
            // This allows client to clear graphics memory before loading new assets.
            channel.SendSetMapMessage("home");
            Debug.WriteLine("Server: Set map sent.");
            CommunicationSystem.Update();

            // Create a container.
            // Create entities to serve as contained items.
            channel.SendCreateEntityMessage("201", "sword");
            Debug.WriteLine("Server: create entity (sword) sent.");
            channel.SendCreateEntityMessage("202", "wine");
            Debug.WriteLine("Server: create entity (wine) sent.");
            // Construct item list.
            List<string> items = new List<string>();
            items.Add("201");
            items.Add("202");
            channel.SendCreateContainerMessage("200", "treasure_chest", items);

            // Create pc.
            // Create entities to serve as inventory.
            channel.SendCreateEntityMessage("100", "dagger");
            Debug.WriteLine("Server: create entity (dagger) sent.");
            channel.SendCreateEntityMessage("101", "hat");
            Debug.WriteLine("Server: create entity (hat) sent.");
            // Construct inventory list.
            List<string> inventory = new List<string>();
            inventory.Add("100");
            inventory.Add("101");
            // Construct capability list.
            List<string> capabilities = new List<string>();
            capabilities.Add("fire_ball");
            capabilities.Add("heal");
            // Create pc.
            channel.SendCreatePcMessage("1", "priest", 0, 0, 0, 0, 0, 0, capabilities, inventory);
            Debug.WriteLine("Server: Create pc sent.");

            // Create entities that are in the map that the player is in.
            channel.SendCreateNpcMessage("2", "orc", 0, 0, 0, 0, 0, 0);
            Debug.WriteLine("Server: Create npc sent.");
            channel.SendCreateNpcMessage("3", "orc", 10, 0, 0, 0, 0, 0);
            Debug.WriteLine("Server: Create npc sent.");
            CommunicationSystem.Update();
        }

        static void ProcessPlayerFireBallAttackOnNpc()
        {
            // Test client sends move pc message.
            MovePcMessage movePcMessage = (MovePcMessage)GetNextReceivedMessage();
            Debug.WriteLine("Server: move pc received.");

            // Test client sends invoke fire_ball capability request message.
            InvokeCapabilityRequestMessage invokeCapabilityRequestMessage =
                (InvokeCapabilityRequestMessage)GetNextReceivedMessage();
            Debug.WriteLine("Server: Invoke capability received.");

            // Accept the request to invoke fire_ball capability.
            channel.SendInvokeCapabilityMessage("1", "fire_ball", "3");

            // Adjust pc mana.
            channel.SendSetManaMessage("1", 31);

            // Adjust npc health.
            channel.SendSetHealthMessage("3", 12);
            CommunicationSystem.Update();
            Debug.WriteLine("Server: invoke capability sent.");
            Debug.WriteLine("Server: set mana sent.");
            Debug.WriteLine("Server: set health sent.");
        }

        static void ProcessPlayerHealSelf()
        {
            string pcEntityId = "1";
            InvokeCapabilityRequestMessage invokeCapabilityRequestMessage =
                (InvokeCapabilityRequestMessage)GetNextReceivedMessage();
            Debug.Assert(invokeCapabilityRequestMessage.Capability.Equals("heal"));
            channel.SendInvokeCapabilityMessage(pcEntityId, "heal", pcEntityId);
            channel.SendSetManaMessage(pcEntityId, 22.2f);
            channel.SendSetHealthMessage(pcEntityId, 12.3f);
            CommunicationSystem.Update(); // flush messages
            // Send another adjustment to mana and health.
            channel.SendSetManaMessage(pcEntityId, 22.2f);
            channel.SendSetHealthMessage(pcEntityId, 12.3f);
            CommunicationSystem.Update(); // flush messages
            // Heal is interrupted or complete.
            channel.SendRevokeCapabilityMessage(pcEntityId, "heal", pcEntityId);
        }

        static void ProcessGameStateExitAndLogout()
        {
            // Test client sends exit game play message.
            ExitGamePlayMessage exitGamePlayMessage = (ExitGamePlayMessage)GetNextReceivedMessage();
            Debug.WriteLine("Server: exit game play received.");

            // Test client sends logout message.
            LogoutMessage logoutMessage = (LogoutMessage)GetNextReceivedMessage();
            Debug.WriteLine("Server: logout received.");
        }
    }
}
