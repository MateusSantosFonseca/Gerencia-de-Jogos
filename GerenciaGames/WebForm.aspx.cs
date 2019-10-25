using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebForm : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (FileUtilities.HaArquivoEJogos())
            {
                List<Jogo> jogosLidos = FileUtilities.LerJogos();
                Label listagem = new Label();
                listagem.Text = "Todos os jogos inseridos no arquivo são:";
                PlaceHolder2.Controls.Add(listagem);
                foreach (Jogo jogo in jogosLidos)
                    AddJogoTela(jogo);
            }
        }
    }


    //essa funcao adiciona jogos na tela
    public void AddJogoTela(Jogo jogo)
    {

        //stacktrace e method base so pra saber qual metodo chamou este metodo
        StackTrace stackTrace = new StackTrace();
        MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();
        Panel panel = new Panel();

        if (methodBase.Name.Equals("Button1_Click"))
            panel.GroupingText = "Jogo adicionado: " + jogo.nome;
        else
            panel.GroupingText = jogo.nome;

        panel.ToolTip = "Título do jogo: " + jogo.nome;

        Image imagemJogo = new Image
        {
            ImageUrl = jogo.urlImagem,
            Height = 350,
            Width = 350,
            ToolTip = "Capa do " + jogo.nome
        };

        TextBox sinopseLable = new TextBox
        {
            BorderStyle = BorderStyle.NotSet,
            BorderWidth = 30,
            BorderColor = System.Drawing.Color.White,
            Width = 550,
            Height = 300,
            Text = jogo.sinopse,
            TextMode = TextBoxMode.MultiLine,
            ReadOnly = true,
            ToolTip = "Sinopse do " + jogo.nome
        };

        panel.Controls.Add(imagemJogo);
        panel.Controls.Add(sinopseLable);

        //se quiser adicionar no topo
        //PlaceHolder1.Controls.AddAt(0, panel);
        PlaceHolder1.Controls.Add(panel);

    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        string imgUrl = (TestUrl(TextBox_URLImagem.Text.Trim())) ? TextBox_URLImagem.Text.Trim() : "https://elitescreens.com/images/product_album/no_image.png";
        Jogo jogo = new Jogo(TextBox_NomeJogo.Text.Trim(), imgUrl, TextBox_Sinopse.Text.Trim().Replace("\r\n", string.Empty));

        TextBox_NomeJogo.Text = "";
        TextBox_URLImagem.Text = "";
        TextBox_Sinopse.Text = "";

        AddJogoTela(jogo);
        FileUtilities.AddJogoArquivo(jogo);
        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Jogo cadastrado!!')", true);

    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        ListarJogos();
    }

    protected void ListarJogos()
    {
        if (FileUtilities.HaArquivoEJogos())
        {
            Label listagem = new Label();
            listagem.Text = "Jogos cadastrados:";
            PlaceHolder2.Controls.Add(listagem);
            List<Jogo> jogosLidos = FileUtilities.LerJogos();
            foreach (Jogo jogo in jogosLidos)
                AddJogoTela(jogo);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ainda não existem jogos cadastrados!!');", true);
        }
    }

    protected void Button_ExcluirTodosJogos_Click(object sender, EventArgs e)
    {
        if (FileUtilities.HaArquivoEJogos())
        {
            FileUtilities.ExcluirArquivo();
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Todos os jogos foram excluidos!!');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Não existem jogos cadastrados para serem excluídos!!');", true);
        }

    }

    protected bool TestUrl(string url)
    {

        return Uri.IsWellFormedUriString(url, UriKind.Absolute);
    }

    protected void Pesquisar_Click(object sender, EventArgs e)
    {
        int indexJogo = FileUtilities.LocalizarIndexJogo(TextBox_SearchGame.Text.TrimEnd());
        TextBox_SearchGame.Text = "";

        if (indexJogo == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Jogo não encontrado');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Jogo encontrado, ele está na " + ((indexJogo / 3) + 1) + "° posição:');", true);
        }

        ListarJogos();
    }

    protected void ButtonRemover_Click(object sender, EventArgs e)
    {
        string jogo = TextBox2_RemoveGame.Text.TrimEnd();
        int indexJogo = FileUtilities.LocalizarIndexJogo(jogo);
        TextBox2_RemoveGame.Text = "";

        if (indexJogo == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Jogo não encontrado, nenhum jogo foi excluído');", true);
        }
        else
        {
            FileUtilities.ExcluirJogo(jogo);
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('O jogo: " + jogo + " foi excluído com sucesso');", true);
        }
        ListarJogos();
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('https://textuploader.com/1kxy7', '_blank');", true);
    }

    protected void BotaoEnviarArquivo_Click(object sender, EventArgs e)
    {
        if (TestaArquivo())
        {
            try
            {
                FileUpload1_txtProprio.PostedFile.SaveAs(FileUtilities.GetPath());
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Arquivo enviado com sucesso!!')", true);

            }
            catch (Exception exception)
            {
                Response.Redirect("ErrorPage.aspx");
                throw new Exception("Erro ao enviar o arquivo: " + exception.Message);
            }

            finally
            {
                FileUpload1_txtProprio.Dispose();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Arquivo não enviado: Formato indesejado ou tamanho excedente!!')", true);
        }
    }

    protected bool TestaArquivo()
    {
        bool ehValido = false;

        if (FileUpload1_txtProprio.PostedFile.ContentLength < 10000000 &&
            System.IO.Path.GetExtension(FileUpload1_txtProprio.FileName).ToLower() == ".txt")
        {
            ehValido = true;
        }

        return ehValido;
    }
}
