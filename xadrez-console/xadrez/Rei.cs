using tabuleiro;

namespace xadrez
{
    class Rei: Peca
    {
        public Rei(Tabuleiro tab, Cor cor) : base(tab, cor)
        {
        }
            public override string ToString()
        {
            return "R";
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

           

            //norte
            pos.definirValores(posicao.linha - 1, posicao.coluna);
            if((tab.posicaoValida(pos) && podeMover(pos)))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //nordeste
            pos.definirValores(posicao.linha - 1, posicao.coluna + 1);
            if ((tab.posicaoValida(pos) && podeMover(pos)))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            // leste 
            pos.definirValores(posicao.linha, posicao.coluna + 1);
            if ((tab.posicaoValida(pos) && podeMover(pos)))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //sudeste
            pos.definirValores(posicao.linha + 1, posicao.coluna + 1);
            if ((tab.posicaoValida(pos) && podeMover(pos)))
            {
                mat[pos.linha, pos.coluna] = true;
            }
           
            //sul
            pos.definirValores(posicao.linha + 1, posicao.coluna);
            if ((tab.posicaoValida(pos) && podeMover(pos)))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //sudoeste
            pos.definirValores(posicao.linha + 1, posicao.coluna - 1);
            if ((tab.posicaoValida(pos) && podeMover(pos)))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //oeste
            pos.definirValores(posicao.linha, posicao.coluna - 1);
            if ((tab.posicaoValida(pos) && podeMover(pos)))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //noroeste         
            pos.definirValores(posicao.linha - 1, posicao.coluna - 1);
            if ((tab.posicaoValida(pos) && podeMover(pos)))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            return mat;

        }
    }
    
}
