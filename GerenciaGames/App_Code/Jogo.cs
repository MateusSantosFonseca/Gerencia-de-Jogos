using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Jogo
{
    public string nome;
    public string urlImagem;
    public string sinopse;

    public Jogo(string nome, string urlImagem, string sinopse)
    {
        this.nome = nome;
        this.urlImagem = urlImagem;
        this.sinopse = sinopse;
    }

    public void testeString()
    {
        System.Diagnostics.Debug.WriteLine(this.urlImagem);
    }
    

}