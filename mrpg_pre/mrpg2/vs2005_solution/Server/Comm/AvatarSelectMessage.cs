using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server
{
    class AvatarSelectMessage : Message
    {
        #region Fields

        string avatarId;

        #endregion

        #region Properties

        public string AvatarId
        {
            get { return avatarId; }
        }

        #endregion

        #region Initialization

        private AvatarSelectMessage()
        {
        }

        #endregion

        #region Read

        public static Message Read(BinaryReader binaryReader)
        {
            Log.Write();
            AvatarSelectMessage avatarSelectMessage = new AvatarSelectMessage();
            avatarSelectMessage.avatarId = binaryReader.ReadString();
            return avatarSelectMessage;
        }

        #endregion
    }
}
