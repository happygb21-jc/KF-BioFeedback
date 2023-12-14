using Kingfar.BioFeedback.Shared.Abstractions.Common;

namespace Kingfar.BioFeedback.Shared
{
    public class FilePathConst
    {
        private static readonly string _basePath = SysSetting.Current.DataSavePath;

        private static readonly string _dbBasePath = SysSetting.Current.DBSavePath;
        public static readonly string _dbPath = Path.Combine(_dbBasePath, SysSetting.SystemName, "DB");

        private static readonly string ResourcePath = Path.Combine(_basePath, SysSetting.SystemName, "resources");
        public static readonly string ProjectPath = Path.Combine(_basePath, SysSetting.SystemName,"project");
        public static readonly string SchemePath = Path.Combine(ResourcePath, "schemes");
        public static readonly string SchemeSetPath = Path.Combine(ProjectPath, "schemeSet");
        public static readonly string SchemeTemplatePath = Path.Combine(ProjectPath, "schemeTemplates");

        public static string GetSchemeConfigPath(Guid schemeId) => Path.Combine(SchemePath, schemeId.ToString());

        public static string GetSchemeSetConfigPath(Guid schemeSetId) => Path.Combine(SchemeSetPath, schemeSetId.ToString());

        public static string GetTrainConfigPath(string schemeSetPath) => Path.Combine(schemeSetPath, "trains");

        public static string GetTrainReportPath(string schemeSetPath) => Path.Combine(schemeSetPath, "reports");

        public static string GetSchemeSetTempConfigPath(Guid schemeSetTempId) => Path.Combine(SchemeTemplatePath, schemeSetTempId.ToString());

        public static string GetTrainTemplateConfigPath(string schemeSetTempPath) => Path.Combine(schemeSetTempPath, "trainTemps");
    }
}