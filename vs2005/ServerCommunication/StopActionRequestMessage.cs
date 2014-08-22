using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServerCommunication
{
    public class StopActionRequestMessage : Message
    {
        #region Fields

        string actionId;

        #endregion

        #region Properties

        public string ActionId
        {
            get { return actionId; }
        }

        #endregion

        #region Initialization

        StopActionRequestMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            StopActionRequestMessage message = new StopActionRequestMessage();
            message.actionId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
