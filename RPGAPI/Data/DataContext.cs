using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RpgApi.Models;
using RPGAPI.Models;
using RPGAPI.Models.Enums;
using RPGAPI.Utils;

namespace RPGAPI.Data
{
    public class DataContext : DbContext
    {
        //Classe das configurações do banco de dados
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        //Referenciar igual ao contexto do banco de dados   
        public DbSet<Personagem> TB_PERSONAGENS { get; set; }
        public DbSet<Arma> TB_ARMAS { get; set; }
        public DbSet<Usuario> TB_USUARIOS { get; set; }
        public DbSet<Habilidade> TB_HABILIDADES { get; set; }
        public DbSet<PersonagemHabilidade> TB_PERSONAGENS_HABILIDADES { get; set; }

        //Método que já existe e o override é uma sobrescrição do método, nos permite personalizar ele
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Relacionando a tabela
            modelBuilder.Entity<Personagem>().ToTable("TB_PERSONAGENS");
            modelBuilder.Entity<Arma>().ToTable("TB_ARMAS");
            modelBuilder.Entity<Usuario>().ToTable("TB_USUARIOS");
            modelBuilder.Entity<Habilidade>().ToTable("TB_HABILIDADES");
            modelBuilder.Entity<PersonagemHabilidade>().ToTable("TB_PERSONAGENS_HABILIDADES");

            //Relacionamento One to Many (Um para Muitos)
            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Personagens)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioId)
                .IsRequired(false);

            //Relacionamento One to One (Um para Um)
            modelBuilder.Entity<Personagem>()
                .HasOne(e => e.Arma)
                .WithOne(e => e.Personagem)
                .HasForeignKey<Arma>(e => e.PersonagemId)
                .IsRequired();
            
            modelBuilder.Entity<PersonagemHabilidade>()
                .HasKey(ph => new {ph.PersonagemId, ph.HabilidadeId });

            modelBuilder.Entity<Personagem>().HasData
            (
                new Personagem() { Id = 1, Nome = "Frodo", PontosVida = 100, Forca = 17, Defesa = 23, Inteligencia = 33, Classe = ClasseEnum.Cavaleiro, UsuarioId = 1 },
                new Personagem() { Id = 2, Nome = "Sam", PontosVida = 100, Forca = 15, Defesa = 25, Inteligencia = 30, Classe = ClasseEnum.Cavaleiro, UsuarioId = 1 },
                new Personagem() { Id = 3, Nome = "Galadriel", PontosVida = 100, Forca = 18, Defesa = 21, Inteligencia = 35, Classe = ClasseEnum.Clerigo, UsuarioId = 1 },
                new Personagem() { Id = 4, Nome = "Gandalf", PontosVida = 100, Forca = 18, Defesa = 18, Inteligencia = 37, Classe = ClasseEnum.Mago, UsuarioId = 1 },
                new Personagem() { Id = 5, Nome = "Hobbit", PontosVida = 100, Forca = 20, Defesa = 17, Inteligencia = 31, Classe = ClasseEnum.Cavaleiro, UsuarioId = 1 },
                new Personagem() { Id = 6, Nome = "Celeborn", PontosVida = 100, Forca = 21, Defesa = 13, Inteligencia = 34, Classe = ClasseEnum.Clerigo, UsuarioId = 1 },
                new Personagem() { Id = 7, Nome = "Radagast", PontosVida = 100, Forca = 25, Defesa = 11, Inteligencia = 35, Classe = ClasseEnum.Mago, UsuarioId = 1 }
            );

            modelBuilder.Entity<Arma>().HasData
            (
                new Arma() { Id = 1, Nome = "Arco e Flecha", Dano = 35, PersonagemId = 1},
                new Arma() { Id = 2, Nome = "Espada", Dano = 33, PersonagemId = 2 },
                new Arma() { Id = 3, Nome = "Machado", Dano = 31, PersonagemId = 3 },
                new Arma() { Id = 4, Nome = "Punho", Dano = 30, PersonagemId = 4 },
                new Arma() { Id = 5, Nome = "Chicote", Dano = 34, PersonagemId = 5 },
                new Arma() { Id = 6, Nome = "Foice", Dano = 33, PersonagemId = 6 },
                new Arma() { Id = 7, Nome = "Cajado", Dano = 32, PersonagemId = 7 }
            );

            modelBuilder.Entity<Habilidade>().HasData
            (
                new Habilidade() { Id = 1, Nome = "Adormecer", Dano = 39 },
                new Habilidade() { Id = 2, Nome = "Congelar", Dano = 41 },
                new Habilidade() { Id = 3, Nome = "Hipnotizar", Dano = 37 }
            );

            modelBuilder.Entity<PersonagemHabilidade>().HasData
            (
                new PersonagemHabilidade() { PersonagemId = 1, HabilidadeId = 1 },
                new PersonagemHabilidade() { PersonagemId = 1, HabilidadeId = 2 },
                new PersonagemHabilidade() { PersonagemId = 2, HabilidadeId = 2 },
                new PersonagemHabilidade() { PersonagemId = 3, HabilidadeId = 2 },
                new PersonagemHabilidade() { PersonagemId = 3, HabilidadeId = 3 },
                new PersonagemHabilidade() { PersonagemId = 4, HabilidadeId = 3 },
                new PersonagemHabilidade() { PersonagemId = 5, HabilidadeId = 1 },
                new PersonagemHabilidade() { PersonagemId = 6, HabilidadeId = 2 },
                new PersonagemHabilidade() { PersonagemId = 7, HabilidadeId = 3 }
            );

            //Criação de usuário padrão
            Usuario user = new Usuario();
            user.Id = 1;
            user.Username = "UsuarioAdmin";;
            user.PasswordString = string.Empty;
            user.PasswordHash = Convert.FromBase64String("Fv9xjOVqzVFAqQxHj1Q6J+rz1XIN7ZrjPU1oVYBeAMH59/6rjD6TjYMMvU4IY0ZmWptzYMZ9cy82I+woz0wghA==");
            user.PasswordSalt = Convert.FromBase64String("E36pZ+vHY6Y52JgR7B6Pb1KHJ4U7GZZJKQ7ZcfHqrx8FD9nYkmwHgZrR9eK6lZoVW6QZKkbStM+Z3Wx41NOjVg==");
            user.Perfil = "Admin";
            user.Email = "seuEmail@gmail.com";
            user.Latitude = -23.5200241;
            user.Longitude = -46.596498;

            modelBuilder.Entity<Usuario>().HasData(user);
            //Fim da criação do usuário padrão

            //Define que se o Perfil não for informado, o valor padrão será jogador
            modelBuilder.Entity<Usuario>().Property(u => u.Perfil).HasDefaultValue("Jogador");
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("Varchar").HaveMaxLength(200);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => 
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        //base.OnModelCreating(modelBuilder);

    }
}