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
        public abstract bool[,] movimentosPossiveis();
    }
}
