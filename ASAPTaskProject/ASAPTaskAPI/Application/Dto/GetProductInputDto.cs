using ASAPTaskAPI.Domain.Entities;
using ASAPTaskAPI.Helper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ASAPTaskAPI.Application.Dto
{
    public class GetProductInputDto : PaginatedInputDto
    {
        public string Search { get; set; } = string.Empty;
    }
}
