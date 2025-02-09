using MtnDogShared;

namespace MtnDogLogger
{
    public class LogFile
    {
        private DateTime StartTime;

        private string FileName;

        public LogFile(DateTime startTime)
        {
            StartTime = startTime;
            FileName = $"{StartTime.ToFileTime()}.csv";
        }

        public static LogFile GetLog(string fileName)
        {
            var logStartTime = long.Parse(fileName.Replace(".csv", ""));

            return new LogFile(DateTime.FromFileTime(logStartTime));
        }

        public async Task AppendLogAsync(LogMessage message)
        {
            await AppendFileAsync(message.ToCsv());
        }

        public int GetLineCount()
        {
            var lineCount = File.ReadLines(FileName);

            return lineCount.Count();
        }

        public async IAsyncEnumerable<string> ReadLogLinesAsync()
        {
            using (StreamReader reader = new StreamReader(FileName))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    yield return line;
                }
            }
        }

        public async IAsyncEnumerable<string> ReadLogMessagesAsync()
        {
            using (StreamReader reader = new StreamReader(FileName))
            {
                string line;
                int lineNum = 0;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    yield return LogMessage.ParseCsv(line, lineNum++).EncodeMessage();
                }
            }
        }

        private async Task AppendFileAsync(string text)
        {
            using (StreamWriter logFile = new StreamWriter(FileName, true))
            {
                await logFile.WriteLineAsync(text);
            }
        }
    }
}
