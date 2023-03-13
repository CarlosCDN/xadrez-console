﻿using tabuleiro;

namespace xadrez
{
     class Dama: Peca
    {
        public Dama(Tabuleiro tab, Cor cor) : base(tab,cor)
        {
        }

        public override string ToString()
        {
            return "D";
        }
        private bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p == null || p.Cor != Cor;
        }
        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.linha, tab.coluna];

            Posicao pos = new Posicao(0, 0);

            return mat;
        }
    }
}
