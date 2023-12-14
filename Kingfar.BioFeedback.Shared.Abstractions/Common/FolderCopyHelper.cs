namespace Kingfar.BioFeedback.Shared
{
    public static class FolderCopyHelper
    {
        public static void CopyDirectory(string sourceDirPath, string destDirPath, bool copySubDirs = true)
        {
            // 获取源文件夹信息
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirPath);

            // 检查源文件夹是否存在
            if (!sourceDir.Exists)
            {
                throw new DirectoryNotFoundException(
                    $"Source directory does not exist or could not be found: {sourceDirPath}");
            }

            // 如果目标文件夹不存在，则创建它
            if (!Directory.Exists(destDirPath))
            {
                Directory.CreateDirectory(destDirPath);
            }

            // 获取文件列表并复制所有文件
            FileInfo[] files = sourceDir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirPath, file.Name);
                file.CopyTo(tempPath, false); // false表示不覆盖已存在的文件
            }

            // 如果需要复制子目录及其内容，则递归调用此方法
            if (copySubDirs)
            {
                DirectoryInfo[] subDirs = sourceDir.GetDirectories();
                foreach (DirectoryInfo subDir in subDirs)
                {
                    string tempPath = Path.Combine(destDirPath, subDir.Name);
                    CopyDirectory(subDir.FullName, tempPath, true);
                }
            }
        }
    }
}