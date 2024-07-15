using System.ComponentModel.DataAnnotations;
using kafkaclient.web.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace kafkaclient.web.Core.Entities;

public class Cluster : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(20)]
    public string Version { get; set; }
    [Required]
    [MaxLength(100)]
    public string Host { get; set; }
    [Required]
    [MaxLength(250)]
    public string Path { get; set; }
    [Required]
    public bool Status { get; set; } = true;

    [Required]
    public string Slug { get; set; }
}