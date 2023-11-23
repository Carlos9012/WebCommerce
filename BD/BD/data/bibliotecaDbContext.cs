using BD.Models.socialMedia;
using BD.Models.handbool;
using BD.Models.sistemAcademico;
using Microsoft.EntityFrameworkCore;
using BD.Models.biblioteca;

namespace BD.data;

public class bancoDeDadosDbContext : DbContext
{
    public bancoDeDadosDbContext(DbContextOptions options) : base(options)
    {
    }

    //biblioteca
    public DbSet<emprestimo> emprestimo { get; set; }
    public DbSet<Models.biblioteca.user> user { get; set; }
    public DbSet<livro> livro { get; set; }
    public DbSet<genero> genero { get; set; }

    //sistema Academico
    //public DbSet<aluno> alunos { get; set; }
    //public DbSet<curso> curso { get; set; }
    //public DbSet<disciplina_aluno> disciplina_Alunos { get; set; }
    //public DbSet<disciplina_curso> disciplina_Cursos { get; set; }
    //public DbSet<matricula> matricula { get; set; }
    //public DbSet<turma> turma { get; set; }

    //public DbSet<turma_aluno> Turma_Alunos { get; set; }

    //handbool
    //public DbSet<estadio> estadios { get; set; }
    //public DbSet<historico_partidas> historico_Partidas { get; set; }
    //public DbSet<jogador> jogadors { get; set; }
    //public DbSet<partida> partida { get; set; }
    //public DbSet<time>  times { get; set; }

    //social media
    //public DbSet<user> Users { get; set; }
    //public DbSet<seguidores> Seguidores { get; set; }
    //public DbSet<foto> Fotos { get; set; }
    //public DbSet<album> Albums { get; set; }
    //public DbSet<timeline> Timelines { get; set; }

}
