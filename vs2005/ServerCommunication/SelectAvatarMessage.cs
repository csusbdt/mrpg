using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServerCommunication
{
    public class SelectAvatarMessage : Message
    {
        #region Fields

        string avatarName;

        #endregion

        #region Properties

        public string AvatarName
        {
            get { return avatarName; }
        }

        #endregion

        #region Initialization

        SelectAvatarMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            SelectAvatarMessage message = new SelectAvatarMessage();
            message.avatarName = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
