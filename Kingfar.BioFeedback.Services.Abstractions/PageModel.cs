namespace Kingfar.BioFeedback.Services;

/// <summary>
/// 通用分页信息类
/// </summary>
public class PageModel<T>
{
    public PageModel(int totalCount, List<T> data)
    {
        TotalCount = totalCount;
        Data = data;
    }

    /// <summary>
    /// 数据总数
    /// </summary>
    public int TotalCount { get; set; } = 0;

    /// <summary>
    /// 返回数据
    /// </summary>
    public List<T> Data { get; set; }
}