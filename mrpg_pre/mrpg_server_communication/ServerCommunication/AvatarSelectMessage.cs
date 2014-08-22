using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server.Communication
{
    public class AvatarSelectMessage : Message
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

        AvatarSelectMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            AvatarSelectMessage message = new AvatarSelectMessage();
            message.avatarName = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
