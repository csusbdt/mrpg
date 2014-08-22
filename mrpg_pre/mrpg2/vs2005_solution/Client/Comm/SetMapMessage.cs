using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class SetMapMessage : Message
    {
        string mapId;

        public string MapId
        {
            get { return mapId; }
        }

        SetMapMessage()
        {
        }

        public static Message Read(BinaryReader binaryReader)
        {
            Log.Write();
            SetMapMessage setMapMessage = new SetMapMessage();
            setMapMessage.mapId = binaryReader.ReadString();
            return setMapMessage;
        }

    }
}
