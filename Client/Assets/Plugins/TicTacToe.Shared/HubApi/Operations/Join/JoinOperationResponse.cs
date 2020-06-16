namespace TicTacToe.Shared
{
    public sealed class JoinOperationResponse : BaseResponse
    {
        public JoinOperationResponse() : this(string.Empty)
        {
        }

        public JoinOperationResponse(string error) : base(error)
        {
        }
    }
}