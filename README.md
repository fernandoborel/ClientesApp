# ClientesApp API

> API RESTful para gerenciamento de clientes, construída com **.NET 10**, **ASP.NET Core**, **Entity Framework Core** e documentada via **Swagger UI** e **Scalar**. Containerizada com **Docker** e publicada no **Docker Hub**, com pipeline de CI/CD para **Azure** em desenvolvimento.

---

## Índice

- [Visão Geral](#-visão-geral)
- [Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [Arquitetura e Estrutura do Projeto](#-arquitetura-e-estrutura-do-projeto)
- [Modelo de Domínio](#-modelo-de-domínio)
- [Endpoints da API](#-endpoints-da-api)
- [Como Executar Localmente](#-como-executar-localmente)
- [Docker Hub](#-docker-hub)
- [Azure — Hospedagem em Nuvem](#-azure--hospedagem-em-nuvem)
- [Documentação Interativa](#-documentação-interativa)
- [Pontos de Melhoria e Roadmap Técnico](#-pontos-de-melhoria-e-roadmap-técnico)

---

## Visão Geral

O **ClientesApp** é uma Web API de CRUD completo para a entidade `Cliente`. O projeto foi desenvolvido com foco em:

- Simplicidade e clareza de código
- Uso das features mais recentes do .NET 10
- Containerização com Docker para portabilidade total
- Documentação de API de alta qualidade com Swagger e Scalar

---

## Tecnologias Utilizadas

| Tecnologia | Versão | Finalidade |
|---|---|---|
| .NET / ASP.NET Core | 10.0 | Framework principal |
| Entity Framework Core | 10.0.8 | ORM — acesso a dados |
| EF Core InMemory | 10.0.8 | Banco de dados em memória (dev/test) |
| Swashbuckle (Swagger) | 10.1.7 | Documentação interativa da API |
| Scalar | 2.14.14 | UI alternativa para OpenAPI |
| Docker | — | Containerização da aplicação |
| Docker Hub | — | Registro e distribuição da imagem |
| Azure | — | Hospedagem em nuvem *(pipeline em desenvolvimento)* |

---

## Arquitetura e Estrutura do Projeto

```
ClientesApp/
??? ClientesApp.API/
?   ??? Controllers/
?   ?   ??? ClientesController.cs   # Camada de apresentação — endpoints REST
?   ??? Contexts/
?   ?   ??? DataContext.cs          # DbContext do EF Core
?   ??? Models/
?   ?   ??? Cliente.cs              # Entidade de domínio
?   ??? Program.cs                  # Bootstrap da aplicação
??? Dockerfile                      # Imagem multi-stage para produção
??? docker-compose.yml              # Orquestração local dos containers
??? docker-compose.override.yml     # Overrides para ambiente de desenvolvimento
```

O projeto segue uma estrutura **monolítica simplificada** — adequada para o estágio atual — com clara separação entre as responsabilidades de apresentação (`Controllers`), persistência (`Contexts`) e domínio (`Models`).

---

## Modelo de Domínio

### `Cliente`

| Propriedade | Tipo | Descrição |
|---|---|---|
| `Id` | `Guid` | Identificador único, gerado automaticamente |
| `Nome` | `string` | Nome completo do cliente |
| `Email` | `string` | Endereço de e-mail do cliente |
| `DataHoraCadastro` | `DateTime` | Timestamp do cadastro, preenchido automaticamente |

---

## Endpoints da API

**Base URL:** `http://localhost:5050/api/v1/clientes`

| Método | Rota | Descrição | Código de Sucesso |
|---|---|---|---|
| `POST` | `/api/v1/clientes` | Cadastra um novo cliente | `201 Created` |
| `PUT` | `/api/v1/clientes/{id}` | Atualiza dados de um cliente existente | `200 OK` |
| `DELETE` | `/api/v1/clientes/{id}` | Remove um cliente pelo ID | `200 OK` |
| `GET` | `/api/v1/clientes` | Lista todos os clientes (ordenados por cadastro) | `200 OK` / `204 No Content` |
| `GET` | `/api/v1/clientes/{id}` | Busca um cliente pelo ID | `200 OK` / `204 No Content` |

### Request Body (POST / PUT)

```json
{
  "nome": "João da Silva",
  "email": "joao.silva@email.com"
}
```

### Exemplo de Response (POST — 201)

```json
{
  "message": "Cliente cadastrado com sucesso.",
  "cliente": {
    "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "nome": "João da Silva",
    "email": "joao.silva@email.com",
    "dataHoraCadastro": "2025-07-14T10:30:00"
  }
}
```

---

## Como Executar Localmente

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Opção 1 — Via .NET CLI

```bash
# Clone o repositório
git clone https://github.com/fernandoborel/ClientesApp.git
cd ClientesApp

# Execute a API
dotnet run --project ClientesApp.API
```

A API estará disponível em: `https://localhost:7xxx` / `http://localhost:5xxx`

### Opção 2 — Via Docker Compose (Recomendado)

```bash
# Clone o repositório
git clone https://github.com/fernandoborel/ClientesApp.git
cd ClientesApp

# Suba o container
docker-compose up -d

# Verifique os containers em execução
docker ps
```

A API estará disponível em: `http://localhost:5050`

### Parar os containers

```bash
docker-compose down
```

---

## Docker Hub

A imagem oficial do projeto está publicada no **Docker Hub** e pode ser utilizada diretamente, sem necessidade de build local:

```bash
docker pull sergiocoti/clientes-app:latest
```

Para rodar a imagem isoladamente:

```bash
docker run -d -p 5050:8080 --name clientesapp sergiocoti/clientes-app:latest
```

 **Imagem:** [`sergiocoti/clientes-app`](https://hub.docker.com/r/sergiocoti/clientes-app)

---

## Azure — Hospedagem em Nuvem

O projeto está sendo integrado ao ecossistema **Microsoft Azure** para garantir alta disponibilidade, escalabilidade e CI/CD automatizado.

>  **Status atual:** As pipelines de deploy contínuo estão em desenvolvimento.

### Infraestrutura planejada

| Serviço Azure | Finalidade |
|---|---|
| **Azure Container Apps** | Hospedagem do container Docker em ambiente serverless gerenciado |
| **Azure Container Registry (ACR)** | Registro privado de imagens Docker |
| **Azure Pipelines / GitHub Actions** | CI/CD — build, push da imagem e deploy automatizado |
| **Azure Monitor / Application Insights** | Observabilidade, rastreamento de requisições e alertas |

### Fluxo de Deploy Planejado

```
Push para branch main
        ?
        ?
  GitHub Actions (CI)
  ??? dotnet build
  ??? dotnet test
  ??? docker build & push ? ACR / Docker Hub
        ?
        ?
  Azure Container Apps (CD)
  ??? Pull da nova imagem ? Deploy automático
```

---

## Documentação Interativa

Com a aplicação em execução, acesse a documentação da API pelos seguintes endereços:

| Interface | URL |
|---|---|
| **Swagger UI** | `http://localhost:5050/swagger` |
| **Scalar UI** | `http://localhost:5050/scalar/v1` |
| **OpenAPI JSON** | `http://localhost:5050/openapi/v1.json` |

---

## Licença

Este projeto está sob a licença MIT. Consulte o arquivo `LICENSE` para mais detalhes.

---

<div align="center">
  <sub>Desenvolvido com <3 usando .NET 10 | Containerizado com Docker | Hospedado no Azure</sub>
</div>
