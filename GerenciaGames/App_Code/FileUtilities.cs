using System.Collections.Generic;
using System.IO;
using System.Web.UI;

public static class FileUtilities
{

    private readonly static string Path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/jogos.txt";


    public static List<Jogo> LerJogos()
    {
        List<Jogo> listaJogos = new List<Jogo>();


        if (HaArquivoEJogos())
        {
            string[] jogosTexto = File.ReadAllLines(Path);
            int i = 0;

            while (i < jogosTexto.Length)
            {
                Jogo jogo = new Jogo(jogosTexto[i], jogosTexto[i + 1].Replace(" ", ""), jogosTexto[i + 2]);
                listaJogos.Add(jogo);
                i += 3;
            }
            //System.Diagnostics.Debug.WriteLine();
        }

        return listaJogos;
    }

    public static void AddJogoArquivo(Jogo jogo)
    {

        string[] jogoNovo = new string[3];
        jogoNovo[0] = jogo.nome.Trim();
        jogoNovo[1] = jogo.urlImagem.Trim();
        jogoNovo[2] = jogo.sinopse.Trim();
        AppendArquivo(jogoNovo);
    }

    public static bool HaArquivoEJogos()
    {
        return (File.Exists(Path) && File.ReadAllText(Path).Length > 0) ? true : false;
    }

    public static void ExcluirArquivo()
    {
        File.Delete(Path);
    }


    public static int LocalizarIndexJogo(string jogo)
    {
        int indexJogo = -1;
        string[] jogosTexto = File.ReadAllLines(Path);
       


        for (int i = 0; i < jogosTexto.Length; i++)
        {
            if (jogosTexto[i].ToLower().Equals(jogo.ToLower()))
            {
                indexJogo = i;
                return indexJogo;
            }
        }


        return indexJogo;
        
    }

    public static bool ExcluirJogo(string jogo)
    {
        bool jogoExcluido = false;
        int indexJogo = LocalizarIndexJogo(jogo);

        if (indexJogo == -1)
        {
            jogoExcluido = false;
        }
        else
        {
            string[] jogosTexto = File.ReadAllLines(Path);
            jogosTexto[indexJogo+2] = null;
            jogosTexto[indexJogo+1] = null;
            jogosTexto[indexJogo] = null;
            jogosTexto = RemoveAllEmptyValuesArray(jogosTexto);
            AtualizarArquivo(jogosTexto);
        }

        return jogoExcluido;
    }

    public static void AtualizarArquivo(string[] jogos)
    {
        File.WriteAllLines(Path, jogos);
    }

    public static void AppendArquivo(string[] jogos)
    {
        File.AppendAllLines(Path, jogos);
    }

    public static string[] RemoveAllEmptyValuesArray(string[] array)
    {
        string[] result;
        var temp = new List<string>();
        foreach (var s in array)
        {
            if (!string.IsNullOrEmpty(s))
                temp.Add(s);
        }
        result = temp.ToArray();
        return result;
    }

    public static string GetPath()
    {
        return Path;
    }

}