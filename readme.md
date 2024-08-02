# Sistema para Gestão de Investimentos - InvestSys

É uma aplicação .NET que simula um Sistema de Gestão de Investimentos, permitindo que os usuários gerenciem seus Portfólios de Investimentos, realizando Transações de compra e venda de Ações de Ativos (ações, títulos, criptomoedas etc.). A solução foi desenvolvida utilizando a linguagem de programação C# na versão 6.0 do .NET e banco de dados SQL Server com publicação dentro de um cluster Kubernets e testes sendo executados via pipeline no GitHub Actions.

## Requisitos
Disponibilizar **área logada** utilizando JWT.

Disponibilizar endpoint para inclusão de um novo usuário para uso da aplicação, bem como endopoints para alteração, exclusão e busca por Id do **Usuário**.

Disponibilizar endpoints para inclusão, alteração, exclusão e busca por Id de **Portfólio**.

Disponibilizar endpoints para inclusão, alteração, exclusão e busca por Id de **Ativos**.

Disponibilizar endpoints para inclusão, alteração, exclusão e busca por Id de **Transações**.

### Recursos:
* Gerenciar usuários
* Gerenciar portfólios
* Gerenciar ativos
* Permitir o usuário realizar transações de compra/venda de ativos

### Regras de negócio:
* Para efetuar login no Sistema, o usuário deverá possuir cadastro e ter um e-mail válido para autenticação na aplicação.
* Somentes após efeturar login o usuário poderá gerenciar os Portfólios, bem como realizar Transações de Ativos de compra/venda.
* Para criar um novo Portfólio, deverá ser informado o Usuário.
* Para criação de Ativos, deverá obrigatoriamente selecionar um Tipo de Ativo (Ações, títulos, criptomoedas e etc.).
* Para efetuar uma Transação, deverá ser informado o Portfólio, o Ativo, o Tipo da Transação (compra ou venda), quantidade e preço.

### Tipos de Ativos:

#### Ações:
* Blue Chips: Ações de grandes empresas bem estabelecidas.
* Small Caps: Ações de empresas com menor capitalização de mercado.
* Penny Stocks: Ações de empresas muito pequenas, geralmente negociadas a preços baixos.

#### Títulos:
* Títulos do Tesouro: Incluem títulos como Treasury Bonds, Treasury Notes e Treasury Bills.
* Títulos Corporativos: Títulos emitidos por empresas.
* Títulos Municipais: Títulos emitidos por estados, cidades ou outras entidades locais.

#### Criptomoedas:
* Bitcoin (BTC): A primeira e mais conhecida criptomoeda.
* Ethereum (ETH): Plataforma descentralizada com contratos inteligentes.
* Altcoins: Outras criptomoedas como Litecoin (LTC), Ripple (XRP), etc.

#### Fundos:
* Fundos Mútuos: Coleção de ações, títulos ou outros ativos geridos por uma empresa de investimentos.
* ETFs (Exchange-Traded Funds): Fundos negociados em bolsa que seguem um índice ou setor específico.

#### Derivativos:
* Opções: Contratos que dão ao comprador o direito, mas não a obrigação, de comprar ou vender um ativo a um preço específico antes de uma data de vencimento.
* Futuros: Contratos para comprar ou vender um ativo a um preço acordado no futuro.

#### Commodities:
* 	Metais Preciosos: Ouro, prata, platina.
* 	Energia: Petróleo, gás natural.
* 	Agrícolas: Trigo, milho, café.

#### Imóveis:
* REITs (Real Estate Investment Trusts): Fundos que investem em imóveis e são negociados em bolsa.


### Tipos de Transações:
* Compra (Buy): Aquisição de um ativo.
* Venda (Sell): Venda de um ativo.
* Transferência (Transfer): Movimento de ativos entre contas ou carteiras diferentes.
* Depósito (Deposit): Adição de fundos ou ativos a uma conta ou portfólio.
* Retirada (Withdrawal): Remoção de fundos ou ativos de uma conta ou portfólio.
* Dividendos (Dividends): Pagamentos recebidos de ações ou fundos que distribuem lucro aos acionistas.
* Juros (Interest): Recebimento de juros de títulos ou outros investimentos.
* Splits de Ações (Stock Splits): Divisão de ações existentes em múltiplas ações para aumentar a liquidez.
* Fusão/Aquisição (Mergers/Acquisitions): Incorporação ou aquisição de empresas e seus ativos.
* Contribuição (Contribution): Adição de ativos a um fundo ou portfólio.
* Reinvestimento (Reinvestment): Utilização de dividendos ou outros rendimentos para comprar mais ativos.
* Rebalanceamento (Rebalancing): Ajuste das proporções de ativos em um portfólio para manter a estratégia de investimento.

## Critérios de aceite:

Para cadastro de um novo **usuário** na plataforma, deverá ser informado o nome, e-mail e uma senha:

**POST /User**
```
{
  "name": "João Silva",
  "email": "joao.silva@email.com",
  "password": "********"
}

```

Para efetuar **login**, deverá ser informado o e-mail cadastrado:

**POST / Login**

```
{
  "email": "joao.silva@email.com"
}
```

Após isso, caso o e-mail esteja cadastrado no banco de dados do sistema, o sistema retornará o *token* de acesso que possui validade de 8 horas, conforme exemplo abaixo, com informações do usuário logado:

