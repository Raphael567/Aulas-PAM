using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RPGAPI.Models;
using RPGAPI.Models.Enums;

namespace RPGAPI.Data
{
    public class DataContext : DbContext
    {
        //Classe das configurações do banco de dados
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        //Referenciar igual ao contexto do banco de dados   
        public DbSet<Personagem> TB_PERSONAGENS { get; set; }

        //Método que já existe e o override é uma sobrescrição do método, nos permite personalizar ele
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Relacionando a tabela
            modelBuilder.Entity<Personagem>().ToTable("TB_PERSONAGENS");

            modelBuilder.Entity<Personagem>().HasData
            (
                new Personagem() { Id = 1, Nome = "Frodo", PontosVida = 100, Forca = 17, Defesa = 23, Inteligencia = 33, Classe = ClasseEnum.Cavaleiro },
                new Personagem() { Id = 2, Nome = "Sam", PontosVida = 100, Forca = 15, Defesa = 25, Inteligencia = 30, Classe = ClasseEnum.Cavaleiro },
                new Personagem() { Id = 3, Nome = "Galadriel", PontosVida = 100, Forca = 18, Defesa = 21, Inteligencia = 35, Classe = ClasseEnum.Clerigo },
                new Personagem() { Id = 4, Nome = "Gandalf", PontosVida = 100, Forca = 18, Defesa = 18, Inteligencia = 37, Classe = ClasseEnum.Mago },
                new Personagem() { Id = 5, Nome = "Hobbit", PontosVida = 100, Forca = 20, Defesa = 17, Inteligencia = 31, Classe = ClasseEnum.Cavaleiro },
                new Personagem() { Id = 6, Nome = "Celeborn", PontosVida = 100, Forca = 21, Defesa = 13, Inteligencia = 34, Classe = ClasseEnum.Clerigo },
                new Personagem() { Id = 7, Nome = "Radagast", PontosVida = 100, Forca = 25, Defesa = 11, Inteligencia = 35, Classe = ClasseEnum.Mago }
            );

            //base.OnModelCreating(modelBuilder);
        }
    }
}