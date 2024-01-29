using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Algoritmo
{
    PerlinNoise, PerlinNoiseSuavizado, RandomWalk,RandomWalkSuavizado,PerlinNoiseCueva,RandomWalkCueva,TunelDireccional,MapaAleatorio,AutomataCelularMoore, AutomataCelularVonNeumann
}
public class Generador : MonoBehaviour
{
    [Header("Referencias")]
    public Tilemap mapaDeCasillas;
    public TileBase casillas;

    [Header("Dimenciones")]
    public int ancho = 60;
    public int alto = 34;

    [Header("Semilla")]
    public bool semillaAleatoria = true;
    public float semilla = 0;

    [Header("Algoritmo")]
    public Algoritmo algoritmo = Algoritmo.PerlinNoise;

    [Header("Perlin Noise Suavizado")]
    public int intervalo = 2;

    [Header("Random Walk Suavizado")]
    public int minimoAnchoSeccion = 2;

    [Header("Cuevas")]
    public bool losBordesSonMuros = true;

    [Header("Perlin Noise Cueva")]
    public float modificador = 0.1f;
    public float OffsetX = 0f;
    public float OffsetY = 0f;

    [Header("Random Walk Cueva")]
    [Range(0f, 1f)]
    public float porcentajeAELiminar = 0.25f;
    public bool movimientoEnDiagonal = false;

    [Header("Tunel Direccional")]
    public int anchoMaximo = 4;
    public int anchoMinimo = 1;
    public int desplazamientoMaximo = 2;
    [Range(0f, 1f)]
    public float aspereza = 0.75f;
    [Range(0f, 1f)]
    public float desplazamiento = 0.75f;

    [Header("Automata Celular")]
    [Range(0f, 1f)]
    public float porcentajeDeRelleno = 0.45f;
    public int totalDePasadas = 3;

    public void GenerarMapa()
    {
        //Limpiamos el mapa de casillas
        mapaDeCasillas.ClearAllTiles();
        //Creamos el array bidireccional del mapa
        int[,] mapa = null;
        //Generamos una semilla nueva de forma aleatoria
        if (semillaAleatoria)
        {
            semilla = Random.Range(0f,1000f);
        }

        switch(algoritmo)
        {
            case Algoritmo.PerlinNoise:
                mapa = Metodos.GenerarArray(ancho, alto, true);
                mapa = Metodos.PerlinNoise(mapa,semilla);
                break;
            case Algoritmo.PerlinNoiseSuavizado:
                mapa = Metodos.GenerarArray(ancho, alto, true);
                mapa = Metodos.PerlinNoiseSuavizado(mapa, semilla,intervalo);
                break;
            case Algoritmo.RandomWalk:
                mapa = Metodos.GenerarArray(ancho, alto, true);
                mapa = Metodos.RandomWalk(mapa, semilla);
                break;
            case Algoritmo.RandomWalkSuavizado:
                mapa = Metodos.GenerarArray(ancho, alto, true);
                mapa = Metodos.RandomWalkSuavizado(mapa, semilla, minimoAnchoSeccion);
                break;
            case Algoritmo.PerlinNoiseCueva:
                mapa = Metodos.GenerarArray(ancho, alto, false);
                mapa = Metodos.PerlinNoiseCueva(mapa, modificador, losBordesSonMuros,OffsetX,OffsetY,semilla);
                break;
            case Algoritmo.RandomWalkCueva:
                mapa = Metodos.GenerarArray(ancho, alto, false);
                mapa = Metodos.RandomWalkCueva(mapa, semilla,porcentajeAELiminar,losBordesSonMuros,movimientoEnDiagonal);
                break;
            case Algoritmo.TunelDireccional:
                mapa = Metodos.GenerarArray(ancho, alto, false);
                mapa = Metodos.TunelDireccional(mapa,semilla,anchoMinimo,anchoMaximo,aspereza,desplazamientoMaximo,desplazamiento);
                break;
            case Algoritmo.MapaAleatorio:
                mapa = Metodos.GenerarMapaAleatorio(ancho, alto, semilla,porcentajeDeRelleno,losBordesSonMuros);
                break;
            case Algoritmo.AutomataCelularMoore:
                mapa = Metodos.GenerarMapaAleatorio(ancho, alto, semilla, porcentajeDeRelleno, losBordesSonMuros);
                mapa = Metodos.AutomataCelularMoore(mapa,totalDePasadas,losBordesSonMuros);
                break;
            case Algoritmo.AutomataCelularVonNeumann:
                mapa = Metodos.GenerarMapaAleatorio(ancho, alto, semilla, porcentajeDeRelleno, losBordesSonMuros);
                mapa = Metodos.AutomataCelularVonNeumann(mapa, totalDePasadas, losBordesSonMuros);
                break;
        }
        Metodos.GenerarMapa(mapa, mapaDeCasillas, casillas);
    }

    public void LimpiarMapa()
    {
        mapaDeCasillas.ClearAllTiles();
    }
}
