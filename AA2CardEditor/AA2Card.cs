using System;
using System.Drawing;
using System.IO;

namespace AA2CardEditor
{
    public class AA2Card
    {
        private string filePath = "";
        private MemoryStream faceImageStream = null;
        private MemoryStream editorFileStream = null;

        public AA2Card()
        {
            IsDirty = false;
        }

        public string FileName
        {
            get
            {
                return Path.GetFileName(filePath);
            }
        }

        public Image FaceImage
        {
            get
            {
                if (faceImageStream == null)
                {
                    return null;
                }
                else
                {
                    faceImageStream.Seek(0, SeekOrigin.Begin);
                    return Image.FromStream(faceImageStream);
                }
            }
        }

        public bool IsDirty { get; private set; }

        public void ReadCardFile(String filePath)
        {
            // Load the file at the path into a memory stream.
            byte[] cardFile = File.ReadAllBytes(filePath);
            MemoryStream cardFileStream = new MemoryStream(
                cardFile, 0, cardFile.Length, false, true);

            // Try to read a PNG from the card file stream.
            MemoryStream faceImageStream = PngFileReader.Read(cardFileStream);

            // Try to read an editor file from the card file stream.
            MemoryStream editorFileStream = EditorFileReader.Read(cardFileStream);

            // Success. Update card state and mark card as clean.
            this.filePath = filePath;
            this.faceImageStream = faceImageStream;
            this.editorFileStream = editorFileStream;
            this.IsDirty = false;
        }

        public void ReplaceFaceImage(String filePath)
        {
            // Load the file at the path into a memory stream.
            byte[] imageFile = File.ReadAllBytes(filePath);
            MemoryStream imageFileStream = new MemoryStream(
                imageFile, 0, imageFile.Length, false, true);

            // Try to read a PNG from the image file stream.
            MemoryStream faceImageStream = PngFileReader.Read(imageFileStream);

            // Success. Update card state and mark card as dirty.
            this.faceImageStream = faceImageStream;
            this.IsDirty = true;
        }

        public void Save()
        {
            Save(filePath);
        }

        public void Save(String filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                // Copy the face image to the file stream.
                faceImageStream.Seek(0, SeekOrigin.Begin);
                faceImageStream.CopyTo(fs);

                // Copy the editor file to the file stream.
                editorFileStream.Seek(0, SeekOrigin.Begin);
                editorFileStream.CopyTo(fs);

                // Update card state and mark card as clean.
                this.filePath = filePath;
                IsDirty = false;
            }
        }
    }
}
