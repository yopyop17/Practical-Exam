using Exam_Project.Api.Commands;
using Exam_Project.Api.DTO;
using Exam_Project.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Contracts
{
    public interface IOrderService
    {
        Task<Response<OrderDTO>> NewOrder(OrderCommand command);
    }
}
