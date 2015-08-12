using ECA.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ECA.WebApi.Controllers
{
    /// <summary>
    /// The Logs controller will return current logging information for the web api.
    /// </summary>
    [RoutePrefix("api/Logs")]
    [Authorize]
    public class LogsController : ApiController
    {
        private const string CURRENT_LOG_FILE_NAME_PATTERN = @"log*resources";

        /// <summary>
        /// Returns the system logs.
        /// </summary>
        /// <param name="id">The id of the application.  KMT is 1.</param>
        /// <returns>The system logs.</returns>
        [Route("All/{id}")]
        [ResourceAuthorize(CAM.Data.Permission.ADMINISTRATOR_VALUE, CAM.Data.ResourceType.APPLICATION_VALUE)]
        public HttpResponseMessage GetAllLogs(int id)
        {
            var logFiles = new List<String>();
            var currentLog = GetCurrentLogFile();
            var archiveLogs = GetArchiveLogFiles();
            if (currentLog != null)
            {
                logFiles.Add(currentLog);
            }
            if (archiveLogs != null)
            {
                logFiles.AddRange(archiveLogs);
            }            
            using (var memoryStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var filePath in logFiles)
                    {
                        FileInfo info = new FileInfo(filePath);
                        archive.CreateEntryFromFile(info.FullName, info.Name, CompressionLevel.Optimal);
                    }
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                var content = new ByteArrayContent(memoryStream.ToArray());
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = content;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "logs.zip"
                };
                return response;
            }
        }

        /// <summary>
        /// Returns the system logs.
        /// </summary>
        /// <param name="id">The id of the application.  The KMT is 1.</param>
        /// <returns>The system logs.</returns>
        [Route("Recent/{id}")]
        [ResourceAuthorize(CAM.Data.Permission.ADMINISTRATOR_VALUE, CAM.Data.ResourceType.APPLICATION_VALUE)]
        public HttpResponseMessage GetMostRecentLog(int id)
        {
            string logContent = String.Empty;
            var currentLogFile = GetCurrentLogFile();
            if (currentLogFile != null)
            {
                logContent = File.ReadAllText(currentLogFile);
            }
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(logContent, Encoding.UTF8, "text/plain");
            return response;
        }

        private string GetCurrentLogFile()
        {
            var currentLogFolder = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data");
            var directoryInfo = new DirectoryInfo(currentLogFolder);
            if (directoryInfo.Exists)
            {
                var currentLogFiles = Directory.GetFiles(currentLogFolder, CURRENT_LOG_FILE_NAME_PATTERN);
                if (currentLogFiles.Length == 0)
                {
                    return null;
                }
                Contract.Assert(currentLogFiles.Length == 1, "There should be just one log file in the log directory.");                
                return currentLogFiles.First();
            }
            else
            {
                return null;
            }
        }

        private List<string> GetArchiveLogFiles()
        {
            var archiveLogFolder = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/archive");
            var info = new DirectoryInfo(archiveLogFolder);
            if (info.Exists)
            {
                var archiveLogFiles = Directory.GetFiles(archiveLogFolder, CURRENT_LOG_FILE_NAME_PATTERN);
                return archiveLogFiles.ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
