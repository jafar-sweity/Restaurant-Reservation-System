﻿using FluentValidation;
using RestaurantReservation.API.Models.OrderItem;

namespace RestaurantReservation.API.Validators.OrderItems
{
    public class OrderItemUpdateValidator : AbstractValidator<OrderItemUpdateDto>
    {
        public OrderItemUpdateValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
