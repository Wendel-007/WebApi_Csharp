const url = "http://localhost:5125"; // Substitua pela URL da sua API

// Fazendo uma requisição GET
fetch(url)
    .then(response => response.json())
    .then(data => {
        // Manipule os dados da API aqui
        console.log(data); // Imprime o JSON no console
        // Você também pode exibir os dados na tela, por exemplo:
        const jsonContainer = document.getElementById("json-container");
        jsonContainer.textContent = JSON.stringify(data, null, 2); // Pretty-print com 2 espaços de indentação
    })
    .catch(error => {
        console.error("Erro ao buscar dados da API:", error);
    });
