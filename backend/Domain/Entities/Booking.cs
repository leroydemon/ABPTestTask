﻿using Domain.Entities;

public class Booking : Entity
{
    public Guid Id { get; set; }
    public Guid HallId { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? EquipmentsListJson { get; set; }
    public List<Equipment>? Equipments { get; set; }
    public bool IsConfirmed { get; set; } 
}