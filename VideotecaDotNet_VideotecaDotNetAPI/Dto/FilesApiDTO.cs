﻿namespace VideotecaDotNet_VideotecaDotNetAPI.Dto
{
    public class FilesApiDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Kind { get; set; }
        public string Description { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
