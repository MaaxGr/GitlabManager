using System;
using System.IO;
using GitlabManager.Services.Gitlab.Model;
using GitlabManager.Services.Logging;
using Newtonsoft.Json;

namespace GitlabManager.Services.Cache
{
    /// <summary>
    /// Implementation for <see cref="GitlabManager.Services.Cache.IJsonCache"/> that accesses the file system
    /// </summary>
    public class JsonCache : IJsonCache
    {
        private readonly string _gitlabManagerCacheFolder;
        
        public JsonCache()
        {
            var baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var gitlabManagerFolder = $"{baseFolder}/GitlabManager";
            _gitlabManagerCacheFolder = $"{gitlabManagerFolder}/Cache";
            
            if (!Directory.Exists(gitlabManagerFolder))
            {
                Directory.CreateDirectory(gitlabManagerFolder);
            }

            if (!Directory.Exists(_gitlabManagerCacheFolder))
            {
                Directory.CreateDirectory(_gitlabManagerCacheFolder);
            }
        }

        public void WriteProject(int projectId, JsonProject jsonProject)
        {
            var fileName = $"project-{projectId}.json";
            var json = JsonConvert.SerializeObject(jsonProject);
            var filePath = $"{_gitlabManagerCacheFolder}/{fileName}";
            
            LoggingService.LogD($"File: {filePath}");

            var file = File.CreateText(filePath);
            file.Write(json);
            file.Flush();
            file.Close();
        }

        public void DeleteCache(int projectId)
        {
            var fileName = $"project-{projectId}.json";
            var filePath = $"{_gitlabManagerCacheFolder}/{fileName}";
            File.Delete(filePath);
        }

        public JsonProject ReadProject(int projectId)
        {
            var fileName = $"project-{projectId}.json";
            var filePath = $"{_gitlabManagerCacheFolder}/{fileName}";

            // return null if not existing
            if (!File.Exists(fileName))
            {
                return null;
            }
            
            try
            {
                // return deserialized json object 
                var jsonContent = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<JsonProject>(jsonContent);
            }
            catch (Exception e)
            {
                // print error to console and return null
                LoggingService.LogD(e.ToString());
                return null;
            }
        }
        
    }
}