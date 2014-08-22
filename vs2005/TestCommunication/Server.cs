using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using ServerCommunication;

namespace TestCommunication
{
    class Server
    {
        //static bool shutdown = false;
        static CommunicationChannel channel = null;

        // When a client connects to the server, the communication system will invoke
        // registered callback methods that handle new communication channel events.
        public static void CreateCommunicationChannelEventHandler(CommunicationChannel communicationChannel)
        {
            Server.channel = communicationChannel;
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
            try
            {
                Start2();
            }
            catch (Exception e)
            {
                Console.WriteLine("Test failed.");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ReadKey();
            }
        }

        static void Start2()
        {
            // Must call Init on Communication System before anything else.
            CommunicationSystem.Init(
                Program.ServerPort, 
                Program.MaximumNumberOfCommunicationChannels, 
                CreateCommunicationChannelEventHandler);

            // Loop until test client connects to server.
            while (channel == null)
            {
                // Note: should call update in game loop to process incoming connection requests.
                CommunicationSystem.Update();
            }

            TestClientMessages();
            TestServerMessages();

            // Give client time to disconnect.
            Thread.Sleep(1000);

            // Terminate server instance.
            CommunicationSystem.Shutdown();
            CommunicationSystem.Update();
            Debug.WriteLine("All tests passed; press enter to terminate.");
            Console.ReadKey();
        }

        static void TestClientMessages()
        {
            Message message;
            message = (LoginMessage)GetNextReceivedMessage();
            message = (LogoutMessage)GetNextReceivedMessage();
            message = (SelectAvatarMessage)GetNextReceivedMessage();
            message = (ExitGameMessage)GetNextReceivedMessage();
            message = (MoveRequestMessage)GetNextReceivedMessage();
            message = (ActionRequestMessage)GetNextReceivedMessage();
            message = (StopActionRequestMessage)GetNextReceivedMessage();
            message = (InteractRequestMessage)GetNextReceivedMessage();
            message = (AcquireItemRequestMessage)GetNextReceivedMessage();
        }

        static void TestServerMessages()
        {
            channel.SendLoginSuccessMessage();
            channel.SendLoginFailureMessage();
            List<Avatar> avatars = new List<Avatar>();
            Avatar avatar = new Avatar("", "", 0);
            avatars.Add(avatar);
            channel.SendAvatarListMessage(avatars);
            channel.SendSetMapMessage("");
            channel.SendCreateModeledEntityMessage("", "", 0, 0, 0, 0, 0, 0);
            channel.SendSetPcMessage("");
            channel.SendAddCapabilityMessage("");
            channel.SendRemoveCapabilityMessage("");
            channel.SendAddItemToInventoryMessage("", "");
            channel.SendRemoveItemFromInventoryMessage("");
            channel.SendCreateItemListMessage("");
            channel.SendAddItemToListMessage("", "");
            channel.SendRemoveItemFromMessage("");
            channel.SendCreateActionMessage("", "", "", "");
            channel.SendStopActionMessage("");
            channel.SendSetManaMessage("", 0);
            channel.SendSetHealthMessage("", 0);
            channel.SendMoveEntityMessage("", 0, 0, 0, 0, 0, 0);
            channel.SendDieMessage("");
            channel.SendDeleteEntityMessage("");
            CommunicationSystem.Update();
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
    }
}
