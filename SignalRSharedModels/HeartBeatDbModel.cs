using System;
using System.ComponentModel.DataAnnotations;

namespace SignalRSharedModels
{
    public class HeartBeatDbModel
    {
        [Key] public Guid guid { get; set; }
        public string randomInfo { get; set; }
        public string timeStamp { get; set; }
    }
}