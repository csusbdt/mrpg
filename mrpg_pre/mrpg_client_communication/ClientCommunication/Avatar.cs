using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class Avatar
    {
        #region Fields

        string avatarName;
        string avatarClass;
        int level;

        #endregion

        #region Properties

        public string AvatarName
        {
            get { return avatarName; }
        }

        public string AvatarClass
        {
            get { return avatarClass; }
        }

        public int Level
        {
            get { return level; }
        }

        #endregion

        #region Initialization

        Avatar()
        {
        }

        internal static Avatar Read(BinaryReader binaryReader)
        {
            Avatar avatar = new Avatar();
            avatar.avatarName = binaryReader.ReadString();
            avatar.avatarClass = binaryReader.ReadString();
            avatar.level = binaryReader.ReadInt32();
            return avatar;
        }

        #endregion
    }
}
