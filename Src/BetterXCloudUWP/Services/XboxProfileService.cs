using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.Achievements;
using Microsoft.Xbox.Services.System;
using Microsoft.Xbox.Services.Statistics.Manager;//.Stats.Manager;

namespace BetterXCloudUWP.Services
{
    public class XboxProfileService
    {
        private XboxLiveUser _user;
        private XboxLiveContext _context;

        public XboxProfileService(XboxLiveUser user)
        {
            _user = user;
            _context = new XboxLiveContext(_user);
        }

        public async Task<string> GetGamerTagAsync()
        {
            // Fix: Replace the call to the non-existent GetProfileAsync with the Gamertag property
            return _user.Gamertag;
        }

        public async Task<IList<string>> GetGamesAsync()
        {
            //var titleHistory = await _context.ProfileService.GetTitleHistoryAsync(_user.XboxUserId);
            //if (titleHistory == null || titleHistory.Items == null)
                return new List<string> { "Нет данных" };
            //return titleHistory.Items.Select(t => t.Name).ToList();
        }

        public async Task<IList<string>> GetAchievementsAsync()
        {
            // Получение ачивок для первой доступной игры из истории
            //var titleHistory = await _context.ProfileService.GetTitleHistoryAsync(_user.XboxUserId);
            //if (titleHistory == null || titleHistory.Items == null || titleHistory.Items.Count == 0)
                return new List<string> { "Нет данных" };
            //var titleId = titleHistory.Items[0].TitleId;
            //var achievementsResult = await _context.AchievementService.GetAchievementsForTitleIdAsync(_user.XboxUserId, titleId);
            //if (achievementsResult == null || achievementsResult.Items == null)
            //    return new List<string> { "Нет данных" };
            //return achievementsResult.Items.Select(a => $"{a.Name} ({a.ProgressState})").ToList();
        }
    }
}
