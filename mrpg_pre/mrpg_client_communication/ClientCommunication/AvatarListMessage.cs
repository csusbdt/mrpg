using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class AvatarListMessage : Message
    {
        #region Fields

        List<Avatar> avatars = new List<Avatar>();

        #endregion

        #region Properties

        public List<Avatar> AvatarList
        {
            get { return avatars; }
        }

        #endregion

        #region Initialization

        AvatarListMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            AvatarListMessage avatarListMessage = new AvatarListMessage();
            int numberOfAvatars = binaryReader.ReadInt32();
            for (int i = 0; i < numberOfAvatars; ++i)
            {
                Avatar avatar = Avatar.Read(binaryReader);
                avatarListMessage.avatars.Add(avatar);
            }
            return avatarListMessage;
        }

        #endregion
    }
}
