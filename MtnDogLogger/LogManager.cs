namespace MtnDogLogger
{
    public class LogManager
    {
        private string filePath = "";

        public LogManager() 
        {
            // TODO: Set better file path
            filePath = Directory.GetCurrentDirectory();
        }

        public IEnumerable<string> GetLogList()
        {
            try
            {
                var logList = Directory.EnumerateFiles(filePath, "*.csv");

                return logList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed reading directory.  Exception: {ex.ToString()}");

                return new List<string>();
            }
        }

        public void DeleteLog(string fileName)
        {
            try
            {
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete log file.  Exception: {ex.ToString()}");
            }
        }

        public void DeleteAllLogs()
        {
            var logList = GetLogList();

            foreach (var log in logList)
            {
                DeleteLog(log);
            }
        }
    }
}
