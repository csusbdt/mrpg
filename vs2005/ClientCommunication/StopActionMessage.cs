using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class StopActionMessage : Message
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

        StopActionMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            StopActionMessage message = new StopActionMessage();
            message.actionId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
