using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Core
{
    public class PagingParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int m_PageSize = 10;
        public int PageSize
        {
            get => m_PageSize;
            set => m_PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }

}