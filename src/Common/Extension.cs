using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using ProductmanagementCore.Models.ModelInput;

namespace ProductmanagementCore.Common
{
    public static class Extension
    {
        public static IEnumerable<TModel> SortList<TModel>(this IEnumerable<TModel> source, CriteriaRequest modelSort)
        {
            //const string defaultOrderString = "Id desc";
            var orderedQueryable = source.AsQueryable();
            try
            {
                if (!string.IsNullOrEmpty(modelSort.SortColumn))
                {
                    var column = modelSort.SortColumn;
                    var sortDesc = modelSort.SortDesc != null && (bool)modelSort.SortDesc;
                    var isDesc = sortDesc ? "desc" : "asc";
                    var orderString = column + " " + isDesc;
                    orderedQueryable = orderedQueryable.AsQueryable().OrderBy(orderString);
                }
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }

            return orderedQueryable;
        }
        public static bool IsNull(this object T)
        {
            return T == null;
        }


        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            var tb = new DataTable(typeof(T).Name);
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];

                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }
    }
}
