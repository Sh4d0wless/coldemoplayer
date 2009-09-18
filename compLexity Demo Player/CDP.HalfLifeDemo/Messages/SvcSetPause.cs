﻿using System;
using System.IO;
using BitReader = CDP.Core.BitReader;
using BitWriter = CDP.Core.BitWriter;

namespace CDP.HalfLifeDemo.Messages
{
    public class SvcSetPause : EngineMessage
    {
        public override byte Id
        {
            get { return (byte)EngineMessageIds.svc_setpause; }
        }

        public override string Name
        {
            get { return "svc_setpause"; }
        }

        public byte State { get; set; }

        public override void Read(BitReader buffer)
        {
            State = buffer.ReadByte();
        }

        public override byte[] Write()
        {
            BitWriter buffer = new BitWriter();
            buffer.WriteByte(State);
            return buffer.Data;
        }

#if DEBUG
        public override void Log(StreamWriter log)
        {
            log.WriteLine("State: {0}", State);
        }
#endif
    }
}