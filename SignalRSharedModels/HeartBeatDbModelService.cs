using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SignalRSharedModels
{
    public interface IHeartBeatDbModelService
    {
        public void SaveMessage(HeartBeatDbModel data);

        public List<HeartBeatDbModel> GetLast10HeartBeats();
    }
    
    public class HeartBeatDbModelService : IHeartBeatDbModelService
    {
        private HeartBeatModelContext _dbCtx;
        private IConfiguration _appConfig;

        public HeartBeatDbModelService(IConfiguration configuration, HeartBeatModelContext dbCtx)
        {
            _appConfig = configuration;
            _dbCtx = dbCtx;
        }

        public void SaveMessage(HeartBeatDbModel data)
        {
            data.timeStamp = DateTime.Now.ToString();
            _dbCtx.Add(data);
            _dbCtx.SaveChanges();
        }

        public List<HeartBeatDbModel> GetLast10HeartBeats()
        {
            using (_dbCtx)
            {
                return _dbCtx.HeartBeats.OrderByDescending(p => p.timeStamp).Take(5).ToList();
            }
        }
    }
}