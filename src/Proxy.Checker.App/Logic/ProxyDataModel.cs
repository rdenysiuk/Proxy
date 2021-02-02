using Proxy.Primitives;
using System;
using System.Collections.Generic;
using System.Data;

namespace Proxy.Checker.App.Logic
{
    public class ProxyDataModel
    {
        public DataTable Table { get; }
        public List<ProxyState> Proxies { get; }

        public ProxyDataModel()
        {
            Table = new DataTable();
            var linkColumn = new DataColumn("Link", typeof(string)) { Unique = true };

            Table.Columns.Add(linkColumn);
            Table.Columns.Add("Status", typeof(string));
            Table.Columns.Add("Time", typeof(int));
            Table.Columns.Add("Error", typeof(string));
            Table.PrimaryKey = new DataColumn[] { linkColumn };
            Proxies = new List<ProxyState>();
        }

        internal void AddRange(IEnumerable<ProxyState> proxies)
        {
            foreach (ProxyState proxy in proxies)
                Add(proxy);
        }

        /// <summary>
        /// Remove all proxy elements from the <see cref="Proxies"/>
        /// </summary>
        internal void Clear()
        {
            Proxies.Clear();
            Table.Clear();
        }

        #region Private methods
        private void Add(ProxyState proxy)
        {
            try
            {
                Table.Rows.Add(
                proxy.Address,
                proxy.Status);
                Proxies.Add(proxy);
            }
            catch (ConstraintException) { }

        }
        #endregion
    }
}
