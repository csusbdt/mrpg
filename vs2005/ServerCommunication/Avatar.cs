using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCommunication
{
    public class Avatar
    {
        #region Fields

        string avatarName;
        string avatarClass;
        int level;

        #endregion

        #region Properties

        public string AvatarId
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

        public Avatar(string avatarName, string avatarClass, int level)
        {
            this.avatarClass = avatarClass;
            this.avatarName = avatarName;
            this.level = level;
        }

        #endregion
    }
}
