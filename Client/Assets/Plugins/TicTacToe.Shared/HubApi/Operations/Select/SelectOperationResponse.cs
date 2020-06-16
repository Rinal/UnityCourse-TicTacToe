namespace TicTacToe.Shared
{
    public sealed class SelectOperationResponse : BaseResponse
    {
        public SelectOperationResponse() : this(string.Empty)
        {
        }

        public SelectOperationResponse(string error) : base(error)
        {
        }
    }
}