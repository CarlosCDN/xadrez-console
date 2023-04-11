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
        public Peca vuneravelEnPassant { get; private set; }

        public PartidaDeXadrez() 
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branco;
            terminada = false;
            xeque = false;
            vuneravelEnPassant = null;
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

            // Jogada Especial en passant
            if(p is Peao)
            {
                if(origem.coluna != destino.coluna && pecaCapturada == null) 
                {
                    Posicao posP;
                    if(p.Cor == Cor.Branco)
                    {
                        posP = new Posicao(destino.linha + 1, destino.coluna);
                    }
                    else
                    {
                        posP =new Posicao(destino.linha - 1, destino.coluna);
                    }
                    pecaCapturada = tab.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }

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
            //Jogada Especial en passant
            if(p is Peao)
            {
                if(origem.coluna != destino.coluna && pecaCapturada == vuneravelEnPassant)
                {
                    Peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    if(p.Cor == Cor.Branco) { 
                        posP = new Posicao(3, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.coluna);
                    }
                    tab.colocarPeca(peao, posP);
                }
            }
        }
        public void realizaJogada(Posicao origem, Posicao destino) {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorAtual)) { 
             desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = tab.peca(destino);

            // Jogada Especial promocao
            if(p is Peao)
            {
                if ((p.Cor == Cor.Branco && destino.linha == 0) || (p.Cor == Cor.Preto && destino.linha == 8)) 
                { 
                    p = tab.retirarPeca(destino);
                    pecas.Remove(p);
                    Console.WriteLine("Digite a peça de promoção:");
                    Console.WriteLine("1 - Dama, 2 - Bispo, 3 - Cavalo, 4 -Torre ");
                    int promocao = int.Parse(Console.ReadLine());
                    while (promocao > 0) {

                        if (promocao == 1)
                        {
                            Peca dama = new Dama(tab, p.Cor);
                            tab.colocarPeca(dama, destino);
                            pecas.Add(dama);
                            break;
                        }
                    else if (promocao == 2)
                        {
                            Peca bispo = new Bispo(tab, p.Cor);
                            tab.colocarPeca(bispo, destino);
                            pecas.Add(bispo);
                            break;
                        }
                    else if(promocao == 3)
                        {
                            Peca cavalo = new Cavalo(tab, p.Cor);
                            tab.colocarPeca(cavalo, destino);
                            pecas.Add(cavalo);
                            break;
                        }
                    else if( promocao == 4)
                        {
                            Peca torre = new Torre(tab, p.Cor);
                            tab.colocarPeca(torre, destino);
                            pecas.Add(torre);
                            break;
                        }
                        Console.WriteLine("Opção invalida digite novamente:");
                        Console.WriteLine("Digite a peça de promoção:");
                        Console.WriteLine("1 - Dama, 2 - Bispo, 3 - Cavalo, 4 -Torre ");
                        promocao = int.Parse(Console.ReadLine());

                    }
                }
            }

            if (estaEmXeque(adversario(jogadorAtual)))
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

            // #JogadaEspecial en passant
            if(p is Peao && ( destino.linha == origem.linha - 2 || destino.linha == origem.linha + 2))
            {
                vuneravelEnPassant = p;
            }
            else
            {
                vuneravelEnPassant = null;
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
            colocarNovaPeca('a', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('b', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('c', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('d', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('e', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('f', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('g', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('h', 2, new Peao(tab, Cor.Branco, this));




            //Pretas
            colocarNovaPeca('a', 8, new Torre(tab, Cor.Preto));
            colocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preto));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.Preto));
            colocarNovaPeca('d', 8, new Dama(tab, Cor.Preto));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Preto, this));
            colocarNovaPeca('f', 8, new Bispo(tab, Cor.Preto));
            colocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preto));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Preto));
            colocarNovaPeca('a', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('b', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('c', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('d', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('e', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('f', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('g', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('h', 7, new Peao(tab, Cor.Preto, this));


        }
    }
}
