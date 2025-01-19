using MultiDownloader.DownloaderApi.Downloader.Models;

namespace MultiDownloader.DownloaderApi.Downloader.Executors
{
    internal class LinuxCmdExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns>cmd execution output result</returns>
        public static string RunCommand(string command)
        {
            string result = "";
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \" " + command + " \"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                result += proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();

                proc.WaitForExit();
            }

            return result;
        }
    }
}
