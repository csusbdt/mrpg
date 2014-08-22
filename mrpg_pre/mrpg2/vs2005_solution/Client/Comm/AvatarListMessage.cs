using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class AvatarListMessage : Message
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

        //public AvatarListMessage(List<Avatar> avatars) 
        //{
        //    this.avatars = avatars;
        //}

        public static Message Read(BinaryReader binaryReader)
        {
            Log.Write();
            AvatarListMessage avatarListMessage = new AvatarListMessage();
            int numberOfAvatars = binaryReader.ReadInt32();
            for (int i = 0; i < numberOfAvatars; ++i)
            {
                string avatarId = binaryReader.ReadString();
                string avatarClass = binaryReader.ReadString();
                Avatar avatar = new Avatar(avatarId, avatarClass);
                avatarListMessage.avatars.Add(avatar);
            }
            return avatarListMessage;
        }

        #endregion

    }
}
