using tabuleiro;
using xadrez_console;
using System;
using xadrez;



try 
    {
        PartidaDeXadrez partida = new PartidaDeXadrez();
    while (!partida.terminada)
    {
        try {
            Console.Clear();
            Tela.ImprimirTabuleiro(partida.tab);
            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.turno);
            Console.WriteLine("Aguardando jogada: " + partida.jogadorAtual);
            Console.Write("Origem: ");
            Posicao origem = Tela.lerPosicaoXadrez().toPosicao();
            partida.validarPosicaoOrigem(origem);
            bool[,] posicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();

            Console.Clear();
            Tela.ImprimirTabuleiro(partida.tab, posicoesPossiveis); ;

            Console.Write("Destino: ");
            Posicao destino = Tela.lerPosicaoXadrez().toPosicao();
            partida.validarPosicaoDestino(origem, destino);

            partida.realizaJogada(origem, destino);
        } catch (TabuleiroException e) {
            Console.WriteLine(e.Message);
            Console.ReadLine();
        }
    }
    
    } catch (TabuleiroException e) { 
        Console.WriteLine(e.Message);
    }
        Console.Read();
