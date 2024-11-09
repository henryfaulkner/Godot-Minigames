using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public static class IncludesHelper
{
	public static Func<IQueryable<Egg>, IQueryable<Egg>>[] GetEggIncludes()
	{
		return new Func<IQueryable<Egg>, IQueryable<Egg>>[]
		{
			query => query.Include(e => e.AnimalType)
		};
	}
	
	public static Func<IQueryable<Animal>, IQueryable<Animal>>[] GetAnimalIncludes()
	{
		return new Func<IQueryable<Animal>, IQueryable<Animal>>[]
		{
			query => query.Include(e => e.AnimalType),
			query => query.Include(e => e.Egg)
		};
	}
}
