namespace WebApi.Util
{
    public static class Util
    {
        public const string idInvalido = "O ID da requisição é invalido.";

        public const string idNaoEncontrado = "O ID da requisição não foi encontrado.";

        public const string exitoDelecao = "Delecao do produto com ID realizada com sucesso.";

        public const string erroDelecao = "Um erro interno ocorreu durante a delecao do produto.";

        public const string exitoCriacao = "Criação do Produto realizada com sucesso.";

        public const string erroCriacao = "Um erro ocorreu durante a criacao desse produto.";

        public const string exitoAtualizacao = "Atualizacao do produto realizada com sucesso.";

        public const string erroAtualizacao = "Um erro ocorreu durante a atualizacao desse produto.";

        public const string erroBuscaPorId = "Um erro interno ocorreu durante a busca do produto por ID.";

        public const string exitoBuscaPorId = "Busca do produto por ID realizada com sucesso";

        public const string erroBuscaDeProdutos = "Um erro interno ocorreu durante a busca dos produto registrados.";

        public const string exitoBuscaDeProdutos = "Busca do produtos registrados realizada com sucesso";

        public const string exitoAutenticacao = "Autenticacao realizada com sucesso";

        public const string invalidaAutenticacao = "Autenticação invalida pois Usuario e/ou Senha invalido(s)";

        public const string erroAutenticacao = "Um erro ocorreu durante sua autenticação";

        public static object msgRetorno(int codigoHttp, string mensagem, object conteudo)
        {
            return new { StatusHTTP = codigoHttp, Mensagem = mensagem, Conteudo = conteudo };
        }

        public static object msgRetorno(int codigoHttp, string mensagem, string conteudo)
        {
            return new { StatusHTTP = codigoHttp, Mensagem = mensagem, Conteudo = conteudo };
        }
    }
}
