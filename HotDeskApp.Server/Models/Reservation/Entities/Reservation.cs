﻿using HotDeskApp.Server.Infrastructure;

namespace HotDeskApp.Server.Models.Reservation.Entities;

public class Reservation : Entity
{
    public Reservation()
    {
    }

    public Reservation(Guid id) : base(id)
    {
    }

    public virtual Guid UserId { get; set; }
    public virtual Guid DeskId { get; set; }
    public virtual DateTime StartDate { get; set; }
    public virtual DateTime EndDate { get; set; }
}