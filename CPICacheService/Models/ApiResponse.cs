﻿namespace CPICacheService.Models
{
    public class ApiResponse
    {
        public string status {  get; set; }
        public List<Cpi> Cpis { get; set; } = new List<Cpi>();
        public bool RequestSucceeded => status == "REQUEST_SUCCEEDED";
    }
}
