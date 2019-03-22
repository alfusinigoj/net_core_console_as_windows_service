using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace My.Core.Windows.Service
{
    public class FileCreateProcessor : IProcessor
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<FileCreateProcessor> logger;

        public FileCreateProcessor(IConfiguration configuration, ILogger<FileCreateProcessor> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public void Execute()
        {
            var outputPath = configuration["OutputFilePath"];

            Directory.CreateDirectory(outputPath);

            var currentDateTime = DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss-tt");

            var fileName = Path.Combine(outputPath, $"{currentDateTime}.txt");

            logger.LogInformation($"Creating file {fileName}");

            File.WriteAllText(fileName, currentDateTime);
        }
    }

    public interface IProcessor
    {
        void Execute();
    }
}
