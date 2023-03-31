using tabuleiro;

namespace tabuleiro
{
    abstract class Peca 
    {
        public Posicao posicao { get; set; }
        public Cor Cor { get; protected set; }    
        public int qteMovimentos { get; protected set; }  
        public Tabuleiro tab { get; protected set; }  

        public Peca(Tabuleiro tab, Cor cor)
        {
            this.posicao = null;
            Cor = cor;
            this.qteMovimentos = 0;
            this.tab = tab;
        }

        public void incrementarQtdMovimentos() {
            qteMovimentos++;
        }
        public void decrementarQtdMovimentos()
        {
            qteMovimentos--;
        }
        public bool existeMovimentosPossiveis()
        {
            bool[,] mat = movimentosPossiveis();
                for (int i = 0; i < tab.linha; i++)
                {
                    for (int j = 0; j < tab.coluna; j++) {
                        if (mat[i, j]) {
                            return true;
                        }
                    }
                }
                return false;
        }
        public bool podeMoverPara(Posicao pos) {
            return movimentosPossiveis()[pos.linha, pos.coluna];
        }
        public abstract bool[,] movimentosPossiveis();
    }
}
