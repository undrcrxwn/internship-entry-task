using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
}