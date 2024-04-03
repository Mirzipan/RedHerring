namespace RedHerring.Studio.Import;

internal static class CollectionUtils
{
	public static bool ResizeAndUpdateList<T>(
		ref List<T>? list,
		int desiredCount,
		Func<T, int, bool> needsUpdate,
		Action<int> update)
	{
		bool changed = false;

		list ??= new List<T>();
		for (int i = 0; i < desiredCount; ++i)
		{
			if (i == list.Count)
			{
				list.Add(default);
				update(i);
				changed = true;
			}
			else if (needsUpdate(list[i], i))
			{
				update(i);
				changed = true;
			}
		}

		if (list.Count > desiredCount)
		{
			list.RemoveRange(desiredCount, list.Count - desiredCount);
			changed = true;
		}

		return changed;
	}
}