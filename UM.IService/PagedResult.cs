using System.Collections.Generic;

namespace UM.IService
{
    // 分页查询结果的自定义类
    public class PagedResult<T>
    {
        public int TotalCount { get; set; } // 表示总数据条数
        public int TotalPages { get; set; } // 表示总页数
        public List<T> Rows { get; set; }   // 表示当前页的数据列表
    }
}