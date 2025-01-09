namespace MagicWords.Domain.Data.Models
{
    public enum CellState
    {
        Normal,
        Blocked,
        Special,
        TempBlocked
    }

    public class Cell
    {
        public char Letter { get; private set; }
        public CellState State { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Cell(char letter, int x, int y)
        {
            Letter = letter;
            State = CellState.Normal;
            X = x;
            Y = y;
        }
        //Métodos para cambiar la letra y el estado si es necesario
        public void SetLetter(char newLetter)
        {
            Letter = newLetter;
        }

        public void SetState(CellState newState)
        {
            State = newState;
        }
    }
}