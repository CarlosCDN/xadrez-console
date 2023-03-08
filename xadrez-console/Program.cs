using tabuleiro;
using xadrez_console;
using System;
using xadrez;

    try 
    {
        Tabuleiro tab = new Tabuleiro(8, 8);
    tab.colocarPeca(new Torre(tab, Cor.Preto), new Posicao(0,0));
    tab.colocarPeca(new Torre(tab, Cor.Preto), new Posicao(1, 3));
    tab.colocarPeca(new Rei(tab, Cor.Preto), new Posicao(0, 2));

    tab.colocarPeca(new Torre(tab, Cor.Branco), new Posicao(3, 5));

    Tela.ImprimirTabuleiro(tab);
    
    } catch (TabuleiroException e) { 
        Console.WriteLine(e.Message);
    }
        Console.Read();
