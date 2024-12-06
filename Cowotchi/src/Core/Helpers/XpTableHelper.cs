public static class XpTableHelper
{
    public static int GetLevelsXpGoal(int level)
    {
        int[] xpTable = GenerateXpTable();
        return xpTable[level];   
    }

    // DEPRECATED
    public static int[] GenerateXpTable(int maxLevel = 20, int baseXP = 100, float multiplier = 1.5f)
	{
		int[] result = new int[maxLevel + 1];  // Array to store XP required for each level

		// Generate the XP table
		for (int level = 1; level <= maxLevel; level++)
		{
			if (level == 1)
			{
				result[level] = baseXP;
			}
			else
			{
				// Calculate XP for the next level based on the previous level
				result[level] = (int)(result[level - 1] * multiplier);
			}

			// Print XP needed for each level
			_logger.LogInfo($"Level {level}: {result[level]} XP");
		}

		return result;
	}	
}