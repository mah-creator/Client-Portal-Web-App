namespace ClientPortalApi.Paging;

public class PagedList<T>
{
	public List<T> Items { get; set; } = [];
	public int? Page { get; set; }
	public int? PageSize { get; set; }
	public int TotalCount { get; set; }
	public bool HasNext => Page * PageSize < TotalCount;
	public bool HasPrevious => Page > 1;

	public static PagedList<T> CreatePagedList(IQueryable<T>? query, int? page, int? pageSize)
	{
		var size = pageSize ?? query?.Count() ?? 20;

		var items = query;

		if (page != null && pageSize != null)
		{
			items = query?
				.Skip((page.Value - 1) * (pageSize.Value))
				.Take(size);
		}

		return new()
		{
			Items = items?.ToList() ?? [],
			Page = page,
			PageSize = pageSize,
			TotalCount = query?.Count() ?? 0
		};
	}
}