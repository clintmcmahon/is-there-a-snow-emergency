using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using SnowEmergency.Models;
using SnowEmergency.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace SnowEmergency.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly HttpClient client = new HttpClient();
        private readonly ApplicationDbContext _dbContext;

        public WorkerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SnowEmergencyAlert> IsThereASnowEmergency()
        {
            var snowEmergencyNotice = await _dbContext.Notices.Where(x => x.PublishDate <= DateTime.Now && x.ExpireDate >= DateTime.Now).FirstOrDefaultAsync();
            if (snowEmergencyNotice == null)
            {
                return new SnowEmergencyAlert()
                {
                    IsThereASnowEmergency = false,
                    Body = "There's currently no Snow Emergency. Park it like it's hot."
                };
            }
            else
            {
                return new SnowEmergencyAlert()
                {
                    IsThereASnowEmergency = true,
                    Body = snowEmergencyNotice.Body
                };
            }
        }

        public async Task ProcessSnowEmergency()
        {
            var allNotices = await _dbContext.Notices.ToListAsync();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("User-Agent", "");

            string baseUrl = $"https://www.minneapolismn.gov/media/minneapolismngov/site-assets/javascript/site-wide-notices/emergency-en-1.json";

            var streamTask = client.GetStreamAsync(baseUrl);
            var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(await streamTask);
            var newNotices = new List<Notice>();
            foreach (var notice in apiResponse.NoticeApiResponse.Where(x=>x.Version != null).ToList())
            {
                var version = notice.Version;
                if (!allNotices.Any(x => x.Version == version))
                {
                    var noticeDate = DateTime.MinValue;
                    if (!string.IsNullOrEmpty(notice.Date))
                    {
                        noticeDate = DateTime.Parse(notice.Date.Remove(19, 6));
                    }

                    var noticeType = notice.NoticeType;
                    var publishDate = DateTime.MinValue;
                    if (!string.IsNullOrEmpty(notice.PublishDate))
                    {
                        publishDate = DateTime.Parse(notice.PublishDate.Remove(19, 6));
                    }

                    var expireDate = DateTime.MinValue;
                    if (!string.IsNullOrEmpty(notice.ExpireDate))
                    {
                        expireDate = DateTime.Parse(notice.ExpireDate.Remove(19, 6));
                    }

                    var idType = notice.IdType;
                    var html = notice.HTML;

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(@"<html><body>" + html + "</body></html>");

                    var messageNode = doc.DocumentNode.SelectNodes("//div[@class='message']/h2").FirstOrDefault();
                    if(messageNode != null)
                    {
                        html = messageNode.InnerText;
                    }

                    newNotices.Add(new Notice()
                    {
                        Date = noticeDate,
                        ExpireDate = expireDate,
                        Body = html,
                        IdType = idType,
                        NoticeType = noticeType,
                        PublishDate = publishDate,
                        Version = version
                    });
                }
            }

            await _dbContext.AddRangeAsync(newNotices);
            await _dbContext.SaveChangesAsync();
        }
    }

    public interface IWorkerService
    {
        Task ProcessSnowEmergency();
        Task<SnowEmergencyAlert> IsThereASnowEmergency();
    }
}
