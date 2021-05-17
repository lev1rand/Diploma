using System;

namespace DiplomaServices.Pagination
{
    public class PagedResponse<T> 
    {
        public T Data { get; set; }

        //Number of the page selected
        public int PageNumber { get; set; }

        //Quantity of Data Records for each page
        public int PageSize { get; set; }

        //Count of Pages
        public decimal TotalPages { get; set; }

        //Quantity of Total Data Records
        public int TotalRecords { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int total)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            TotalRecords = total;
            TotalPages = Math.Ceiling((decimal)total / pageSize);
        }
    }
}
