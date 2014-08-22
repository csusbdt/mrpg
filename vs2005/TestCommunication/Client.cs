using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Diagnostics;
using ClientCommunication;

namespace TestCommunication
{
    class Client
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

        public static void Start2()
        {
            // Make a TCP connection to the server.
            CommunicationSystem.Connect(IPAddress.Loopback, Program.ServerPort);
            TestClientMessages();
            TestServerMessages();
            CommunicationSystem.Disconnect();
            Console.WriteLine("Client test done.");
        }

        static void TestClientMessages()
        {
            CommunicationSystem.SendLoginMessage("x", "x");
            CommunicationSystem.SendLogoutMessage();
            CommunicationSystem.SendSelectAvatarMessage("");
            CommunicationSystem.SendExitGameMessage();
            CommunicationSystem.SendMoveRequestMessage(0, 0, 0, 0, 0, 0);
            CommunicationSystem.SendActionRequestMessage("", "");
            CommunicationSystem.SendStopActionRequestMessage("");
            CommunicationSystem.SendInteractRequestMessage("");
            CommunicationSystem.SendAcquireItemRequestMessage("");
            CommunicationSystem.Update();
        }

        static void TestServerMessages()
        {
            Message message;
            message = (LoginSuccessMessage)GetNextReceivedMessage();
            message = (LoginFailureMessage)GetNextReceivedMessage();
            message = (AvatarListMessage)GetNextReceivedMessage();
            message = (SetMapMessage)GetNextReceivedMessage();
            message = (CreateModeledEntityMessage)GetNextReceivedMessage();
            message = (SetPcMessage)GetNextReceivedMessage();
            message = (AddCapabilityMessage)GetNextReceivedMessage();
            message = (RemoveCapabilityMessage)GetNextReceivedMessage();
            message = (AddItemToInventoryMessage)GetNextReceivedMessage();
            message = (RemoveItemFromInventoryMessage)GetNextReceivedMessage();
            message = (CreateItemListMessage)GetNextReceivedMessage();
            message = (AddItemToListMessage)GetNextReceivedMessage();
            message = (RemoveItemFromListMessage)GetNextReceivedMessage();
            message = (CreateActionMessage)GetNextReceivedMessage();
            message = (StopActionMessage)GetNextReceivedMessage();
            message = (SetManaMessage)GetNextReceivedMessage();
            message = (SetHealthMessage)GetNextReceivedMessage();
            message = (MoveEntityMessage)GetNextReceivedMessage();
            message = (DieMessage)GetNextReceivedMessage();
            message = (DeleteEntityMessage)GetNextReceivedMessage();
        }
    }
}
