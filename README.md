# Cálculo do CDB

## Sobre

Projeto com o intuito de calcular o valor do CDB baseando-se em um valor inicial e uma quantidade de meses de rendimento.  
A aplicação faz o cálculo e retorna o valor final **bruto** e **líquido**. 

A operação do CDB reflete a fórmula para um mês:
```
(Valor Final bruto) = (Valor inicial) x [1 +(CDI x TB)]
```

A tabela de imposto a ser considerada é a seguinte:

| Prazo de Investimento | Alíquota de Imposto |
|------------------------|----------------------|
| Até 6 meses            | 22,5%                |
| De 6 a 12 meses        | 20%                  |
| De 12 a 24 meses       | 17,5%                |
| Acima de 24 meses      | 15%                  |

Para este exercício, foram consideradas taxas fixas do banco (TB) como 108% e CDI 0,9%.

![image](https://github.com/user-attachments/assets/039feb74-a355-4b48-8497-809a03de1be4)


---

## Setup

O projeto consiste de:

- Uma **Web API .NET 8** (no diretório `API`) como back-end
- Um **aplicativo Angular** (no diretório `UI`) como front-end

1. Clone o repositório localmente.
2. No diretório `UI`, instale os pacotes necessários para o Angular:
```
npm install
```
3. Faça o build da API pelo Visual Studio ou pelo comando, no diretório `API`
```
dotnet build
```
Após o build, o código se encarregará de atualizar a build do Angular para execução de back e front-end em conjunto. (Alteraçoes poderão ser percebidas na pasta `wwwrooot`)
O projeto .Net poderá ser executado normalmente após esse processo, e o front também estará acessível.

Os endereços de acesso do front-end e documentação do back-end (Swagger) serão os seguintes:
- **Front** - https://localhost:7192/ (ou https://localhost:7192/index.html)
- **Back** - https://localhost:7192/swagger/ (aberto automaticamente)

## Testes

Os testes unitários podem ser executados a partir dos respectivos projetos `API` e `UI`.

- Para o **Back-end .Net**, acesse a janela **Test Explorer** no Visual Studio, ou execute o seguinte comando:
  ```
  dotnet test
  ```
- Para o **Front-end Angular**, executar o seguinte comando na raiz (diretório `UI`) do projeto
  ```
  ng test
  ```
