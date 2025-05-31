﻿using CadParcial2Derh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnParcialDerh
{
    public class SerieCln
    {
        public static int insertar(Serie serie)
        {
            using (var context = new Parcial2DerhEntities())
            {
                context.Serie.Add(serie);
                context.SaveChanges();
                return serie.id;
            }
        }

        public static int actualizar(Serie serie)
        {
            using (var context = new Parcial2DerhEntities())
            {
                var existente = context.Serie.Find(serie.id);
                existente.titulo = serie.titulo;
                existente.sinopsis = serie.sinopsis;
                existente.director = serie.director;
                existente.episodios = serie.episodios;
                existente.fechaEstreno = serie.fechaEstreno;
                existente.usuarioRegistro = serie.usuarioRegistro;

                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuario)
        {
            using (var context = new Parcial2DerhEntities())
            {
                var serie = context.Serie.Find(id);
                serie.estado = -1;
                serie.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }

        public static Serie obtenerUno(int id)
        {
            using (var context = new Parcial2DerhEntities())
            {
                return context.Serie.Find(id);
            }
        }

        public static List<Serie> listar()
        {
            using (var context = new Parcial2DerhEntities())
            {
                return context.Serie.Where(x => x.estado != -1).ToList();
            }
        }

        public static List<paSerieListar_Result> listarPa(string parametro)
        {
            using (var context = new Parcial2DerhEntities())
            {
                return context.paSerieListar(parametro).ToList();
            }
        }
    }
}