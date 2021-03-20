using System.Reflection;

namespace Core.Data
{
	public static class UpdateController
	{
		public static void Update<T>(this T oldInformation, T newInfromation) where T : new()
		{
			foreach (PropertyInfo property in newInfromation.GetType().GetProperties())
			{
				if (property.SetMethod != null)
					oldInformation.GetType().GetProperty(property.Name).SetValue(oldInformation, newInfromation.GetType().GetProperty(property.Name).GetValue(newInfromation));
			}
		}
	}
}
