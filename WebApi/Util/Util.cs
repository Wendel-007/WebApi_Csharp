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

        public const string exitoBuscaDeProdutos = "Busca do produto registradoss realizada com sucesso";

        public static object retorno(int codigoHttp, string mensagem, string detalhes)
        {
            string tipo = "desconhecido";
            if(codigoHttp ==  200 || codigoHttp == 201 || codigoHttp == 202 || codigoHttp == 204 )
            {
                tipo = "Sucesso";
            } else
            {
                if(codigoHttp == 400 || codigoHttp == 401 || codigoHttp == 403 || codigoHttp == 404)
                {
                    tipo = "Erro na Requisição";
                } else
                {
                    if (codigoHttp == 500 || codigoHttp == 501 || codigoHttp == 502 || codigoHttp == 503)
                    {
                        tipo = "Erro no Servidor";
                    }
                }
            }
            return new { CodigoHTTP = codigoHttp, Tipo = tipo, Mensagem = mensagem, Detalhes = detalhes };
        }
    }
}
