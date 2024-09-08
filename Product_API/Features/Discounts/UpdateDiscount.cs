﻿using BaseDomain.Results;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Domains.Discounts;
using Product_API.Events.DiscountEvents;
using Product_API.Interfaces;

namespace Product_API.Features.Discounts;

public static class UpdateDiscount
{
    public record Command(string DiscountId, DiscountRequest Request) : IRequest<Result>;

    public class Handler(IDiscountRepository repository, IMediator mediator)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await repository.GetByIdAsync(request.DiscountId);
            if (!result.IsFailure)
            {
                var discount = result.Value;
                var discountRequest = request.Request;

                discount.Update(
                    discountRequest.Name,
                    discountRequest.Percent,
                    discountRequest.StartDate,
                    discountRequest.EndDate
                );
                await repository.UpdateAsync(discount);

                var @event = new UpdateDiscountEvent(discount.DiscountId, discountRequest.Percent);
                await mediator.Publish(@event, cancellationToken);
            }

            return result;
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut(
                "/api/discount",
                async ([FromBody] Command command, ISender sender) =>
                {
                    var result = await sender.Send(command);
                    return result.IsFailure ? Results.Ok() : Results.BadRequest(result.ErrorTypes);
                }
            );
        }
    }
}
