using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class SetMapMessage : Message
    {
        #region Fields

        string mapId;

        #endregion

        #region Properties

        public string MapId
        {
            get { return mapId; }
        }

        #endregion

        #region Initialization

        SetMapMessage()
        {
        }

        internal static SetMapMessage Read(BinaryReader binaryReader)
        {
            SetMapMessage message = new SetMapMessage();
            message.mapId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