````
{
  "authenticated": true,
  "created": "2024-07-21 16:29:05",
  "expirationDate": "2024-07-22 00:29:05",
  "acessToken": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6WyJqb2FvLnNpbHZhQGVtYWlsLmNvbSIsImpvYW8uc2lsdmFAZW1haWwuY29tIl0sImp0aSI6ImM5Njg1MWNhLTVmNzUtNDk0YS05ZTlhLWI5NWNhMmUwOTJlZSIsIm5iZiI6MTcyMTU5MDE0NSwiZXhwIjoxNzIxNjE4OTQ1LCJpYXQiOjE3MjE1OTAxNDUsImlzcyI6IkV4ZW1wbG9Jc3N1ZXIiLCJhdWQiOiJFeGVtcGxvQXVkaWVuY2UifQ.cwQz5878YWdfZfSxEIJnEsvHxD__TX0HbWyWSepqBQDvG9fdTc54-fWICZLi40Msra-xWYjbRDmiSbvjVd_Jmd2Ow-bifaYXZEPQSbpK7jLfVP1Nhccgt6GlQLWtT4h6BsEQR61j70pLNU1L81CP-zJRx6irCM82O_zbD-R2e9iucKOVVuRh_tFOOgReX1eIbxkJVUMOsAVpXX214utC8wqQhnyCxoY12cM1V9QMux2UYj2B8imVo0NAOC7n50FW8BZ8urOEgugX45y8ER0i4biZTUW6qCwe0T-QGA6pkFMvbfY2FWVEmQgrUBheTc6kKQxpbrCq5HAngM6kowD3vg",
  "userName": "joao.silva@email.com",
  "name": "João Silva",
  "message": "Usuário Logado com sucesso"
}
````

Deverá ser copiado o *acessToken* e clicar no botão **Authorize** passando as informações necessárias conforme exemplo abaixo:

````
Bearer (apiKey)
Entre com o Token JWT
Name: Authorization
In: header
Value: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6WyJqb2FvLnNpbHZhQGVtYWlsLmNvbSIsImpvYW8uc2lsdmFAZW1haWwuY29tIl0sImp0aSI6ImM5Njg1MWNhLTVmNzUtNDk0YS05ZTlhLWI5NWNhMmUwOTJlZSIsIm5iZiI6MTcyMTU5MDE0NSwiZXhwIjoxNzIxNjE4OTQ1LCJpYXQiOjE3MjE1OTAxNDUsImlzcyI6IkV4ZW1wbG9Jc3N1ZXIiLCJhdWQiOiJFeGVtcGxvQXVkaWVuY2UifQ.cwQz5878YWdfZfSxEIJnEsvHxD__TX0HbWyWSepqBQDvG9fdTc54-fWICZLi40Msra-xWYjbRDmiSbvjVd_Jmd2Ow-bifaYXZEPQSbpK7jLfVP1Nhccgt6GlQLWtT4h6BsEQR61j70pLNU1L81CP-zJRx6irCM82O_zbD-R2e9iucKOVVuRh_tFOOgReX1eIbxkJVUMOsAVpXX214utC8wqQhnyCxoY12cM1V9QMux2UYj2B8imVo0NAOC7n50FW8BZ8urOEgugX45y8ER0i4biZTUW6qCwe0T-QGA6pkFMvbfY2FWVEmQgrUBheTc6kKQxpbrCq5HAngM6kowD3vg
````

Após isso, os demais endpoints da aplicação estarão liberados para uso.

Para criação de um **ativo**, deverá ser informado o tipo do ativo, nome e código, conforme exemplo abaixo:

**POST /Active**

````
{
  "activeType": 2,
  "name": "Ativo de criptomoedas - Bitcoin",
  "code": "BTC"
}
````

Para criação de um novo **portfólio** de investimentos, deverá ser informado o Id do usuário, nome do portfólio e a descrição, conforme exemplo abaixo:

**POST /Portfolio**

````
{
  "userId": 16,
  "name": "Portfólio de Investimentos em Criptomoedas",
  "description": "Portfólio diversificado com as principais criptomoedas, incluindo Bitcoin, Ethereum e altcoins emergentes."
}
````

Para realizar uma **transação**, deverá ser informado o código do portfólio, código do ativo, tipo da transação, quantidade e preço, conforme exemplo abaixo:

**POST /Transaction**

````
{
  "portfolioId": 20,
  "activeId": 9,
  "transactionType": 1,
  "quantity": 3,
  "price": 90000,
  "date": "2024-07-21T19:48:35.461Z"
}
````

## Execução:
Abra a solução (InvestPortfolio.sln), preferencialmente, na versão 2022 ou posterior do Microsoft Visual Studio.

Ter instalado o SQL Server Management Studio, de preferência a versão 19 ou posterior.

Altere a string de conexão (ConnectionString) da base de dados:

````
Path: InsvestPortfolio\PortfolioService\Consumers\API\appsettings.json

"ConnectionStrings": {
    "Main": "Data Source=(localdb)\\MSSQLLocalDB;Database=PortfolioManagement;Integrated Security=true"
},
````

## Banco de dados:
Utilizar o Entity Framework (pasta Migrations no projeto BookingService):
````
PortfolioService\Adpters\Data
````
Abra o Console do Gerenciador de Pacotes e gere a Migration:
````
Add-Migration MigrationInvestSys
Update-Database
````
Diagrama do Banco de Dados:

![image](https://hackmd.io/_uploads/SJ67cJsOC.png)

## Diagramas
Para melhor visualização e navegação nos diagramas, realizar login no site:
* [https://app.diagrams.net/](https://app.diagrams.net/)

Link dos diagramas e apresentação do projeto:
* [https://drive.google.com/file/d/1VeDI75nii4_PM9WByKpALJZ_3XmrxEvx/view?usp=sharing](https://drive.google.com/file/d/1VeDI75nii4_PM9WByKpALJZ_3XmrxEvx/view?usp=sharing)
