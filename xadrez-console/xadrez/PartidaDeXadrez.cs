using System.Collections.Generic;
using tabuleiro;
using xadrez;

namespace xadrez
{
     class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }
        public PartidaDeXadrez() 
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branco;
            terminada = false;
            xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }
        public Peca executaMovimento(Posicao origem, Posicao destino) {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQtdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            // Jogada especial roque pequeno
            if(p is Rei && destino.coluna == origem.coluna + 2) {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQtdMovimentos();
                tab.colocarPeca(T, destinoT);
            }
            // Jogada especial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQtdMovimentos();
                tab.colocarPeca(T, destinoT);
            }
            return pecaCapturada;
        }
        public  void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQtdMovimentos();
            if(pecaCapturada != null) { 
            tab.colocarPeca(pecaCapturada, destino); 
                capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);

            // Jogada especial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQtdMovimentos();
                tab.colocarPeca(T, origemT);
            }
            // Jogada especial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQtdMovimentos();
                tab.colocarPeca(T, origemT);
            }
        }
        public void realizaJogada(Posicao Origem, Posicao Destino) {
            Peca pecaCapturada = executaMovimento(Origem, Destino);

            if (estaEmXeque(jogadorAtual)) { 
             desfazMovimento(Origem, Destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }
            if(estaEmXeque(adversario(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }
            if (testeXequeMate(adversario(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();
            }
        }


        public void validarPosicaoOrigem(Posicao pos) {
            if (tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peca na posição de origem escolhida");
            }
            if  (jogadorAtual != tab.peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem não é sua");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possiveis para a peça de origem escolhida!");
            }
        }

        public void validarPosicaoDestino(Posicao origem, Posicao destino) {
        if (!tab.peca(origem).movimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida");
            }
        }
        private void mudaJogador()
        {
            if(jogadorAtual == Cor.Branco)
            {
                jogadorAtual = Cor.Preto;                                         
            }
            else
            {
                jogadorAtual = Cor.Branco;
            }
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux= new HashSet<Peca>();
            foreach(Peca x in capturadas)
            {
                if(x.Cor == cor)
                {
                    aux.Add(x);
                }
            } 
            return aux;
        }

        private Cor adversario (Cor cor)
        {
            if (cor == Cor.Branco)
            {
                return Cor.Preto;
            }
            else 
            {
                return Cor.Branco;
            }
        }
        private Peca rei (Cor cor)
        {
            foreach(Peca x in pecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }
        public HashSet<Peca> pecasEmJogo(Cor cor) {
            {
                HashSet<Peca> aux = new HashSet<Peca>();
                foreach (Peca x in pecas)
                {
                    if (x.Cor == cor)
                    {
                        aux.Add(x);
                    }
                }
                aux.ExceptWith(pecasCapturadas(cor));
                return aux;
            }
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null) 
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }
            foreach (Peca x in pecasEmJogo(adversario(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }
        public bool testeXequeMate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach(Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for(int i = 0; i < tab.linha; i++)
                {
                    for(int j = 0; j < tab.coluna; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public void colocarNovaPeca(char coluna, int linha, Peca peca) {
        tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }
        private void colocarPecas()
        {
            //Brancas
            colocarNovaPeca('a', 1, new Torre(tab, Cor.Branco));
            colocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branco));
            colocarNovaPeca('c', 1, new Bispo(tab, Cor.Branco));
            colocarNovaPeca('d', 1, new Dama(tab, Cor.Branco));
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Branco, this));
            colocarNovaPeca('f', 1, new Bispo(tab, Cor.Branco));
            colocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branco));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Branco));
            colocarNovaPeca('a', 2, new Peao(tab, Cor.Branco));
            colocarNovaPeca('b', 2, new Peao(tab, Cor.Branco));
            colocarNovaPeca('c', 2, new Peao(tab, Cor.Branco));
            colocarNovaPeca('d', 2, new Peao(tab, Cor.Branco));
            colocarNovaPeca('e', 2, new Peao(tab, Cor.Branco));
            colocarNovaPeca('f', 2, new Peao(tab, Cor.Branco));
            colocarNovaPeca('g', 2, new Peao(tab, Cor.Branco));
            colocarNovaPeca('h', 2, new Peao(tab, Cor.Branco));




            //Pretas
            colocarNovaPeca('a', 8, new Torre(tab, Cor.Preto));
            colocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preto));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.Preto));
            colocarNovaPeca('d', 8, new Dama(tab, Cor.Preto));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Preto, this));
            colocarNovaPeca('f', 8, new Bispo(tab, Cor.Preto));
            colocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preto));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Preto));
            colocarNovaPeca('a', 7, new Peao(tab, Cor.Preto));
            colocarNovaPeca('b', 7, new Peao(tab, Cor.Preto));
            colocarNovaPeca('c', 7, new Peao(tab, Cor.Preto));
            colocarNovaPeca('d', 7, new Peao(tab, Cor.Preto));
            colocarNovaPeca('e', 7, new Peao(tab, Cor.Preto));
            colocarNovaPeca('f', 7, new Peao(tab, Cor.Preto));
            colocarNovaPeca('g', 7, new Peao(tab, Cor.Preto));
            colocarNovaPeca('h', 7, new Peao(tab, Cor.Preto));


        }
    }
}
