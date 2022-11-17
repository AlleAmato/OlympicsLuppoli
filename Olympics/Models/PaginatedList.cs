using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olympics.Models
{
    internal class PaginatedList<T> 
    {
        public int TotalCount { get; }
        public List<T> Data { get; }

        public PaginatedList(int totalCount, List<T> data)
        {
            TotalCount = totalCount;
            Data = data;
        }
    }
}
