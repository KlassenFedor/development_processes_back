﻿namespace dev_processes_backend.Models.Dtos.Companies.RequestModels;

public class CreateCompanyRequestModel
{
    public string Name { get; set; }
    public string Site { get; set; }
    public string Information { get; set; }
}