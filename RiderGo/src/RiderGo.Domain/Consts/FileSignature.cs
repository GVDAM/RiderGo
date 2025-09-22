namespace RiderGo.Domain.Consts
{
    public static class FileSignature
    {

        public static readonly Dictionary<byte[], string> ImageFileSignatures = new()
        {
            { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, "png" },
            { new byte[] { 0x42, 0x4D }, "bmp" }
        };
    }
}
