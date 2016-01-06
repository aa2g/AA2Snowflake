using System;
using System.IO;
using System.Linq;

namespace AA2CardEditor
{
    class EditorFileReader
    {
        private static readonly byte[] EditorFileSignature = { 0x81, 0x79, 0x83, 0x47, 0x83, 0x66, 0x83, 0x42,
                                                               0x83, 0x62, 0x83, 0x67, 0x81, 0x7a, 0x00, 0x00};

        public static MemoryStream Read(MemoryStream inputStream)
        {
            byte[] inputBuffer = inputStream.GetBuffer();
            MemoryStream outputStream = new MemoryStream();

            using (WrappingStream wrappingStream = new WrappingStream(inputStream))
            using (BinaryReader reader = new BinaryReader(inputStream))
            {
                int editorFileBeginPosition = (int)inputStream.Position;

                // Read the signature and verify that it's equal to the editor signature.
                byte[] fileSignature = reader.ReadBytes(EditorFileSignature.Length);

                if (!fileSignature.SequenceEqual(EditorFileSignature))
                {
                    throw new Exception("The file does not appear to contain editor data.");
                }
                
                inputStream.Seek(0, SeekOrigin.End);
                int editorFileEndPosition = (int)inputStream.Position;

                // Copy the rest of the file. We're assuming it's a valid editor file.
                int editorFileLength = editorFileEndPosition - editorFileBeginPosition;
                outputStream.Write(inputBuffer, editorFileBeginPosition, editorFileLength);
            }

            return outputStream;
        }
    }
}
