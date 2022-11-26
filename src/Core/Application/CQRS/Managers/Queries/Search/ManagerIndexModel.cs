﻿using System;

namespace Application.CQRS.Managers.Queries.Search;

public class ManagerIndexModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime CreatedAt { get; set; }
}