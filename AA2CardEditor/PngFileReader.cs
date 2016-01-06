using System;
using System.IO;
using System.Linq;
using System.Net;

namespace AA2CardEditor
{
    class PngFileReader
    {
        private static readonly byte[] PngFileSignature = { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };

        private static readonly int PngChunkTypeLength = 4;
        private static readonly int PngChunkOverheadLength = 12;

        private static readonly byte[] PngHeaderChunkType = { 0x49, 0x48, 0x44, 0x52 };
        private static readonly byte[] PngDataChunkType = { 0x49, 0x44, 0x41, 0x54 };
        private static readonly byte[] PngEndChunkType = { 0x49, 0x45, 0x4e, 0x44 };

        public static MemoryStream Read(MemoryStream inputStream)
        {
            byte[] inputBuffer = inputStream.GetBuffer();
            MemoryStream outputStream = new MemoryStream();

            using (WrappingStream wrappingStream = new WrappingStream(inputStream))
            using (BinaryReader reader = new BinaryReader(wrappingStream))
            {
                // Read the signature and verify that it's equal to the PNG signature.
                byte[] fileSignature = reader.ReadBytes(PngFileSignature.Length);

                if (!fileSignature.SequenceEqual(PngFileSignature))
                {
                    throw new Exception("The file is not a PNG format file.");
                }

                // Copy the PNG file signature to the card face memory stream.
                outputStream.Write(fileSignature, 0, fileSignature.Length);

                // Copy all of the critical chunks to the card face memory stream.
                byte[] chunkType = new byte[PngChunkTypeLength];
                do
                {
                    int chunkBeginPosition = (int)reader.BaseStream.Position;

                    int chunkLength = IPAddress.NetworkToHostOrder(reader.ReadInt32());
                    chunkType = reader.ReadBytes(PngChunkTypeLength);

                    if (chunkType.SequenceEqual(PngHeaderChunkType) ||
                        chunkType.SequenceEqual(PngDataChunkType) ||
                        chunkType.SequenceEqual(PngEndChunkType))
                    {
                        outputStream.Write(inputBuffer, (int)chunkBeginPosition,
                            chunkLength + PngChunkOverheadLength);
                    }

                    reader.BaseStream.Seek(chunkLength + 4, SeekOrigin.Current);
                } while (!chunkType.SequenceEqual(PngEndChunkType));
            }
            return outputStream;
        }
    }
}
