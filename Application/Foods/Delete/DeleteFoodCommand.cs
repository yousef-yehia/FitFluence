using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Foods.Delete
{
    public record DeleteFoodCommand(int Id) : IRequest;
}
