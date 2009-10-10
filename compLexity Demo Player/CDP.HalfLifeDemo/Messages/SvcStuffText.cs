﻿using System;
using BitReader = CDP.Core.BitReader;
using BitWriter = CDP.Core.BitWriter;
using System.IO;

namespace CDP.HalfLife.Messages
{
    public class SvcStuffText : EngineMessage
    {
        public override byte Id
        {
            get { return (byte)EngineMessageIds.svc_stufftext; }
        }

        public override string Name
        {
            get { return "svc_stufftext"; }
        }

        public override bool CanSkipWhenWriting
        {
            get { return true; }
        }

        public string Text { get; set; }

        public override void Skip(BitReader buffer)
        {
            buffer.SeekString();
        }

        public override void Read(BitReader buffer)
        {
            Text = buffer.ReadString();
        }

        public override void Write(BitWriter buffer)
        {
            buffer.WriteString(Text);
        }

        public override void Log(StreamWriter log)
        {
            log.WriteLine("Text: \"{0}\"", Text);
        }
    }
}
